using Necromancy.Server.Data.Setting;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Bag : IInstance64
    {
        public Bag()
        {
        }

        public ulong InstanceId { get; set; }
        public int BagId { get; set; }
        public byte StorageId { get; set; }
        public short NumSlots { get; set; }
    }

    //BagId
    //50100501,Bag(S)
    //50100502,Bag(M)
    //50100503,Pouch
    //50100504,Search Bag(S)
    //50100505,Search Bag(M)
    //50100506,Guardian Bag
    //50100511,Jewel Bag(S)
    //50100512,Jewel Bag(M)
}
