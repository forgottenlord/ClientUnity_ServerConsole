using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Client;

namespace Client.UI
{
    public class LoginPanel : MonoBehaviour
    {
        public InputField clientNameInputField;
        public InputField serverAddressInputField;
        public InputField passwordInputField;
        public Button connectButton;
        [SerializeField] NetClient client;

        private void OnEnable()
        {
            connectButton.onClick.AddListener(ConnectToServerButton);
        }
        private void OnDisable()
        {
            connectButton.onClick.RemoveAllListeners();
        }

        public void ConnectToServerButton()
        {
            World.clientName = clientNameInputField.text;
            World.password = passwordInputField.text;
            client.CloseSocket();
            try
            {
                client.ConnectToServer(serverAddressInputField.text);
            }
            catch (Exception e)
            {
                Logger.Log(e.Message);
            }
        }
    }
}
