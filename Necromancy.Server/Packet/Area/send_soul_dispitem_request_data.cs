using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_soul_dispitem_request_data : ClientHandler
    {
        public send_soul_dispitem_request_data(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_soul_dispitem_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_request_data_r, res, ServerType.Area);

            //notify you of the soul item you got based on something above.
            IBuffer res19 = BufferProvider.Provide();
            res19.WriteInt32(Util.GetRandomNumber(62000001, 62000015)); //soul_dispitem.csv
            Router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_notify_data, res19, ServerType.Area);

            //ToDo  find a better home for these functionalities . This send is the last stop before initial map entry.
            LoadInventory(client);
            //LoadCloakRoom(client);
            //LoadBattleStats(client);
            LoadHonor(client);
        }

        public void LoadHonor(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); // must be under 0x3E8 // DBLoad your honor titles, and dump them here. 1000 at a time
            for (int i = 0; i < 1; i++)
            {
                res.WriteInt32(10010101);/*novice monster hunter*/
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteByte(1); // bool	New Title 0:Yes  1:No	
            }
            Router.Send(client, (ushort)AreaPacketId.recv_get_honor_notify, res, ServerType.Area);
        }
        public void LoadBattleStats(NecClient client)
        {
            //populate your stats.
            short PhysAttack = 0;
            short MagAttack = 0;
            short PhysDef = 0;
            short MagDef = 0;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt16((short)client.Character.Strength); //base Phys Attack
            res.WriteInt16((short)client.Character.Intelligence); //base Mag attack
            res.WriteInt16((short)client.Character.Dexterity); //base Phys Def
            res.WriteInt16((short)client.Character.Piety); //base Mag Def

            res.WriteInt16(PhysAttack); //Equip Bonus Phys attack
            res.WriteInt16(MagAttack); //Equip Bonus Mag Attack
            res.WriteInt16(PhysDef); //Equip bonus Phys Def
            res.WriteInt16(MagDef); //Equip bonus Mag Def
            //Router.Send(client, (ushort)AreaPacketId.recv_chara_update_battle_base_param, res, ServerType.Area);
        }

        public void LoadInventory(NecClient client)
        {
            ItemService itemService = new ItemService(client.Character);
            List<ItemInstance> ownedItems = itemService.LoadOwnedInventoryItems();
            foreach (ItemInstance item in ownedItems)
            {
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, item);
                Router.Send(recvItemInstance);
            }
        }
        public void LoadCloakRoom(NecClient client)
        {
            //populate soul inventory from database.

        }
    }
}
