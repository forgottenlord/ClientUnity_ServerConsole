using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Client.Data;

namespace Client.Reactions
{
    public class SyncReaction : Reaction
    {
        public override void Process(string[] aData)
        {
            int numberOfUnitsOnServersMap;
            Int32.TryParse(aData[1], out numberOfUnitsOnServersMap);
            List<UnitState> states = new List<UnitState>();

            for (int i = 0; i < numberOfUnitsOnServersMap; i++)
            {
                string unitName = aData[2 + i * 4];
                if (World.units.ContainsKey(unitName))
                {
                    World.units[unitName].Iterate(new Data.UnitState(aData[2 + i * 4],
                       aData[3 + i * 4], aData[3 + i * 4], aData[3 + i * 4]));
                }
                else
                {
                    Unit unit = World.Spawn(unitName, World.clientName == unitName);
                    unit.Iterate(new Data.UnitState(aData[2 + i * 4],
                       aData[3 + i * 4], aData[3 + i * 4], aData[3 + i * 4]));
                }
            }

            ///Если есть ЛИШНИЕ юниты
            if (World.units.Count > numberOfUnitsOnServersMap)
            {
                foreach (var unit in World.units)
                {

                }
            }

            ///Если юнитов нехватает
            if (World.units.Count > numberOfUnitsOnServersMap)
            {

            }
        }

        public SyncReaction(NetClient _client)
        {
            client = _client;
        }
    }
}
