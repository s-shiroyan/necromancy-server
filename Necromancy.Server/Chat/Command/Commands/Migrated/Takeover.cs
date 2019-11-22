using System.Collections.Generic;
using Necromancy.Server.Model;
using Necromancy.Server.Common.Instance;
using System;
using Necromancy.Server.Packet.Response;




namespace Necromancy.Server.Chat.Command.Commands
{
    //failsafe to end events when frozen
    public class Takeover : ServerChatCommand
    {
        public Takeover(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            
            if(client.Character.takeover == true)
            { client.Character.takeover = false; }
            else if (client.Character.takeover == false)
            { client.Character.takeover = true; }



            if (command[0] =="cancel" || command[0] == "c" )
            { client.Character.takeover = false; }






            IInstance instance = Server.Instances.GetInstance(client.Character.eventSelectReadyCode);

            if (command[0] == "save" || command[0] == "s")
            {
                client.Character.takeover = false;

                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        Logger.Debug($"NPCId: {npcSpawn.Id} is being updated in the database");
                        npcSpawn.Heading = (byte)(client.Character.Heading);
                        npcSpawn.X = (client.Character.X);
                        npcSpawn.Y = (client.Character.Y);
                        npcSpawn.Z = (client.Character.Z);



                        npcSpawn.Updated = DateTime.Now;
                        if (!Server.Database.UpdateNpcSpawn(npcSpawn))
                        {
                            Logger.Error("Could not update the database");
                            return;
                        }
                        break;
                    case MonsterSpawn monsterSpawn:
                        Logger.Debug($"MonsterId: {monsterSpawn.Id} is being updated in the database");

                        break;
                    case Character character:
                        Logger.Debug($"CharacterId: {character.Id} is being updated in the database");

                        break;
                    default:
                        Logger.Error($"Instance with InstanceId: {instance.InstanceId} does not exist");
                        break;
                }


            }
            else
            {
                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        Logger.Debug($"NPCId: {npcSpawn.Id} is now under your movement control. Click NPC to Cancel");

                        break;
                    case MonsterSpawn monsterSpawn:
                        Logger.Debug($"MonsterId: {monsterSpawn.Id} is now under your movement control. Click NPC to Cancel");

                        break;
                    case Character character:
                        Logger.Debug($"CharacterId: {character.Id} is now under your movement control. Click NPC to Cancel");

                        break;
                    default:
                        Logger.Error($"Instance with InstanceId: {instance.InstanceId} does not exist");
                        break;
                }
            }


        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "takeover";
        public override string HelpText => "usage: `/takeover ` - Takes over the last object targeted \n `/takeover cancel` - cancels the takeover \n `/takeover save` - saves the takeover to database ";

    }
}
