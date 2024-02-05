using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : MonoBehaviour
{
    public Transform parent;
    public CreateSphere sphere;
    public GameObject interactables;
    //We could use the random position spawne
    // Start is called before the first frame update

    static Vector3 GetRandomVector(Dictionary<int, Vector3> keys)
    {
        List<Vector3> vectorList = new List<Vector3>(keys.Values);
        return vectorList[Random.Range(0, vectorList.Count - 1)];
    }
    // Update is called once per frame
    public void Spawn()
    {
        //Debug.Log(index);
        Vector3 location = GetRandomVector(sphere.vertexLocator);
        //Vector3 location = GetIncrementVector(sphere.vertexLocator);
        GameObject pickUp = Instantiate(interactables, location, Quaternion.identity);
        pickUp.transform.parent = parent;
    }
}
