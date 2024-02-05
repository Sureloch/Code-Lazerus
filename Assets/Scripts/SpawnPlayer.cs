using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Camera cam;
    public GameObject playerObject;
    public bool hasSpawned = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!hasSpawned)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 spawnPosition = hit.transform.position + 3 * (hit.transform.up);
                    GameObject player = Instantiate(playerObject, spawnPosition, Quaternion.identity);
                    hasSpawned = true;
                }

            }
        }
    }
}
