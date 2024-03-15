using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    public class Unit : MonoBehaviour
    {
        public int unitID;
        public bool isPlayersUnit;

        public void MoveTo(Vector3 pos)
        {
            GetComponent<NavMeshAgent>().SetDestination(pos);
        }
    }
}