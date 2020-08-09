using Necromancy.Server.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems
{
    abstract class Message 
    {
        public abstract ushort Id { get; }
        public abstract int ExpectedSize { get; }
    }
}
