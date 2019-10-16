using System.Diagnostics;
using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Model
{
    [DebuggerDisplay("{Identity,nq}")]
    public class NecClient
    {
        private readonly NecLogger _logger;

        public NecClient()
        {
            _logger = LogProvider.Logger<NecLogger>(this);
        }

        #region Session

        public Session Session { get; set; }

        public Account Account
        {
            get => Session.Account;
            set => Session.Account = value;
        }

        public Soul Soul
        {
            get => Session.Soul;
            set => Session.Soul = value;
        }

        public Character Character
        {
            get => Session.Character;
            set => Session.Character = value;
        }

        public Channel Channel
        {
            get => Session.Channel;
            set => Session.Channel = value;
        }


        public Map Map
        {
            get => Session.Map;
            set => Session.Map = value;
        }

        #endregion

        public string Identity { get; private set; }

        public NecConnection AuthConnection { get; set; }
        public NecConnection MsgConnection { get; set; }
        public NecConnection AreaConnection { get; set; }

        public void Send(NecPacket packet, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.Area:
                    AreaConnection.Send(packet);
                    break;
                case ServerType.Msg:
                    MsgConnection.Send(packet);
                    break;
                case ServerType.Auth:
                    AuthConnection.Send(packet);
                    break;
                default:
                    _logger.Error(this, "Invalid ServerType");
                    break;
            }
        }
    }
}