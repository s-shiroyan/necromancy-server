using Necromancy.Server.Data.Setting;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Party : IInstance
    {
        public Party()
        {
        }

        public uint InstanceId { get; set; }
        public int PartyType { get; set; }
        public int NormalItemDist { get; set; }
        public int RareItemDist { get; set; }
        public uint TargetClientId { get; set; }

    }

}
