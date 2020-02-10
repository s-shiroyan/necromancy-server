using Necromancy.Server.Common.Instance;
using System;


namespace Necromancy.Server.Model.Union
{
    public class UnionMember : IInstance
    {
        public uint InstanceId { get; set; }
        public uint UnionId { get; set; }
        public uint CharacterDatabaseId { get; set; }

        public uint CharacterInstanceId { get; set; }
        public uint MemberPriviledgeBitMask { get; set; }

        public DateTime Joined { get; set; }

        public UnionMember()
        {
            InstanceId = 0;
            UnionId = 0;
            CharacterDatabaseId = 0;
            CharacterInstanceId = 0;
            MemberPriviledgeBitMask = 0b01100111;
            Joined = DateTime.Now;

        }

    }

}
