using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public SpawnPlayer playerSpawn;
    public SpawnEnemy enemySpawn;
    public PickUpSpawn pickUp;
    public float enemySpawnTime;
    public float powerSpawnTime;
    private float spawnTimer;
    private float powerupTimer;
    private bool testSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = enemySpawnTime;
        powerupTimer = powerSpawnTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerSpawn.hasSpawned && spawnTimer == 0.0f &&!testSpawn)
        {
            enemySpawn.Spawn();
            spawnTimer = enemySpawnTime;
            //testSpawn = true;
        }else if(spawnTimer <= enemySpawnTime && spawnTimer != 0.0f && playerSpawn.hasSpawned)
        {
            spawnTimer -= Time.fixedDeltaTime;
            if(spawnTimer < 0.0f)
            {
                spawnTimer = 0.0f;
            }
        }
    }
}
