using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : ClientHandler
    {
        public send_map_get_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.MapId);


            Router.Send(client, (ushort)AreaPacketId.recv_map_get_info_r, res, ServerType.Area);
         
            SendDataNotifyNpcData(client);
        }
       


        private void SendDataNotifyNpcData(NecClient client)
        {
            int parsedStringToNumber;
            int numEntries = 0; // 1 to 19 equipment.  Setting to 0 because NPCS don't wear gear.
            int numEntries2 = 0; //number of status effects.  128 Max.   setting to 0 for noise reduction in console
            int npcCount = 0;

            string[][] jaggedArray = FileReader.GameFileReader(client); //Reads a manually created CSV file to load NPCs per Client.Character.MapID based on settings from map_symbol.csv and npc.csv
            foreach (string[] ary1 in jaggedArray)
            {
                npcCount++;
                Logger.Debug($"Loading NPC Number: {npcCount} from TempNPCData.csv with values {ary1[1]}, {ary1[2]}, {ary1[3]}, {ary1[4]}, {ary1[5]}, {ary1[6]}, {ary1[7]},");
                IBuffer res2 = BufferProvider.Provide();
                
                //Not All SerialIDs in CSV are numeric.  catching the strings and seting a random ID.
                bool success = Int32.TryParse(ary1[1], out parsedStringToNumber);
                if (success)
                {
                    res2.WriteInt32(parsedStringToNumber);             // NPC ID (object id)
                    res2.WriteInt32(parsedStringToNumber);      // NPC Serial ID from "npc.csv"
                }
                else
                {
                    int randomSerialID = Util.GetRandomNumber(90000010, 90000099);
                    res2.WriteInt32(randomSerialID);             // NPC ID (object id)
                    res2.WriteInt32(randomSerialID);      // NPC Serial ID from "npc.csv"
                }
                             

                res2.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

                res2.WriteCString(ary1[6]);//Name

                res2.WriteCString(ary1[7]);//Title

                res2.WriteFloat(Int32.Parse(ary1[3]));//X Pos
                res2.WriteFloat(Int32.Parse(ary1[4]));//Y Pos
                res2.WriteFloat(Int32.Parse(ary1[5]));//Z Pos
                res2.WriteByte((byte)Util.GetRandomNumber(1,180));//view offset

                res2.WriteInt32(numEntries); // # Items to Equip

                for (int i = 0; i < numEntries; i++)

                {
                    res2.WriteInt32(24);
                }

                res2.WriteInt32(numEntries); // # Items to Equip

                for (int i = 0; i < numEntries; i++)

                {
                    // loop start
                    res2.WriteInt32(210901); // this is a loop within a loop i went ahead and broke it up
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(3);

                    res2.WriteInt32(10310503);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(3);

                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(1); // bool
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                }

                res2.WriteInt32(numEntries); // # Items to Equip

                for (int i = 0; i < numEntries; i++) // Item type bitmask per slot

                {
                    res2.WriteInt32(1);

                }

                res2.WriteInt32(Int32.Parse(ary1[8]));   //NPC Model from file "model_common.csv"
                


                res2.WriteInt16(100);       //NPC Model Size

                res2.WriteByte(2);

                res2.WriteByte(5);

                res2.WriteByte(6);

                res2.WriteInt32(0); //Hp Related Bitmask?  This setting makes the NPC "alive"    11111110 = npc flickering, 0 = npc alive

                res2.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull

                res2.WriteInt32(Int32.Parse(ary1[2]));
                res2.WriteFloat(0); //x for icons
                res2.WriteFloat(0); //y for icons
                res2.WriteFloat(50); //z for icons

                res2.WriteInt32(numEntries2); // number of status effects.  128 Max.   setting to 0 for noise reduction in console
                                
                for (int i = 0; i < numEntries2; i++)

                {
                    res2.WriteInt32(0);
                    res2.WriteInt32(0);
                    res2.WriteInt32(0);

                }

                Router.Send(client, (ushort)AreaPacketId.recv_data_notify_npc_data, res2, ServerType.Area);
            }
        }
    }
}
