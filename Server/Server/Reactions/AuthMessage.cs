using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace Server.NetMessages
{
    public class AuthMessage : Message
    {
        public override void Process(ConnectedClient sender, string[] aData)
        {
            //bool authenticated = Database.AuthenticateUser(aData[1], aData[2]);
            //bool authenticated = clients.Find(c=> c.endPoint == sender.endPoint) != null;
            bool authenticated = clients.Find(c => c.clientName == sender.clientName) != null;
            if (authenticated)
            {
                foreach (ConnectedClient client in clients)
                {
                    if (aData[1] == client.clientName /*||
                        sender.endPoint == client.endPoint*/)
                    {
                        Console.WriteLine("\r\nThis user is already connected"+ sender.stringEndPoint);
                        client.Disconnect();
                        disconnectList.Add(sender);
                        return;
                    }
                }
                sender.clientName = aData[1];
                Console.WriteLine("\r\nUser authenticated");
                server.Broadcast(sender, "Authenticated|");
            }
            else
            {
                Console.WriteLine("\r\nUser authentication failed, client disconnected.");
                sender.Disconnect();
                disconnectList.Add(sender);
            }
        }

        public AuthMessage(NetServer _server)
        {
            server = _server;
            clients = server.clients;
            disconnectList = server.disconnectList;
            units = server.units;
        }
    }
}
