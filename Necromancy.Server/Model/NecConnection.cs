using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Model
{
    public class NecConnection
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(NecConnection));

        public NecConnection(ITcpSocket clientSocket, PacketFactory packetFactory, ServerType serverType)
        {
            Socket = clientSocket;
            PacketFactory = packetFactory;
            ServerType = serverType;
            Client = null;
        }

        public string Identity => Socket.Identity;
        public ServerType ServerType { get; }
        public ITcpSocket Socket { get; }
        public PacketFactory PacketFactory { get; }
        public NecClient Client { get; set; }

        public List<NecPacket> Receive(byte[] data)
        {
            List<NecPacket> packets;
            try
            {
                packets = PacketFactory.Read(data, ServerType);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                packets = new List<NecPacket>();
            }

            return packets;
        }

        public void Send(NecPacket packet)
        {
            byte[] data;
            try
            {
                data = PacketFactory.Write(packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                return;
            }

            Logger.LogOutgoingPacket(this, packet, ServerType);
            Socket.Send(data);
        }
    }
}
