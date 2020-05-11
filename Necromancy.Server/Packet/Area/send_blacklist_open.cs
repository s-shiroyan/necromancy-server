using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_blacklist_open : ClientHandler
    {
        public send_blacklist_open(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_blacklist_open;

        public override void Handle(NecClient client, NecPacket packet)
        {
            //Testing Logic.  Delete after blacklist is databased
            int TempCharacterCount = Server.Characters.GetAll().Count;
            if (TempCharacterCount > 10)
            {
                TempCharacterCount = 10;
            }

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //must be 0

            res.WriteInt32(TempCharacterCount); //count of entries to display

            //for (int i = 0; i < 10; i++)
            foreach (Character blackCharacter in Server.Characters.GetAll()
            ) //Max 10 Loops. will break if more than 10 characters in Db.
            {
                Soul blackSoul = Server.Database.SelectSoulById(blackCharacter.SoulId);

                res.WriteUInt32(blackCharacter
                    .InstanceId); //Unique Row ID for blacklist.  using Character InstanceID, but can be anything
                res.WriteInt32(client.Character.AdventureBagGold); //???
                res.WriteInt32(Util.GetRandomNumber(1, 10)); //Count of times BlackListMember PKed you
                res.WriteInt32(Util.GetRandomNumber(0, 3)); //count of times BlackListMember looted you
                res.WriteInt32(Util.GetRandomNumber(0, 150)); //Count of items BlackListMember looted from you
                res.WriteInt32(client.Character.unionId); //Union ID?  we dont have any unions yet
                res.WriteByte(255);
                res.WriteByte(1); // bool

                res.WriteInt32(blackSoul.Id); //Soul ID of BlackListMember for sending bounty to Area server
                res.WriteFixedString(blackSoul.Name, 49); //Soul Name of BlackListMember

                res.WriteUInt32(blackCharacter.InstanceId); //for online location tracking?
                res.WriteFixedString(blackCharacter.Name, 91); //character name of BlackListMember
                res.WriteUInt32(blackCharacter.ClassId); // Character Class of BlackListMember
                res.WriteByte(blackCharacter.Level); //Character level of BlackListMember
                res.WriteInt32(blackCharacter.MapId); //Character Map ID of BlackListMember
                res.WriteInt32(client.Character.AdventureBagGold); //world number?? or Map Area?
                res.WriteFixedString($"Channel {blackCharacter.Channel}", 97);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_blacklist_open_r, res, ServerType.Area);
        }
    }
}
