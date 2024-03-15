using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace Server.NetMessages
{
    public abstract class Message
    {
        public abstract void Process(ConnectedClient sender, string[] message);

        protected List<ConnectedClient> clients;
        protected List<ConnectedClient> disconnectList;
        protected List<Unit> units;
        protected NetServer server;
    }
}
