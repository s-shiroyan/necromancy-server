using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : Handler
    {
        public send_data_get_self_chara_data_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            SendDataGetSelfCharaData(client);
            //SendDataNotifyCharaData(client);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_request_r, res2);

        }

        private void SendDataGetSelfCharaData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            //sub_4953B0 - characteristics
            res.WriteInt32(4); //race
            res.WriteInt32(1); //gender
            res.WriteByte(1); //hair
            res.WriteByte(2); //face
            res.WriteByte(3); //color

            //sub_484720 - combat/leveling info
            res.WriteInt32(2);  // ? character ID maybe?
            res.WriteInt32(3); // class
            res.WriteInt16(1); // current level
            res.WriteInt64(0); // current exp
            res.WriteInt64(12); // soul exp
            res.WriteInt64(33); // exp needed to level
            res.WriteInt64(44); // soul exp needed to level
            res.WriteInt32(123); // current hp
            res.WriteInt32(100); // current mp
            res.WriteInt32(111); // current od
            res.WriteInt32(123); // max hp
            res.WriteInt32(124); // maxmp
            res.WriteInt32(189); // max od
            res.WriteInt32(164); // current gp
            res.WriteInt32(155); // map gp
            res.WriteInt32(1238); // value/100 = current weight
            res.WriteInt32(1895); // value/100 = max weight
            res.WriteByte(200); // condition

            // total stat level includes bonus'?
            res.WriteInt16(24); // str
            res.WriteInt16(28); // vit
            res.WriteInt16(35); // dex
            res.WriteInt16(89); // agi
            res.WriteInt16(42); // int
            res.WriteInt16(52); // pie
            res.WriteInt16(90); // luk

            // mag atk atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(145); // changed nothing visably
            res.WriteInt16(85); // changed nothing visably
            res.WriteInt16(96); // changed nothing visably

            // mag def atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(145); // changed nothing visably
            res.WriteInt16(85); // changed nothing visably
            res.WriteInt16(96); // changed nothing visably

            //status change resistance
            res.WriteInt16(215); // fire
            res.WriteInt16(99); // water
            res.WriteInt16(88); // wind
            res.WriteInt16(455); // earth
            res.WriteInt16(333); // light
            res.WriteInt16(1222); // dark
            res.WriteInt16(1445); // changed nothing visably
            res.WriteInt16(858); // changed nothing visably
            res.WriteInt16(962); // changed nothing visably
            res.WriteInt16(968); // changed nothing visably
            res.WriteInt16(9688); // changed nothing visably

            // gold and alignment?
            res.WriteInt64(214587); // gold
            res.WriteInt32(187); // changed nothing visably
            res.WriteInt32(5); // lawful
            res.WriteInt32(3); // neutral
            res.WriteInt32(1); // chaos
            res.WriteInt32(113); // changed nothing visably

            //sub_484980
            res.WriteInt32(1);// changed nothing visably
            res.WriteInt32(1);// changed nothing visably
            res.WriteInt32(1);// changed nothing visably

            // characters stats
            res.WriteInt16(24); // str
            res.WriteInt16(28); // vit
            res.WriteInt16(35); // dex
            res.WriteInt16(89); // agi
            res.WriteInt16(42); // int
            res.WriteInt16(52); // pie
            res.WriteInt16(90); // luk

            // nothing
            res.WriteInt16(51); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(10); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(87); // changed nothing visably
            res.WriteInt16(122); // changed nothing visably
            res.WriteInt16(14); // changed nothing visably
            res.WriteInt16(73); // changed nothing visably
            res.WriteInt16(69); // changed nothing visably


            // nothing
            res.WriteInt16(51); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(10); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(87); // changed nothing visably
            res.WriteInt16(122); // changed nothing visably
            res.WriteInt16(14); // changed nothing visably
            res.WriteInt16(73); // changed nothing visably
            res.WriteInt16(69); // changed nothing visably

            // nothing
            res.WriteInt16(51); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(10); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(87); // changed nothing visably
            res.WriteInt16(122); // changed nothing visably
            res.WriteInt16(14); // changed nothing visably
            res.WriteInt16(73); // changed nothing visably
            res.WriteInt16(69); // changed nothing visably
            res.WriteInt16(73); // changed nothing visably
            res.WriteInt16(69); // changed nothing visably

            //sub_484B00 map ip and connection
            res.WriteInt32(1001002);//MapSerialID
            res.WriteInt32(1001002);//MapID
            res.WriteFixedString("127.0.0.1", 65);//IP
            res.WriteInt16(60002);//Port

            //sub_484420
            res.WriteFloat(23162);//X Pos
            res.WriteFloat(-219);//Y Pos
            res.WriteFloat(3);//Z Pos
            res.WriteByte(1);//view offset

            //sub_read_int32 skill point
            res.WriteInt32(0); // skill point

            //sub_483420 character state like alive/dead/invis
            res.WriteInt32(0);

            //sub_494AC0
            res.WriteByte(78); // soul level
            res.WriteInt32(22); // current soul points
            res.WriteInt32(29); // changed nothing visably
            res.WriteInt32(12); // max soul points
            res.WriteByte(9); // changed nothing visably
            res.WriteByte(1); //Bool
            res.WriteByte(12); // changed nothing visably
            res.WriteByte(72); // changed nothing visably
            res.WriteByte(43); // changed nothing visably
            res.WriteByte(75); // changed nothing visably

            //sub_read_3-int16 unknown
            res.WriteInt16(55); // changed nothing visably
            res.WriteInt16(66); // changed nothing visably
            res.WriteInt16(77); // changed nothing visably

            //sub_4833D0
            res.WriteInt64(6);// changed nothing visably

            //sub_4833D0
            res.WriteInt64(7);// changed nothing visably

            //sub_4834A0
            res.WriteFixedString("", 97);//Shopname

            //sub_4834A0
            res.WriteFixedString("", 385);//Comment

            //sub_494890
            res.WriteByte(1);//Bool

            //sub_4834A0
            res.WriteFixedString("aaaa", 385);//Chatbox?

            //sub_494890
            res.WriteByte(1);//Bool

            //sub_483420
            int numEntries = 19;
            res.WriteInt32(numEntries);//has to be less than 19(defines how many int32s to read?)

            //sub_483660 type of weapon?
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);
            res.WriteInt32(2);



            //sub_483420
            numEntries = 19;
            res.WriteInt32(numEntries);//has to be less than 19(i think this defines the next subs #of loops)

            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(10310503);//item ID
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(1);
                res.WriteByte(2);
                res.WriteByte(1);//Bool
                res.WriteByte(3);
                res.WriteByte(4);
                res.WriteByte(5);
                res.WriteByte(6);
                res.WriteByte(7);
            }

            //sub_483420
            numEntries = 19;//influences a loop that needs to be under 19
            res.WriteInt32(numEntries);

            //sub_483420 has to be 3 for wep to be 1h sword
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);
            res.WriteInt32(3);

            //sub_483420
            numEntries = 128;
            res.WriteInt32(numEntries);//has to be less than 128

            //sub_485A70
            for (int i = 0; i < numEntries; i++)//breaks with iteration i implement, not sure how to do so
            {
                res.WriteInt32(0);
                res.WriteInt32(1);
                res.WriteInt32(2);
            }

            Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_r, res);
        }
    }
}