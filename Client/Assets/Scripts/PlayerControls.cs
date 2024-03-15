using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Client
{
    public class PlayerControls : MonoBehaviour
    {
        public static RaycastResult LastRayHit;
        public static GameObject map;
        public static NetClient client;
        public Button spawnButton;
        public Unit controllerUnit;
        private void Awake()
        {
            GameObject go = GameObject.Find("Client");
            if (go == null)
            {
                Debug.Log("Client object not found");
                SceneManager.LoadScene("ClientLogin");
                return;
            }
            client = go.GetComponent<NetClient>();
            if (client == null)
            {
                Debug.Log("Couldn't find client script");
                return;
            }
            map = GameObject.Find("Terrain");
            if (map == null)
            {
                Debug.Log("Couldn't find map");
                return;
            }

            client.Send("SynchronizeRequest|");
        }

        void Update()
        {
            if (controllerUnit == null) return;

            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (PlayerControls.map.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
                {
                    float x = hit.point.x;
                    float y = hit.point.y;
                    float z = hit.point.z;
                    controllerUnit.MoveTo(hit.point);
                    client.Send("Moving|" + controllerUnit.unitID + "|" + x + "|" + y + "|" + z + "|");
                }
            }
        }

        public void SpawnUnit()
        {
            client.Send("SpawnUnit|");
            spawnButton.interactable = false;
        }
    }
}