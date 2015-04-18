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
	// Use this for initialization
	void Start () {
        this.lastTimeAttacked = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (this.Target != null)
        {
            if (Vector3.Distance(this.gameObject.transform.position, this.Target.transform.position) > this.MinimumAllowedDistance)
                this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, this.Target.transform.position, Speed * Time.deltaTime);
            if (Time.time > (this.lastTimeAttacked + AttackInterval))
            {
                Debug.Log("Attacked");
                this.lastTimeAttacked = Time.time;
            }
        }
	}
}
