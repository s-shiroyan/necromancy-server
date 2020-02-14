using Necromancy.Server.Common.Instance;
using System.Collections.Generic;
using System;


namespace Necromancy.Server.Model.Union
{
    public class UnionNewsEntry : IInstance
    {
        public uint InstanceId { get; set; }
        public string CharacterSoulName { get; set; }

        public string CharacterName { get; set; }
        public uint Activity { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public int ItemCount { get; set; }

        public UnionNewsEntry()
        {
            InstanceId = 0;
            CharacterSoulName = "";
            CharacterName = "";
            String3 = "";
            String4 = "";
            ItemCount = 0;
            

        }

    }

}
