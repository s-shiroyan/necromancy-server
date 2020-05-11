using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_enter : ClientHandler
    {
        public send_map_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
            {
                // This requires database changes to add the GGates to the Npc database!!!!!
                if (npcSpawn.Name == "GGate")
                {
                    GGateSpawn gGate = new GGateSpawn();
                    gGate.X = npcSpawn.X;
                    gGate.Y = npcSpawn.Y;
                    gGate.Z = npcSpawn.Z;
                    gGate.Heading = npcSpawn.Heading;
                    gGate.MapId = npcSpawn.MapId;
                    gGate.Name = npcSpawn.Name;
                    gGate.Title = npcSpawn.Title;

                    RecvDataNotifyGGateData gGateData = new RecvDataNotifyGGateData(gGate);
                    //    Router.Send(gGateData, client);
                }
                else
                {
                    RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                    Router.Send(npcData, client);
                }
            }
            
            
            
            
            
            
            
            
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check. must be 0
            res.WriteByte(0); //Bool - play cutscene. 1 yes, 0 no?
            Router.Send(client, (ushort) AreaPacketId.recv_map_enter_r, res, ServerType.Area);
        }

        private void SendDataNotifyCharaData(NecClient client, NecClient thisNecClient)
        {
            SendMapBGM(client);
            client.Character.weaponEquipped = false;
        }

        private void SendMapBGM(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(100401);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_map_update_bgm, res, ServerType.Area, client);
        }
    }
}
