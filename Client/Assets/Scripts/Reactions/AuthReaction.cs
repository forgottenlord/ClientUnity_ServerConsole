using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Client.Reactions
{
    public class AuthReaction : Reaction
    {
        public override void Process(string[] message)
        {
            SceneManager.LoadScene("ClientGameView");
        }

        public AuthReaction(NetClient _client)
        {
            client = _client;
        }
    }
}
