using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public class PacketRouter
    {
        private readonly NecLogger _logger;

        public PacketRouter()
        {
            _logger = LogProvider.Logger<NecLogger>(this);
        }

        /// <summary>
        /// Send a packet to a client.
        /// </summary>
        public void Send(NecClient client, NecPacket packet, ServerType serverType)
        {
            client.Send(packet, serverType);
        }

        /// <summary>
        /// Send a packet to a connection.
        /// </summary>
        public void Send(NecConnection connection, NecPacket packet)
        {
            connection.Send(packet);
        }

        /// <summary>
        /// Send a packet to a client.
        /// </summary>
        public void Send(NecClient client, ushort id, IBuffer data, ServerType serverType)
        {
            NecPacket packet = new NecPacket(id, data);
            Send(client, packet, serverType);
        }

        /// <summary>
        /// Send a packet to a connection.
        /// </summary>
        public void Send(NecConnection connection, ushort id, IBuffer data)
        {
            NecPacket packet = new NecPacket(id, data);
            Send(connection, packet);
        }

        /// <summary>
        /// Send a packet to multiple clients.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(List<NecClient> clients, NecPacket packet, ServerType serverType, params NecClient[] excepts)
        {
            clients = GetClients(clients, excepts);
            foreach (NecClient client in clients)
            {
                Send(client, packet, serverType);
            }
        }

        /// <summary>
        /// Send a packet to multiple clients.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(List<NecClient> clients, ushort id, IBuffer data, ServerType serverType,
            params NecClient[] excepts)
        {
            Send(clients, new NecPacket(id, data), serverType, excepts);
        }


        /// <summary>
        /// Send a packet to everyone in the map.
        /// </summary>
        public void Send(Map map, ushort id, IBuffer data, ServerType serverType, params NecClient[] excepts)
        {
            Send(map, new NecPacket(id, data), serverType, excepts);
        }

        /// <summary>
        /// Send a packet to everyone in the map.
        /// </summary>
        public void Send(Map map, NecPacket packet, ServerType serverType, params NecClient[] excepts)
        {
            List<NecClient> clients = GetClients(map.ClientLookup.GetAll(), excepts);
            foreach (NecClient client in clients)
            {
                Send(client, packet, serverType);
            }
        }

        private List<NecClient> GetClients(List<NecClient> clients, params NecClient[] excepts)
        {
            if (excepts.Length == 0)
            {
                return clients;
            }

            foreach (NecClient except in excepts)
            {
                clients.Remove(except);
            }

            return clients;
        }
    }
}