using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Reactions
{
    public class HandShakeReaction : Reaction
    {
        public override void Process(string[] message)
        {
            client.Send("Iam|" + World.clientName + "|" + World.password);
        }

        public HandShakeReaction(NetClient _client)
        {
            client = _client;
        }
    }
}
