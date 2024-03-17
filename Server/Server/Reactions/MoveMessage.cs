using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace Server.NetMessages
{
    public class MoveMessage : Message
    {
        public override void Process(ConnectedClient sender, string[] aData)
        {
            server.Broadcast(clients, "UnitMoved|" + sender.clientName + "|" + //aData[1] + "|" +
                aData[2] + "|" + aData[3] + "|" + aData[4]);
            float parsedX;
            float parsedY;
            float parsedZ;
            float.TryParse(aData[2], out parsedX);
            float.TryParse(aData[3], out parsedY);
            float.TryParse(aData[4], out parsedZ);
            foreach (Unit u in units)
            {
                if (u.clientName == aData[1])
                {
                    u.posX = parsedX;
                    u.posY = parsedY;
                    u.posZ = parsedZ;
                }
            }
            Console.WriteLine("\r\n" + parsedX + "  " + parsedY + "  " + parsedZ);
        }

        public MoveMessage(NetServer _server)
        {
            server = _server;
            clients = server.clients;
            disconnectList = server.disconnectList;
            units = server.units;
        }
    }
}
