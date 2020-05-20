using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Data.Setting;

namespace Necromancy.Server.Chat.Command.Commands
{
    //adds an unidentified item to inventory
    public class SendItemInstanceUnidentified : ServerChatCommand
    {
        public SendItemInstanceUnidentified(NecServer server) : base(server)
        {
        }

        void AddItemSetting(ItemSetting setting)
        {
            int Id = setting.Id;
            string Name = setting.Name;
            int ItemType = setting.ItemType;
            int IconType = setting.IconType;
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {

            Item item = Server.Instances64.CreateInstance<Item>();
            item.name = "Dagger";
            item.type = 2;
            item.bitmask = 1;
            item.count = 1;
            item.icon = 10202102;

            //recv_item_instance_unidentified = 0xD57A,

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt64(item.InstanceId); //Item Object ID 

            res.WriteCString(item.name.ToString() + $" {item.InstanceId}"); //Name

            res.WriteInt32(item.type); //Item type

            res.WriteInt32(1); //Bit mask designation? (Only lets you apply this to certain slots dependant on what you send here) 1 for right hand, 2 for left hand

            res.WriteByte(item.count); //Number of items

            res.WriteInt32(0); //Item status 0 = identified  (same as item status inside senditeminstance)

            res.WriteInt32(item.icon); //Item icon
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(item.icon);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0); // bool
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2
            res.WriteInt16(10); // bag index

            res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

            res.WriteInt64(0);

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);

            IBuffer res30 = BufferProvider.Provide();
            res30.WriteInt64(10200101);
            res30.WriteInt32(100); // MaxDura points
            //Router.Send(client, (ushort) AreaPacketId.recv_item_update_maxdur, res30, ServerType.Area);

            //recv_item_update_durability = 0x1F5A, 
            IBuffer res31 = BufferProvider.Provide();
            res31.WriteInt64(10200101);
            res31.WriteInt32(10);
            //Router.Send(client, (ushort) AreaPacketId.recv_item_update_durability, res31, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "itus";
    }
}
