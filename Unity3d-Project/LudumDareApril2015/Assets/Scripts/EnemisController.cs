using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemisController : MonoBehaviour {
    public List<Transform> SpawnPoints;
    public List<EnemyHunger> SpawnedEnemies;
    public int BaseSpawnInternal = 3;
    private float lastTimeEnemySpawned;

    private GameObject Player;
	// Use this for initialization
	void Start () {
        this.Player = GameObject.FindGameObjectWithTag("Player");
        if (this.SpawnedEnemies == null)
            this.SpawnedEnemies = new List<EnemyHunger>();
        this.lastTimeEnemySpawned = Time.time;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > this.lastTimeEnemySpawned + BaseSpawnInternal)
        {
            this.lastTimeEnemySpawned = Time.time;
            int totalSpawnPoints = this.SpawnPoints.Count;
            int spawnPointIndex = Random.Range(0, totalSpawnPoints-1);
            GameObject newEnemy = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Enemy_Hunger"));
            newEnemy.transform.position = this.SpawnPoints[spawnPointIndex].transform.position;
            EnemyHunger enemyHungerComponent = newEnemy.GetComponent<EnemyHunger>();
            enemyHungerComponent.Target = this.Player;
            newEnemy.name = "asdadasd";
        }
	}
}
