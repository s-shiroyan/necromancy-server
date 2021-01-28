using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_item_scroll_sub_enchant_check_r : PacketResponse
    {
        public recv_item_scroll_sub_enchant_check_r()
            : base((ushort) AreaPacketId.recv_item_scroll_sub_enchant_check_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            //sub_496540
            res.WriteInt64(0);
            res.WriteInt32(0);
            for (int i = 0; i < 0x5; i++)
            {
                res.WriteInt32(0);
            }
            //4929F0
            res.WriteInt64(0);
            //sub4929A0
            res.WriteInt16(0);
            for (int i = 0; i < 0x2; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0x2; j++)
                {
                    res.WriteInt16(0);
                }
            }
            for (int k = 0; k < 0x5; k++)
            {
                res.WriteInt16(0);
                //sub492940

                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0x2; j++)
                {
                    res.WriteInt16(0);
                }

            }
            //4929F0
            res.WriteInt64(0);
            //sub4929A0
            res.WriteInt16(0);
            for (int i = 0; i < 0x2; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0x2; j++)
                {
                    res.WriteInt16(0);
                }
            }
            for (int k = 0; k < 0x5; k++)
            {
                res.WriteInt16(0);
                //sub492940

                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0x2; j++)
                {
                    res.WriteInt16(0);
                }

            }

            //4929F0
            res.WriteInt64(0);
            //sub4929A0
            res.WriteInt16(0);
            for (int i = 0; i < 0x2; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0x2; j++)
                {
                    res.WriteInt16(0);
                }
            }
            for (int k = 0; k < 0x5; k++)
            {
                res.WriteInt16(0);
                //sub492940

                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0x2; j++)
                {
                    res.WriteInt16(0);
                }

            }
            //end   sub_496540
            return res;
        }
    }
}
