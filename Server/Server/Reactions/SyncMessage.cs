using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace Server.NetMessages
{
    public class SyncMessage : Message
    {
        public override void Process(ConnectedClient sender, string[] message)
        {
            StringBuilder dataToSend = new StringBuilder("Synchronizing|").Append(units.Count);
            foreach (Unit u in units)
            {
                dataToSend.Append("|").Append(u.unitID)
                    .Append("|").Append(u.posX)
                    .Append("|").Append(u.posY)
                    .Append("|").Append(u.posZ);
            }
            server.Broadcast(sender, dataToSend.ToString());
            Console.WriteLine("\r\nSynchronization " + sender.clientName + " request sent: " + dataToSend);
        }

        public SyncMessage(NetServer _server)
        {
            server = _server;
            clients = server.clients;
            disconnectList = server.disconnectList;
            units = server.units;
        }
    }
}
