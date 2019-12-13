using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_next : ClientHandler
    {
        public send_battle_attack_next(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_attack_next;



        public override void Handle(NecClient client, NecPacket packet)
        {
            if (client.Character.battleNext == 0)
            { 
                client.Character.battleAnim = 232; // 232 is the '2nd' attack animation for all weapons.  
                client.Character.battleNext = 1;
            }
            else if (client.Character.battleNext == 1)
            {                
                client.Character.battleAnim = 233; // 233 is the '2nd' attack animation for all weapons. 
                client.Character.battleNext += 1;
            }
            else if (client.Character.battleNext == 2)
            {
                client.Character.battleAnim = 234; // 234 is the '2nd' attack animation for some weapons. 
                client.Character.battleNext += 1;
            }



            //client.Character.battleAnim += 1; // increment the battleAnim to the 'next' one.  231 and 232 are common.  1h weapons also have 233 and 234


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //0 means success
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_attack_next_r, res, ServerType.Area, client);   
        }
    }
}
