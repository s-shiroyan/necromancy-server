using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_wanted_entry : ClientHandler
    {
        public send_wanted_entry(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_wanted_entry;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetSoulId = packet.Data.ReadInt32();
            int targetBountyGold = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Error Check
            Router.Send(client, (ushort)AreaPacketId.recv_wanted_entry_r, res, ServerType.Area);
 
            NecClient bountyTargetclient = Server.Clients.GetBySoulName(Server.Database.SelectSoulById(targetSoulId).Name);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(bountyTargetclient.Character.InstanceId); //Character Instance Id to receive notification of bounty placement
            res2.WriteInt32(6); //?? probably wanted state. not gold ammount
            Router.Send(bountyTargetclient, (ushort)AreaPacketId.recv_wanted_update_state_notify, res2, ServerType.Area);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(1);
            Router.Send(bountyTargetclient, (ushort)AreaPacketId.recv_wanted_update_state, res3, ServerType.Area);

            //This goes to whom-ever killed the bountied target and gets the reward.
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt64(targetBountyGold);
            res4.WriteInt64(targetBountyGold);
            Router.Send(bountyTargetclient/*BountyKillerClient*/, (ushort)AreaPacketId.recv_wanted_update_reward_point, res4, ServerType.Area);

            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(0);
            res5.WriteCString(bountyTargetclient.Character.Name); // Length 0x31
            res5.WriteInt32(bountyTargetclient.Character.InstanceId);
            Router.Send(bountyTargetclient.Map, (ushort)AreaPacketId.recv_wanted_update_state_actor_notify, res5, ServerType.Area);

        }
    }
}
