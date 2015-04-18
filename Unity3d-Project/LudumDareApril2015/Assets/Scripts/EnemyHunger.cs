using UnityEngine;
using System.Collections;

public class EnemyHunger : MonoBehaviour {
    //[SerializeField]
    public Stats CurrentStats;
    public int Speed = 1;
    public GameObject Target;
    public float MinimumAllowedDistance;
    public int AttackInterval = 5;
    private float lastTimeAttacked;
    public int AttackDamage;
    private Player PlayerComponent;
	// Use this for initialization
	void Start () {
        this.lastTimeAttacked = Time.time;
        if (this.Target != null)
            this.PlayerComponent = this.Target.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.Target != null)
        {
            float distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, this.Target.transform.position);
            if ( distanceToPlayer > this.MinimumAllowedDistance)
                this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, this.Target.transform.position, Speed * Time.deltaTime);
            if (Time.time > (this.lastTimeAttacked + AttackInterval) && Mathf.CeilToInt(distanceToPlayer) == MinimumAllowedDistance)
            {
                int currentPlayerHP = this.PlayerComponent.CurrentStats.HealthPoints;
                currentPlayerHP -= this.AttackDamage;
                if (currentPlayerHP <= 0)
                    currentPlayerHP = 0;
                this.PlayerComponent.CurrentStats.HealthPoints = currentPlayerHP;
                if (this.PlayerComponent.CurrentStats.HealthPoints == 0)
                {
                    Debug.Log("Player lost");
                    Time.timeScale = 0;
                }
                Debug.Log("Attacked");
                this.lastTimeAttacked = Time.time;
            }
        }
	}
}
