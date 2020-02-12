using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_detail : ClientHandler
    {
        public send_union_request_detail(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_union_request_detail;

        public override void Handle(NecClient client, NecPacket packet)
        {
            UnionMember unionMember = Server.Database.SelectUnionMemberByCharacterId(client.Character.Id);
            if (unionMember == null) 
            {
                Logger.Debug($"you don't appear to be in a union");
            }
            else
            {
                Logger.Debug($"union member ID{unionMember.Id} found. loading Union information");
                Union myUnion = Server.Database.SelectUnionById(unionMember.UnionId);

                if (myUnion == null)
                {
                    Logger.Error($"This is Strange.. Can't find a Union with id {unionMember.UnionId}");
                }
                else
                {
                    Logger.Debug($"union  ID{unionMember.UnionId} found. continuing loading of Union information");
                    //toDo  make a list of Unions on the Server object, so you can lookup union instanceId by Union Database id
                    //Union myUnion = Server.Instances.GetInstance(unionMember.UnionId) as Union;

                    client.Character.unionId = myUnion.Id;
                    client.Union = myUnion;
                    client.Union.Join(client);

                    //for each unionMember in myUnion.UnionMembers {  } //ToDo.  add member rank to union member table
                    //Notify client of each union member in above union, queried by charaname and InstanceId (for menu based interactions)
                    //foreach (Character character in Server.Characters.GetAll())
                    //List<Character> UnionMembers = Server.Database.SelectUnionMembersByUnionId();
                    //Character character = client.Character;
                    foreach (UnionMember unionMemberList in Server.Database.SelectUnionMembersByUnionId(client.Character.unionId))
                    {
                        Character character = Server.Characters.GetByCharacterId(unionMemberList.CharacterDatabaseId);
                        Soul soul = Server.Database.SelectSoulById(character.SoulId);
                        if (character.Id == myUnion.UnionLeaderId) { unionMemberList.Rank = 0;} //temporary pending rank addition to nec_union_member table
                        IBuffer res3 = BufferProvider.Provide();
                        res3.WriteInt32(character.InstanceId); //not sure what this is.  union_Notify ID?
                        res3.WriteInt32(character.InstanceId);
                        res3.WriteFixedString($"{soul.Name}", 0x31); //size is 0x31
                        res3.WriteFixedString($"{character.Name}", 0x5B); //size is 0x5B
                        res3.WriteInt32(character.ClassId);
                        res3.WriteByte(character.Level);
                        res3.WriteInt32(character.MapId); // Location of your Union Member
                        res3.WriteInt32(0); //Area of Map, somehow. or Channel;
                        res3.WriteFixedString($"Channel {character.Channel}", 0x61); // Channel location
                        res3.WriteInt32(Util.GetRandomNumber(0b01100111, 0b01100111)); //permissions bitmask  obxxxx1 = invite | obxxx1x = kick | obxx1xx = News | 0bxx1xxxxx = General Storage | 0bx1xxxxxx = Deluxe Storage
                        res3.WriteInt32(unionMemberList.Rank); //Rank  3 = beginner 2 = member, 1 = sub-leader 0 = leader
                        res3.WriteInt32(Util.GetRandomNumber(0, 0));
                        res3.WriteInt32(Util.GetRandomNumber(0, 0));
                        res3.WriteInt32(Util.GetRandomNumber(0, 0));
                        res3.WriteInt32(Util.GetRandomNumber(0, 0));

                        Router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail_member, res3, ServerType.Msg);
                    }

                    uint UnionLeaderInstanceId = Server.Characters.GetByCharacterId(myUnion.UnionLeaderId).InstanceId; Logger.Error($"leader ID{UnionLeaderInstanceId}");

                    //Notify client if msg server found Union settings in database(memory) for client character Unique Persistant ID.
                    IBuffer res = BufferProvider.Provide();
                    res.WriteInt32(unionMember.UnionId); //Union Instance ID //form the ToDo Logic above
                    res.WriteFixedString(myUnion.Name, 0x31); //size is 0x31
                    res.WriteInt32(UnionLeaderInstanceId); //leader
                    res.WriteInt32(myUnion.UnionSubLeader1Id); //subleader.  We need to assign character Instance IDs at server start instead of login...
                    res.WriteInt32(myUnion.UnionSubLeader2Id); //subleader2
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteByte((byte)myUnion.Level); //Union Level
                    res.WriteInt32(myUnion.CurrentExp); //Union EXP Current
                    res.WriteInt32(myUnion.NextLevelExp); //Union EXP next level Target
                    res.WriteByte(myUnion.MemberLimitIncrease); //Increase Union Member Limit above default 50 (See Union Bonuses
                    res.WriteByte(myUnion.MemberLimitIncrease);
                    res.WriteInt32(client.Character.InstanceId);
                    res.WriteInt16(myUnion.CapeDesignID); //Mantle/Cape?
                    res.WriteFixedString($"You are all members of {myUnion.Name} now.  Welcome!", 0x196); //size is 0x196
                    for (int i = 0; i < 8; i++)
                        res.WriteInt32(i);
                    res.WriteByte(15);

                    Router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail, res, ServerType.Msg);


                }
            }


            



            //Acknowledge send.  'Hey send,  i'm finished doing my stuff.  go do the next stuff'
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.unionId); //probably error check. 0 for success           
            Router.Send(client, (ushort)MsgPacketId.recv_union_request_detail_r, res2, ServerType.Msg);


        }
    }
}
