using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_news : ClientHandler
    {
        public send_union_request_news(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_news;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int newsEntries = 25; /*myUnion.NewsEntries.Count();*/ //max of 0x3E8
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0 /*myUnion.UnionNewsList.InstanceID*/); // News list instance ID?
            res.WriteInt32(newsEntries); //less than or equal to 0x3E8
            for (int i = 0; i < newsEntries; i++) //limit is the int32 above
            {
                res.WriteInt32(10000 + i); //News entry ID
                res.WriteFixedString($"{client.Soul.Name}", 0x31); //soul name  
                res.WriteFixedString($"{client.Character.Name}", 0x5B); //character name
                res.WriteInt32(i); // Activity lookup from Str_table.csv A=100 B=745 c=0 to 17. 
                res.WriteFixedString($"{i} bottles of beer on the wall", 0x49); //Parameter 3 for str table
                res.WriteFixedString($"Pass it around. {i - 1} bottles of Beer on the wall", 0x49); //unknown
                res.WriteInt32(i - 1); //count of items or gold being actioned. parameter 4 for str table
            }

            Router.Send(client, (ushort) MsgPacketId.recv_union_request_news_r, res, ServerType.Msg);
        }
    }
}
