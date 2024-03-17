using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server.NetMessages;

namespace Server
{
    public class NetServer
    {
        private int port = 6321;

        public List<ConnectedClient> clients = new List<ConnectedClient>();
        public List<ConnectedClient> disconnectList = new List<ConnectedClient>();
        public List<Unit> units = new List<Unit>();

        private TcpListener server;
        private bool serverStarted;
        private bool ResyncNeeded = false;
        private Dictionary<string, Message> messages;


        //the constructor, adds the listener
        public NetServer()
        {
            try
            {
                server = new TcpListener(IPAddress.Any, port);
                messages = new Dictionary<string, Message>()
                {
                    { "Iam", new AuthMessage(this)},
                    { "SpawnUnit", new SpawnMessage(this)},
                    { "Moving", new MoveMessage(this)},
                    { "SyncMsg", new SyncMessage(this)},
                };
                server.Start();
                StartListening();
                Console.WriteLine("server started on port:" + port);
                serverStarted = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\n" + e.Message);
            }
        }

        ~NetServer()
        {
            foreach (ConnectedClient client in clients)
            {
                client.Disconnect();
            }
        }

        public void Stop()
        {
            foreach (ConnectedClient client in clients)
            {
                client.Disconnect();
            }
        }

        //called at every fixed time intervals => time can be adjusted at timer component's property
        //used to check if there's incoming data
        public void Iterate()
        {
            if (!serverStarted)
                return;

            foreach (ConnectedClient client in clients)
            {
                if (!client.IsConnected())
                //if (!IsConnected(client.socket))
                {
                    // если клиент отвалился
                    client.Disconnect();
                    disconnectList.Add(client);
                    continue;
                }
                else
                {
                    // если клиент всё еще подключен
                    NetworkStream s = client.GetStream();
                    if (s.DataAvailable)
                    {
                        StreamReader reader = new StreamReader(s, true);
                        string data = reader.ReadLine();

                        if (data != null)
                            OnIncomingData(client, data);
                    }
                }
            }

            //checking disconnected players
            for (int i = 0; i < disconnectList.Count - 1; i++)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    if (units[j].clientName == disconnectList[i].clientName)
                    {
                        units.RemoveAt(j);
                    }
                }
                Console.WriteLine("\r\nUser disconnected:" + disconnectList[i].clientName);
                clients.Remove(disconnectList[i]);
                disconnectList.RemoveAt(i);
                ResyncNeeded = true;
            }
            //if some1 disconnected, tell it to other players as well.
            if (ResyncNeeded)
            {
                SynchronizeUnits();
                ResyncNeeded = false;
            }
            ///синхронизация персонажей
            SynchronizeUnits();

            Console.Clear();
            for (int n = 0; n < clients.Count; n++)
            {
                if (units.Count > n)
                    Console.WriteLine(string.Format("{0}){1}; {2};         pos:{3},{4},{5}", n,
                        clients[n].clientName, clients[n].stringEndPoint,
                        units[n].posX, units[n].posY, units[n].posZ));
                else
                    Console.WriteLine(string.Format("{0}){1}; {2};         dead", n,
                        clients[n].clientName, clients[n].stringEndPoint));
            }
        }

        private void StartListening()
        {
            server.BeginAcceptTcpClient(AcceptTcpClient, server);
        }

        private void AcceptTcpClient(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;

            string allUsers = "";
            foreach (ConnectedClient i in clients)
            {
                allUsers += i.clientName + '|';
            }

            ConnectedClient sc = new ConnectedClient(listener.EndAcceptTcpClient(ar));
            clients.Add(sc);

            StartListening();
            //request authentication from client
            Broadcast(clients[clients.Count - 1], "WhoAreYou|");
        }

        // Server Send
        public void Broadcast(List<ConnectedClient> cl, string data)
        {
            Logger.Log(data);
            foreach (ConnectedClient sc in cl)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(sc.GetStream());
                    writer.WriteLine(data);
                    writer.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine("\r\n" + e.Message);
                }
            }
        }

        public void Broadcast(ConnectedClient c, string data)
        {
            List<ConnectedClient> sc = new List<ConnectedClient> { c };
            Broadcast(sc, data);
        }

        // Server Read
        private void OnIncomingData(ConnectedClient c, string data)
        {
            string[] aData = data.Split('|');

            if (messages.ContainsKey(aData[0]))
            {
                messages[aData[0]].Process(c, aData);
            }
        }

        //syncing all clients
        public void SynchronizeUnits()
        {
            string dataToSend = "SyncMsg|" + units.Count;
            foreach (Unit u in units)
            {
                dataToSend += "|" + (u.clientName) + "|" + u.posX + "|" + u.posY + "|" + u.posZ;
            }
            Broadcast(clients, dataToSend);
            Console.WriteLine("\r\n SyncMsg request sent: " + dataToSend);
        }
    }

}
