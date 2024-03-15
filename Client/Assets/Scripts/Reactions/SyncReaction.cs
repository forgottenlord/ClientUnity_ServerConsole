using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Client.Reactions
{
    public class SyncReaction : Reaction
    {
        public override void Process(string[] aData)
        {
            int numberOfUnitsOnServersMap;
            Int32.TryParse(aData[1], out numberOfUnitsOnServersMap);
            int serverUnitID;
            int[] serverUnitIDs = new int[numberOfUnitsOnServersMap];
            float parsedX;
            float parsedY;
            float parsedZ;
            for (int i = 0; i < numberOfUnitsOnServersMap; i++)
            {
                Int32.TryParse(aData[2 + i * 4], out serverUnitID);
                serverUnitIDs[i] = serverUnitID;
                bool didFind = false;
                foreach (Unit unit in client.unitsOnMap) //synchronize existing units
                {
                    if (unit.unitID == serverUnitID)
                    {
                        parsedX = float.Parse(aData[3 + i * 4], client.culture);
                        parsedY = float.Parse(aData[4 + i * 4], client.culture);
                        parsedZ = float.Parse(aData[5 + i * 4], client.culture);
                        unit.MoveTo(new Vector3(parsedX, parsedY, parsedZ));
                        didFind = true;
                    }
                }
                if (!didFind) //add non-existing (at client) units
                {
                    GameObject prefab = Resources.Load("Prefabs/Unit1") as GameObject;
                    GameObject go = GameObject.Instantiate(prefab);
                    Unit un = go.AddComponent<Unit>();
                    client.unitsOnMap.Add(un);
                    un.unitID = serverUnitID;

                    parsedX = float.Parse(aData[3 + i * 4], client.culture);
                    parsedY = float.Parse(aData[4 + i * 4], client.culture);
                    parsedZ = float.Parse(aData[5 + i * 4], client.culture);
                    go.GetComponent<NavMeshAgent>().Warp(new Vector3(parsedX, parsedY, parsedZ));
                }

            }
            //remove units which are not on server's list (like disconnected ones)
            foreach (Unit unit in client.unitsOnMap)
            {
                bool exists = false;
                for (int i = 0; i < serverUnitIDs.Length; i++)
                {
                    if (unit.unitID == serverUnitIDs[i])
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    GameObject.Destroy(unit.gameObject);
                    client.unitsOnMap.Remove(unit);
                }
            }
        }

        public SyncReaction(NetClient _client)
        {
            client = _client;
        }
    }
}
