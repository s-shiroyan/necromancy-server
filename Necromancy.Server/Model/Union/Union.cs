using Necromancy.Server.Common.Instance;
using System.Collections.Generic;
using System;


namespace Necromancy.Server.Model.Union
{
    public class Union : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int UnionLeaderId { get; set; }
        public uint UnionSubLeader1Id { get; set; }
        public uint UnionSubLeader2Id { get; set; }
        public uint Level { get; set; }
        public uint CurrentExp { get; set; }
        public uint NextLevelExp { get; set; }
        public byte MemberLimitIncrease { get; set; }
        public short CapeDesignID { get; set; }
        public string UnionNews { get; set; }
        public DateTime Created { get; set; }


        public List<NecClient> UnionMembers { get; set; }
        public List<UnionNewsEntry> UnionNewsEntries { get; set; }
        public Union()
        {
            UnionMembers = new List<NecClient>();
            Id = 1;
            Name = "";
            UnionLeaderId = 0;
            Level = 0;
            CurrentExp = 0;
            NextLevelExp = 100;
            MemberLimitIncrease = 0;
            CapeDesignID = 0;
            Created = DateTime.Now;

        }

        public void Join(NecClient client) //for establish and join
        {
            UnionMembers.Add(client);
        }
        public void Leave(NecClient client) //for Kick and succeed
        {
            UnionMembers.Remove(client); //to-do  try/catch
        }
        public void Leave(List<NecClient> UnionMembers) //for disband
        {
            foreach (NecClient client in UnionMembers)
                UnionMembers.Remove(client); //to-do  try/catch
        }
        public void AddNews(UnionNewsEntry addEntry) //for adding news
        {
            UnionNewsEntries.Add(addEntry);
        }
        public void RemoveNews(UnionNewsEntry removeEntry) //for adding news
        {
            UnionNewsEntries.Remove(removeEntry);
        }

    }
}
