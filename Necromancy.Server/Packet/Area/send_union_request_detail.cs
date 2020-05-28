using System;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_detail : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_union_request_detail));

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

                    client.Character.unionId = myUnion.Id;
                    client.Union = myUnion;
                    client.Union.Join(client);

                    TimeSpan differenceCreated = client.Union.Created.ToUniversalTime() - DateTime.UnixEpoch;
                    int unionCreatedCalculation = (int) Math.Floor(differenceCreated.TotalSeconds);

                    //Notify client of each union member in above union, queried by charaname and InstanceId (for menu based interactions)
                    foreach (UnionMember unionMemberList in Server.Database.SelectUnionMembersByUnionId(client.Character
                        .unionId))
                    {
                        NecClient otherClient = Server.Clients.GetByCharacterId(unionMemberList.CharacterDatabaseId);
                        if (otherClient == null)
                        {
                            continue;
                        }

                        Character character = otherClient.Character;
                        if (character == null)
                        {
                            continue;
                        }

                        Soul soul = otherClient.Soul;
                        if (soul == null)
                        {
                            continue;
                        }

                        TimeSpan differenceJoined = unionMemberList.Joined.ToUniversalTime() - DateTime.UnixEpoch;
                        int unionJoinedCalculation = (int) Math.Floor(differenceJoined.TotalSeconds);
                        IBuffer res3 = BufferProvider.Provide();
                        res3.WriteInt32(client.Character.unionId); //Union Id
                        res3.WriteUInt32(character.InstanceId);
                        res3.WriteFixedString($"{soul.Name}", 0x31); //size is 0x31
                        res3.WriteFixedString($"{character.Name}", 0x5B); //size is 0x5B
                        res3.WriteUInt32(character.ClassId);
                        res3.WriteByte(character.Level);
                        res3.WriteInt32(character.MapId); // Location of your Union Member
                        res3.WriteInt32(unionJoinedCalculation); //Area of Map, somehow. or Channel;
                        res3.WriteFixedString($"Channel {character.Channel}", 0x61); // Channel location
                        res3.WriteUInt32(unionMemberList
                            .MemberPriviledgeBitMask); //permissions bitmask  obxxxx1 = invite | obxxx1x = kick | obxx1xx = News | 0bxx1xxxxx = General Storage | 0bx1xxxxxx = Deluxe Storage
                        res3.WriteUInt32(unionMemberList
                            .Rank); //Rank  3 = beginner 2 = member, 1 = sub-leader 0 = leader
                        res3.WriteInt32(0); //online status. 0 = online, 1 = away, 2 = offline
                        res3.WriteInt32(unionJoinedCalculation); //Date Joined in seconds since unix time
                        res3.WriteInt32(Util.GetRandomNumber(0, 3));
                        res3.WriteInt32(Util.GetRandomNumber(0, 3));
                        Router.Send(client, (ushort) MsgPacketId.recv_union_notify_detail_member, res3, ServerType.Msg);
                    }
                    
                    Server.Clients.GetCharacterInstanceIdByCharacterId(myUnion.UnionLeaderId,
                        out uint UnionLeaderInstanceId);

                    Server.Clients.GetCharacterInstanceIdByCharacterId(myUnion.UnionSubLeader1Id,
                        out uint UnionSubLeader1InstanceId);

                    Server.Clients.GetCharacterInstanceIdByCharacterId(myUnion.UnionSubLeader2Id,
                        out uint UnionSubLeader2InstanceId);

                    //Notify client if msg server found Union settings in database(memory) for client character Unique Persistant ID.
                    IBuffer res = BufferProvider.Provide();
                    res.WriteInt32(unionMember.UnionId); //Union Instance ID //form the ToDo Logic above
                    res.WriteFixedString(myUnion.Name, 0x31); //size is 0x31
                    res.WriteInt32(unionCreatedCalculation); //Creation Date in seconds since unix 0 time (Jan. 1, 1970)
                    res.WriteUInt32(UnionLeaderInstanceId); //Leader
                    res.WriteInt32(0);
                    res.WriteUInt32(UnionSubLeader1InstanceId); //subleader1
                    res.WriteInt32(0);
                    res.WriteUInt32(UnionSubLeader2InstanceId); //subleader2
                    res.WriteInt32(0);
                    res.WriteByte((byte) myUnion.Level); //Union Level
                    res.WriteUInt32(myUnion.CurrentExp); //Union EXP Current
                    res.WriteUInt32(myUnion.NextLevelExp); //Union EXP next level Target
                    res.WriteByte(myUnion
                        .MemberLimitIncrease); //Increase Union Member Limit above default 50 (See Union Bonuses
                    res.WriteByte(myUnion.MemberLimitIncrease);
                    res.WriteInt32(999999999); //Creation Date?
                    res.WriteInt16(myUnion.CapeDesignID); //Mantle/Cape
                    res.WriteFixedString($"You are all members of {myUnion.Name} now.  Welcome!",
                        0x196); //size is 0x196
                    for (int i = 0; i < 8; i++)
                        res.WriteInt32(-1);
                    res.WriteByte(255);
                    Router.Send(client, (ushort) MsgPacketId.recv_union_notify_detail, res, ServerType.Msg);
                }
            }


            //Acknowledge send.  'Hey send,  i'm finished doing my stuff.  go do the next stuff'
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.unionId); //probably error check. 0 for success           
            Router.Send(client, (ushort) MsgPacketId.recv_union_request_detail_r, res2, ServerType.Msg);
        }
    }
}
