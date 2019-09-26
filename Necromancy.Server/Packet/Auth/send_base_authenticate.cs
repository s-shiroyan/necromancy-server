using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet.Auth
{
    public class send_base_authenticate : Handler
    {
        public send_base_authenticate(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AuthPacketId.send_base_authenticate;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string accountName = packet.Data.ReadCString();
            string password = packet.Data.ReadCString();
            string macAddress = packet.Data.ReadCString();
            int unknown = packet.Data.ReadInt16();
            Logger.Info($"Account:{accountName} Password:{password} Unknown:{unknown}");

            Account account = Database.SelectAccountByName(accountName);
            if (account == null)
            {
                if (Settings.NeedRegistration)
                {
                    Logger.Error(client, $"AccountName: {accountName} doesn't exist");
                    SendResponse(client, null);
                    client.Socket.Close();
                    return;
                }

                string bCryptHash = BCrypt.Net.BCrypt.HashPassword(password, NecSetting.BCryptWorkFactor);
                account = Database.CreateAccount(accountName, "", bCryptHash);
            }

            if (!BCrypt.Net.BCrypt.Verify(password, account.Hash))
            {
                Logger.Error(client, $"Invalid password for AccountName: {accountName}");
                SendResponse(client, null);
                client.Socket.Close();
                return;
            }

            // TODO replace with sessionId
            Session session = new Session(account.Id.ToString(), account);
            Server.Sessions.StoreSession(session);
            client.Session = session;

            Server.Clients.Add(client);
            SendResponse(client, account);
        }

        private void SendResponse(NecClient client, Account account)
        {
            IBuffer res = BufferProvider.Provide();
            if (account == null)
            {
                res.WriteInt32(1); // Error (0 = OK,  1 = ID or Pw to long)
                res.WriteInt32(0); // Account Id
            }
            else
            {
                res.WriteInt32(0); // Error (0 = OK,  1 = ID or Pw to long)
                res.WriteInt32(account.Id);
            }

            Router.Send(client, (ushort) AuthPacketId.recv_base_authenticate_r, res);
        }
    }
}