using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Reactions
{
    public class Reactor
    {
        public Dictionary<string, Reaction> rections;
        public Reactor(NetClient client)
        {
            rections = new Dictionary<string, Reaction>()
            {
                { "WhoAreYou", new HandShakeReaction(client) },
                { "Authenticated", new AuthReaction(client) },
                { "UnitSpawned", new SpawnReaction(client) },
                { "UnitMoved", new MoveReaction(client) },
                { "SyncMsg", new SyncReaction(client) },
            };
        }
    }
}
