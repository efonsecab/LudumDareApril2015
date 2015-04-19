using UnityEngine;
using System.Collections;
using System.Linq;

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
    public int PointsPerDefeat = 10;
    private NavMeshAgent NavigationAgent;
    public bool IsValidTarget = false;
    private float DistanceToTarget;
	// Use this for initialization
	void Start () {
        this.lastTimeAttacked = Time.time;
        if (this.Target != null)
        {
            this.PlayerComponent = this.Target.GetComponent<Player>();
            //SetNavigationAgent();
        }
	}

    private void SetNavigationAgent()
    {
        this.NavigationAgent = this.GetComponent<NavMeshAgent>();
        this.NavigationAgent.SetDestination(this.Target.transform.position);
    }

    public float GetDistanceToTarget()
    {
        return this.DistanceToTarget;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.CurrentGamePlayStatus != GameController.GameplayStatus.Play)
            return;
        if (this.Target != null)
        {
            if (this.Target.transform.position.x < this.transform.position.x)
            {
                this.transform.localEulerAngles = new Vector3(0,-90,0);
            }
            else
                if (this.Target.transform.position.x > this.transform.position.x)
                {
                    this.transform.localEulerAngles = new Vector3(0, 90, 0);
                }
            var fwd = transform.rotation * Vector3.forward;
            Debug.DrawRay(this.transform.position, fwd, Color.yellow);
            //Ray newRay = new Ray(this.transform.position, fwd);
            float distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, this.Target.transform.position);
            this.DistanceToTarget = distanceToPlayer;
            if (distanceToPlayer > this.MinimumAllowedDistance)
            {
                NavigateToPlayer();
            }
            if (Time.time > (this.lastTimeAttacked + AttackInterval) && Mathf.CeilToInt(distanceToPlayer) == MinimumAllowedDistance)
            {
                DamagePlayer();
                this.lastTimeAttacked = Time.time;
            }
        }
	}

    private void NavigateToPlayer()
    {
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, this.Target.transform.position, Speed * Time.deltaTime);
        //this.gameObject.transform.LookAt(this.Target.transform.position);
    }

    private void DamagePlayer()
    {
        var fwd = transform.rotation * Vector3.forward;
        Ray newRay = new Ray(this.transform.position, fwd);
        RaycastHit raycastHitInfo = new RaycastHit();
        Collider[] theColliders = Physics.OverlapSphere(this.transform.position, 1);
        bool bHitPlayer = (theColliders != null && theColliders.Length > 0 && theColliders.Where(p => p.gameObject != null && p.gameObject.tag == "Player").Count() > 0);
        bool willCollide = Physics.Raycast(newRay, out raycastHitInfo, (float)this.MinimumAllowedDistance);
        if (willCollide || bHitPlayer)
        {
            if ( (raycastHitInfo.collider != null && raycastHitInfo.collider.gameObject != null && raycastHitInfo.collider.gameObject.tag == "Player") || bHitPlayer)
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
            }
        }
    }
}
