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
            if (aData[1] == client.clientName)
            {
                return;
            }
            else
            {
                int parsed;
                Int32.TryParse(aData[2], out parsed);
                float parsedX = float.Parse(aData[3], client.culture);
                float parsedY = float.Parse(aData[4], client.culture);
                float parsedZ = float.Parse(aData[5], client.culture);
                foreach (Unit unit in client.unitsOnMap)
                {
                    if (unit.unitID == parsed)
                    {
                        parsedX = float.Parse(aData[3], client.culture);
                        parsedY = float.Parse(aData[4], client.culture);
                        parsedZ = float.Parse(aData[5], client.culture);
                        unit.MoveTo(new Vector3(parsedX, parsedY, parsedZ));
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
