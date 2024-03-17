using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Reactions
{
    public class MoveReaction : Reaction
    {
        public override void Process(string[] aData)
        {
            if (aData[1] != World.clientName)
            {
                return;
            }
            else
            {
                float parsedX = float.Parse(aData[2]);
                float parsedY = float.Parse(aData[3]);
                float parsedZ = float.Parse(aData[4]);
                foreach (var u in World.units)
                {
                    if (u.Key == aData[1])
                    {
                        u.Value.MoveTo(new Vector3(parsedX, parsedY, parsedZ));
                    }
                }
            }
        }

        public MoveReaction(NetClient _client)
        {
            client = _client;
        }
    }
}
