using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server.NetMessages;

namespace Server
{
    public class ConnectedClient
    {
        public string clientName;
        private TcpClient tcpClient;
        public byte[] endPoint;
        public string stringEndPoint;
        private byte[] GetEndPoint()
        {
            var address = tcpClient.Client.RemoteEndPoint.Serialize();
            if (address.Size == 4)
                return new byte[] { address[0], address[1], address[2], address[3] };
            else
                return new byte[] { address[0], address[1], address[2], address[3], address[4], address[5] };
        }
        private string GetStringEndPoint()
        {
            StringBuilder sb = new StringBuilder();
            var bytes = GetEndPoint();
            sb.Append(bytes[0]);
            for (int n=1; n< bytes.Length;n++)
            {
                sb.Append(".").Append(bytes[n]);
            }
            return sb.ToString();
        }
        public ConnectedClient(TcpClient tcp)
        {
            this.tcpClient = tcp;
            endPoint = GetEndPoint();
            stringEndPoint = GetStringEndPoint();
        }

        public void Disconnect()
        {
            tcpClient.Close();
            tcpClient.Dispose();
        }

        public NetworkStream GetStream()
        {
            return tcpClient.GetStream();
        }

        public bool IsConnected()
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    if (tcpClient.Client.Poll(0, SelectMode.SelectRead))
                        return !(tcpClient.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
