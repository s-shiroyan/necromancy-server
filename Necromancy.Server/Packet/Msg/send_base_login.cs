using System;
using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_base_login : Handler
    {
        public send_base_login(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_base_login;

        public const int SoulCount = 2;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int accountId = packet.Data.ReadInt32();
            byte[] unknown = packet.Data.ReadBytes(20); // Suspect SessionId
            // TODO replace with sessionId
            Session session = Server.Sessions.GetSession(accountId.ToString());
            if (session == null)
            {
                Logger.Error(client, $"AccountId: {accountId} has no active session");
                SendResponse(client);
                client.Socket.Close();
                return;
            }

            client.Session = session;
            SendResponse(client);
        }


        private void SendResponse(NecClient client)
        {
            List<Soul> souls = Database.SelectSoulsByAccountId(client.Account.Id);
            if (souls.Count <= 0)
            {
                IBuffer resq = BufferProvider.Provide();

                resq.WriteInt32(0); //  Error

                resq.WriteByte(0);
                resq.WriteFixedString(String.Empty, 49); // Soul Name
                resq.WriteByte(0); // Soul Level
                resq.WriteByte(0); // bool - if use value 1, can't join in msg server character list

                resq.WriteByte(0);
                resq.WriteFixedString(String.Empty, 49); // Soul Name
                resq.WriteByte(0); // Soul Level
                resq.WriteByte(0); // bool - if use value 1, can't join in msg server character list


                resq.WriteByte(0); // bool
                resq.WriteByte(0);
                Router.Send(client, (ushort) MsgPacketId.recv_base_login_r, resq);
                return;
            }


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //  Error
            for (int i = 0; i < SoulCount; i++)
            {
                if (souls.Count > i)
                {
                    Soul soul = souls[i];
                    res.WriteByte(1);
                    res.WriteFixedString(soul.Name, 49);
                    res.WriteByte(soul.Level);
                    res.WriteByte(0); // bool - if use value 1, can't join in msg server character list
                }
                else
                {
                    res.WriteByte(0);
                    res.WriteFixedString(String.Empty, 49); // Soul Name
                    res.WriteByte(0); // Soul Level
                    res.WriteByte(0); // bool - if use value 1, can't join in msg server character list
                }
            }

            res.WriteByte(0); // bool
            res.WriteByte(0);

            Router.Send(client, (ushort) MsgPacketId.recv_base_login_r, res);
        }
    }
}