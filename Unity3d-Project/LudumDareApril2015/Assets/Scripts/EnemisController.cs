using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemisController : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public List<EnemyHunger> SpawnedEnemies;
    public int BaseSpawnInternal = 3;
    private float lastTimeEnemySpawned;
    public int MaxAllowedEnemiesInScene = 1;
    private ScoreSystem PlayerScoreSystem;

    private GameObject Player;
    // Use this for initialization
    void Start()
    {
        this.Player = GameObject.FindGameObjectWithTag("Player");
        if (this.SpawnedEnemies == null)
            this.SpawnedEnemies = new List<EnemyHunger>();
        this.lastTimeEnemySpawned = Time.time;
        this.PlayerScoreSystem = this.Player.GetComponent<ScoreSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.CurrentGamePlayStatus != GameController.GameplayStatus.Play)
            return;
        if (Time.time > this.lastTimeEnemySpawned + BaseSpawnInternal && this.SpawnedEnemies.Count < this.MaxAllowedEnemiesInScene)
        {
            this.lastTimeEnemySpawned = Time.time;
            int totalSpawnPoints = this.SpawnPoints.Count;
            int spawnPointIndex = Random.Range(0, totalSpawnPoints - 1);
            GameObject newEnemy = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Enemy_Hunger"));
            newEnemy.transform.position = this.SpawnPoints[spawnPointIndex].transform.position;
            EnemyHunger enemyHungerComponent = newEnemy.GetComponent<EnemyHunger>();
            enemyHungerComponent.Target = this.Player;
            this.SpawnedEnemies.Add(enemyHungerComponent);
            //newEnemy.name = "asdadasd";
        }
    }

    internal void DefeatEnemy(EnemyHunger enemyHunger)
    {
        int indexOfEnemy = this.SpawnedEnemies.IndexOf(enemyHunger);
        Debug.LogFormat("Index of enemy: {0}", indexOfEnemy);
        if (indexOfEnemy >= 0)
        {
            this.PlayerScoreSystem.AddToScore(enemyHunger.PointsPerDefeat);
            GameObject.Destroy(enemyHunger.gameObject);
            this.SpawnedEnemies.RemoveAt(indexOfEnemy);
            this.PlayerScoreSystem.EnemiesDefeated++;
        }
    }

    internal void SetValidTarget(EnemyHunger enemyHungerComponent, bool isValidTarget)
    {
        enemyHungerComponent.IsValidTarget = isValidTarget;
    }
}
