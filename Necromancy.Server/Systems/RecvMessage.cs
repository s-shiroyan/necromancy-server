using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems
{
    abstract class RecvMessage : Message
    {
        public void Send() { 
            //Router.Send(client, (ushort) AreaPacketId.recv_auction_notify_open, res, ServerType.Area);
        }
    }
}
