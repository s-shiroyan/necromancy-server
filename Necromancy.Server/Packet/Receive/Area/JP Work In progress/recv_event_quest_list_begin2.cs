using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_event_quest_list_begin2 : PacketResponse
    {
        public recv_event_quest_list_begin2()
            : base((ushort) AreaPacketId.recv_event_quest_list_begin2, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(0);
            res.WriteInt32(numEntries); //less than 0x1E            res.WriteInt32(0);
            for (int j = 0; j < numEntries; j++)
            {
                //sub_4936D0
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteFixedString("Test", 0x61);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("SecondTest", 0x61);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                for (int i = 0; i < 0xA; i++)
                {
                    //sub 493640
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteInt16(0);
                }

                res.WriteByte(0);

                for (int i = 0; i < 0xC; i++)
                {
                    //sub 493640
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteInt16(0);
                }

                res.WriteByte(0);

                res.WriteInt32(0);
                //Ret

                res.WriteFixedString("", 0x181);
                res.WriteFixedString("", 0x181);
                for (int i = 0; i < 0x5; i++)
                {
                    res.WriteByte(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                }
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(0);
            res.WriteInt32(numEntries); //less than 0x1E            res.WriteInt32(0);
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
            }
            numEntries = 1;
            res.WriteInt32(numEntries); //Less than 0x1  
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
            }
            numEntries = 0x14; //<---
            res.WriteInt32(numEntries); //less than 0x14
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
            }
            res.WriteInt32(numEntries); //less than 0x14
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
            }
            return res;
        }
    }
}
