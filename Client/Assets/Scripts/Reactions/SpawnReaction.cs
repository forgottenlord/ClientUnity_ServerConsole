using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client.Reactions
{
    public class SpawnReaction : Reaction
    {
        public override void Process(string[] aData)
        {
            Unit unit = World.Spawn(aData[1], World.clientName == aData[1]);
            unit.Iterate(new Data.UnitState());
        }

        public SpawnReaction(NetClient _client)
        {
            client = _client;
        }
    }
}
