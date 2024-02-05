using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag.Equals("Player"))
        {
            other.GetComponent<PlayerScript>().numberOfBombs++;
            Destroy(this.gameObject);
        }
    }
}
