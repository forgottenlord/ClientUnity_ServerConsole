using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Client.Data;

namespace Client
{
    public class Unit : MonoBehaviour
    {
        public void MoveTo(Vector3 pos)
        {
            GetComponent<NavMeshAgent>().SetDestination(pos);
        }

        public void Iterate(UnitState state)
        {
            MoveTo(state.pos);
        }
    }
}