using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client.Data
{
    public struct UnitState
    {
        public string unitID;
        public Vector3 pos;
        public UnitState(params string[] data)
        {
            unitID = data[0];
            pos = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
        }
    }
}
