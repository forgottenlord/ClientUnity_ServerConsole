using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace Server.NetMessages
{
    public class SpawnMessage : Message
    {
        public override void Process(ConnectedClient sender, string[] message)
        {
            Unit unit = new Unit();

            unit.clientName = sender.clientName;
            //give a new ID to the new units
            int newid = 0;
            foreach (Unit u in units)
            {
                if (u.unitID >= newid) { newid = u.unitID + 1; }
            }

            unit.unitID = newid;
            unit.posX = 1.0f;
            unit.posY = 0.0f;
            unit.posZ = 1.0f;
            units.Add(unit);
            server.Broadcast(clients, "UnitSpawned|" +
                sender.clientName + "|" +
                unit.unitID + "|" +
                unit.posX + "|" +
                unit.posY + "|" +
                unit.posZ);
        }

        public SpawnMessage(NetServer _server)
        {
            server = _server;
            clients = server.clients;
            disconnectList = server.disconnectList;
            units = server.units;
        }
    }
}
