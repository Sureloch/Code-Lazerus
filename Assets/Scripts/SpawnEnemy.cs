using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Transform parent;
    public CreateSphere sphere;
    public GameObject[] enemies;
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
        GameObject enemy = Instantiate(enemies[Random.Range(1, 4)], location, Quaternion.identity);
        enemy.transform.parent = parent;
    }
    
}
