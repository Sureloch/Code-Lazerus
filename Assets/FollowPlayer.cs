using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook freeLook;
    public SpawnPlayer player;
    public GameObject playerBody;
    public Transform center;
    public CinemachineTargetGroup group;
    private bool hasBeenAdded = false;
    // Start is called before the first frame uate
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.hasSpawned)
        {
            playerBody = GameObject.Find("PlayerBody(Clone)");
            if(!hasBeenAdded)
            {
                group.AddMember(playerBody.transform, 0.0f, 1.0f);
                hasBeenAdded = true;
            }
        }
       
        
    }
}
