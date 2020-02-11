using Necromancy.Server.Common.Instance;
using System;


namespace Necromancy.Server.Model.Union
{
    public class UnionMember : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int UnionId { get; set; }
        public int CharacterDatabaseId { get; set; }
       // public uint CharacterInstanceId { get; set; }
        public uint MemberPriviledgeBitMask { get; set; }

        public DateTime Joined { get; set; }

        public UnionMember()
        {
            InstanceId = 0;
            Id = -1;
            UnionId = 0;
            CharacterDatabaseId = 1;
            //CharacterInstanceId = 0;
            MemberPriviledgeBitMask = 0b01100111;
            Joined = DateTime.Now;

        }

    }

}
