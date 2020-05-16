using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //Adds an Identified item to inventory
    public class SendItemInstance : ServerChatCommand
    {
        private readonly NecServer _server;

        public SendItemInstance(NecServer server) : base(server)
        {
            _server = server;
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            Item item = _server.Instances64.CreateInstance<Item>();
            //recv_item_instance = 0x86EA,
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt64(item.InstanceId); //InstanceID
            res.WriteInt32(100101); //Icon serial id
            res.WriteByte(1); //Number of "items"
            res.WriteInt32(9); //Item status, in multiples of numbers, 1/3 = unidentified, 4/6 = broken, 5/7 = broken unidentified, 8 = cursed, loops after a while, only odd numbers are correct i think
            res.WriteFixedString($"DAGGER {item.InstanceId}", 0x10);
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2 // maybe.. more bag index?
            res.WriteInt16(1); // bag index
            res.WriteInt32(0); //Equip bitmap
            res.WriteInt32(10200101); //Percentage stat, 9 max i think
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteCString(""); // Soul name of who it is bound  to
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt32(0); //Divides max % by this number
            res.WriteByte(0);
            res.WriteInt32(0);
            int numEntries = 2;
            res.WriteInt32(numEntries); // less than or equal to 2

            //for (int i = 0; i < numEntries; i++)
            res.WriteInt32(0);
            res.WriteInt32(0);

            numEntries = 3;
            res.WriteInt32(numEntries); // less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(1); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0); //Guard protection toggle, 1 = on, everything else is off
            res.WriteInt16(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_instance, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "itis";
    }
}
