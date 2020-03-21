using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

using Necromancy.Server.Database;
using Necromancy.Server.Database.Sql;

namespace Necromancy.Test.Database.Sql
{
    public class SqlMigratorTest
    {
        private const string SQLITE_FILE = "TestMigrations/db.sqlite";
        private const string MIGRATION_DIR = "TestMigrations/Script/Migrations/";
        private const string FIRST = MIGRATION_DIR + "1-first.sql";
        private const string SECOND = MIGRATION_DIR + "2.sql";
        /* 0 is a sample and will be silently ignored. */
        private const string IGNORE_ZERO = MIGRATION_DIR + "0.sql";
        /* Non-sql files should be ignored. */
        private const string IGNORE_TEXT = MIGRATION_DIR + "notsql.txt";
        /* Same version, will throw ArgumentException. */
        private const string FAIL_DUPLICATE = MIGRATION_DIR + "1-dup.sql";
        /* No version, will throw FormatException. */
        private const string FAIL_NOVERSION = MIGRATION_DIR + "noversion.sql";

        private readonly ITestOutputHelper _output;

        public SqlMigratorTest(ITestOutputHelper output)
        {
            _output = output;

            /* Create an artificial migrations environment to test specific requirements. */
            PrepFiles();
        }

        [Fact]
        public void TestSqlLite()
        {
            IDatabase db = new NecSqLiteDb(SQLITE_FILE);
            db.CreateDatabase();
            Assert.Equal(0, db.Version);

            /* Valid case, no errors. */
            TestValid(db);

            /* Test sync */
            IDatabase db2 = new NecSqLiteDb(SQLITE_FILE);
            db.CreateDatabase();
            db.Version = 11;
            Assert.Equal(11, db.Version);
            Assert.Equal(11, db2.Version);
            db2.Version = 12;
            Assert.Equal(12, db.Version);
            Assert.Equal(12, db2.Version);

            /* Reset */
            db.Version = 0;
            Assert.Equal(0, db.Version);

            /* Invalid case, errors are thrown. */
            TestInvalid(db);
        }

        [Fact]
        public void TestSortedKeys()
        {
            /* Prep data random */
            Random random = new Random();
            int length = random.Next(5, 100);
            int current;
            int[] versionList = new int[length];
            SortedList<int, string> list = new SortedList<int, string>();
            for (int i = 0; i < length; i++)
            {
                current = random.Next(1, 100);
                versionList[i] = current;
                if (!list.ContainsKey(current))
                    list[current] = "NONAME";
            }

            /* Finally assert list iterates in the sorted order. */
            current = 0;
            foreach (KeyValuePair<int, string> kvp in list)
            {
                Assert.True(kvp.Key > current);
                Assert.Equal("NONAME", kvp.Value);
                current = kvp.Key;
            }

            _output.WriteLine(String.Format(
                "SortedList iterates successfully in sorted order for {0} integers, and a total of {1} inserted values.",
                length, list.Count));
        }

        private void TestValid(IDatabase db)
        {
            SqlMigrator migrator = new SqlMigrator(db);
            migrator.Migrate(MIGRATION_DIR);
            /* Highest migration version = 2. */
            Assert.Equal(2, db.Version);
            db.Version = 0;
            Assert.Equal(0, db.Version);
        }

        private void TestInvalid(IDatabase db)
        {
            SqlMigrator migrator = new SqlMigrator(db);

            /* FAIL_DUPLICATE migration file is a version duplicate. */
            Create(FAIL_DUPLICATE);
            Assert.Throws<ArgumentException>(() =>
            {
                migrator.Migrate(MIGRATION_DIR);
            });
            File.Delete(FAIL_DUPLICATE);

            /* FAIL_NOVERSION migration file has an invalid name, with no version. */
            Create(FAIL_NOVERSION);
            Assert.Throws<FormatException>(() =>
            {
                migrator.Migrate(MIGRATION_DIR);
            });
            File.Delete(FAIL_NOVERSION);
        }

        private void PrepFiles()
        {
            if (!Directory.Exists(MIGRATION_DIR))
                Directory.CreateDirectory(MIGRATION_DIR);
            Create(FIRST);
            Create(SECOND);
            Create(IGNORE_ZERO);
            Create(IGNORE_TEXT);

            /* Clean the runs with errors. */
            if (File.Exists(FAIL_DUPLICATE))
                File.Delete(FAIL_DUPLICATE);
            if (File.Exists(FAIL_NOVERSION))
                File.Delete(FAIL_NOVERSION);
        }

        private void Create(string fileName)
        {
            if (!File.Exists(fileName))
                File.Create(fileName).Close();
        }
    }
}
