using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_draw_bonuspoint : Handler
    {
        public send_chara_draw_bonuspoint(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_draw_bonuspoint;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint unknown = packet.Data.ReadByte();
            Logger.Info($"Unknown: {unknown}");

            byte bonusPoints;
            

            int rangeDetermination = Util.GetRandomNumber(0,10001);
            // ^^ generate a number between 0 and 10000 to determinate the bonus points range

            if (rangeDetermination == 10000)
            {
                bonusPoints = 1;
            }
            else if (rangeDetermination >= 9979)
            {
                bonusPoints = (byte)Util.GetRandomNumber(21, 31);
            }
            else if (rangeDetermination >= 9879)
            {
                bonusPoints = (byte)Util.GetRandomNumber(16, 21);
            }
            else if (rangeDetermination >= 9679)
            {
                bonusPoints = (byte)Util.GetRandomNumber(10, 16);
            }
            else
            {
                bonusPoints = (byte)Util.GetRandomNumber(1, 10);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(bonusPoints); // Number of points

            Router.Send(client, (ushort) MsgPacketId.recv_chara_draw_bonuspoint_r, res);
        }
    }
}