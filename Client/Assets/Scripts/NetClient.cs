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
        public string clientName;
        [SerializeField] private int portToConnect = 6321;
        public string password;
        private bool socketReady;
        private TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        public InputField clientNameInputField;
        public InputField serverAddressInputField;
        public InputField passwordInputField;
        public List<Unit> unitsOnMap = new List<Unit>();
        public CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        private Reactor reactor;
        private void Start()
        {
            reactor = new Reactor(this);
            DontDestroyOnLoad(gameObject);
            culture.NumberFormat.NumberDecimalSeparator = ".";
        }

        public bool ConnectToServer(string host, int port)
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
                Debug.Log("Socket error " + e.Message);
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
            Debug.Log("Received from server: " + data);

            if (reactor.rections.ContainsKey(aData[0]))
                reactor.rections[aData[0]].Process(aData);
            else
                Debug.Log("Unrecognizable command received");
        }



        private void OnApplicationQuit()
        {
            CloseSocket();
        }
        private void OnDisable()
        {
            CloseSocket();
        }
        private void CloseSocket()
        {
            if (!socketReady)
                return;

            writer.Close();
            reader.Close();
            socket.Close();
            socketReady = false;
        }


        public void ConnectToServerButton()
        {
            password = passwordInputField.text;
            clientName = clientNameInputField.text;
            CloseSocket();
            try
            {
                ConnectToServer(serverAddressInputField.text, portToConnect);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}