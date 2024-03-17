using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using UnityEngine.AI;
using Client.Reactions;

namespace Client
{
    public class NetClient : MonoBehaviour
    {
        [SerializeField] private int port = 6321;
        private bool socketReady;
        private TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private Reactor reactor;
        private void Start()
        {
            Application.runInBackground = true;
            reactor = new Reactor(this);
            DontDestroyOnLoad(gameObject);

            CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }

        public bool ConnectToServer(string host)
        {
            if (socketReady)
                return false;

            try
            {
                socket = new TcpClient(host, port);
                stream = socket.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);

                socketReady = true;
            }
            catch (Exception e)
            {
                Logger.Log("Socket error " + e.Message);
            }

            return socketReady;
        }

        private void Update()
        {
            if (socketReady)
            {
                if (stream.DataAvailable)
                {
                    string data = reader.ReadLine();
                    if (data != null)
                        OnIncomingData(data);
                }
            }
        }

        // Sending message to the server
        public void Send(string data)
        {
            if (!socketReady)
                return;

            writer.WriteLine(data);
            writer.Flush();
        }

        // Read messages from the server
        private void OnIncomingData(string data)
        {
            string[] aData = data.Split('|');
            Logger.Log("Received from server: " + data);

            if (reactor.rections.ContainsKey(aData[0]))
                reactor.rections[aData[0]].Process(aData);
            else
                Logger.Log("Unrecognizable command received");
        }

        private void OnApplicationQuit()
        {
            CloseSocket();
        }
        private void OnDisable()
        {
            CloseSocket();
        }
        public void CloseSocket()
        {
            if (!socketReady)
                return;

            writer.Close();
            reader.Close();
            socket.Close();
            socketReady = false;
        }
    }
}