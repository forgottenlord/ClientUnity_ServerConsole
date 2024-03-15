using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Client.Reactions
{
    public class SpawnReaction : Reaction
    {
        public override void Process(string[] aData)
        {
            GameObject prefab = Resources.Load("Prefabs/Unit1") as GameObject;
            GameObject go = GameObject.Instantiate(prefab);
            Unit un = go.AddComponent<Unit>();
            client.unitsOnMap.Add(un);

            float parsedX = float.Parse(aData[3], client.culture);
            float parsedY = float.Parse(aData[4], client.culture);
            float parsedZ = float.Parse(aData[5], client.culture);
            go.GetComponent<NavMeshAgent>().Warp(new Vector3(parsedX, parsedY, parsedZ));
            int parsed;
            Int32.TryParse(aData[2], out parsed);
            un.unitID = parsed;

            //if (aData[1] == clientName)
            if (aData[1] == client.clientName)
            {
                un.isPlayersUnit = true;
                GameObject.FindObjectOfType<PlayerControls>().controllerUnit = un;
            }
            else
            {
                un.isPlayersUnit = false;
            }
        }

        public SpawnReaction(NetClient _client)
        {
            client = _client;
        }
    }
}
