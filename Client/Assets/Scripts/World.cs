using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Client.Data;
using UnityEngine.AI;

namespace Client
{
    public static class World
    {
        public static string clientName;
        public static string password;

        public static Dictionary<string, Unit> units = new Dictionary<string, Unit>();

        public static Unit Spawn(string unitName, bool isLocalPlayer)
        {
            GameObject prefab = Resources.Load("Prefabs/Unit1") as GameObject;
            GameObject go = GameObject.Instantiate(prefab);
            Unit unit = go.AddComponent<Unit>();
            if (isLocalPlayer)
            {
                PlayerControls controll = GameObject.FindObjectOfType<PlayerControls>();
                controll.controllerUnit = unit;
                unit.name = unitName;
            }
            units.Add(unitName, unit);
            return unit;
        }
        public static void Remove(string unitName)
        {
            GameObject.Destroy(units[unitName].gameObject);
            units.Remove(unitName);
        }

        public static void Iterate()
        {

        }
    }
}
