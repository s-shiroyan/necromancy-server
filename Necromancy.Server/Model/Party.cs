using Necromancy.Server.Data.Setting;
using Necromancy.Server.Common.Instance;
using System.Collections.Generic;

namespace Necromancy.Server.Model
{
    public class Party : IInstance
    {
        public uint InstanceId { get; set; }
        public int PartyType { get; set; }
        public int NormalItemDist { get; set; }
        public int RareItemDist { get; set; }
        public uint TargetClientId { get; set; }
        public uint PartyLeaderId { get; set; }
        public List<NecClient> PartyMembers { get; set; }
        public Party()
        {
            PartyMembers = new List<NecClient>();
            PartyLeaderId = 0;
            NormalItemDist = 1;
            RareItemDist = 1;
            TargetClientId = 0;

        }

        public void Join(NecClient client)
        {
            PartyMembers.Add(client);
        }
        public void Leave(NecClient client)
        {
            PartyMembers.Remove(client); //to-do  try/catch
        }
        public void Leave(List<NecClient> PartyMembers)
        {
            foreach (NecClient client in PartyMembers)
                PartyMembers.Remove(client); //to-do  try/catch
        }

    }

}
