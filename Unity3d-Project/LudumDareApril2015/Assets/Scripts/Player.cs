using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum LookingTo
    {
        Left,
        Right
    }
    public LookingTo CurrentLookingTo;
    public Stats CurrentStats;
    public int AttackDamage = 10;
    public EnemisController EnemyController;
    private int InitialHP;
    public Slider PlayerHealhPointsBar;
    // Use this for initialization
    void Start()
    {
        this.InitialHP = this.CurrentStats.HealthPoints;
    }

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = this.transform.localScale;
        if (h < 0 && this.CurrentLookingTo != LookingTo.Left)
        {
            this.CurrentLookingTo = LookingTo.Left;
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
            transform.localEulerAngles = new Vector3(0, -90, 0);
            //direction = new Vector3(this.transform.localScale.x*-1, 1, 1);
            //this.transform.localScale = direction;
        }
        else 
            if (h > 00 && this.CurrentLookingTo != LookingTo.Right)
            {
                this.CurrentLookingTo = LookingTo.Right;
                this.gameObject.GetComponent<Renderer>().material.color = Color.red;
                transform.localEulerAngles = new Vector3(0, 90, 0);
                //direction = new Vector3(this.transform.localScale.x*-1,1,1);
                //this.transform.localScale = direction;
            }
        Vector3 dest = this.transform.position;
        dest.x += h;
        dest.z += v;
        //Debug.LogFormat("Moving To: {0},{1}, {2}", dest.x, dest.y, dest.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, dest, 0.1f);
        var fwd = transform.rotation * Vector3.forward;
        Debug.DrawRay(this.transform.position, fwd, Color.black);
        if (Input.GetKeyUp(KeyCode.Z))
        {
            Ray newRay = new Ray(this.transform.position, fwd);
            RaycastHit raycastHitInfo = new RaycastHit();
            bool willCollide = Physics.Raycast(newRay, out raycastHitInfo);
            if (willCollide)
            {
                if (raycastHitInfo.collider != null)
                {
                    var collidedGameObject = raycastHitInfo.collider.gameObject;
                    EnemyHunger enemyHungerComponent = collidedGameObject.GetComponent<EnemyHunger>();
                    if (enemyHungerComponent != null)
                    {
                        Debug.LogFormat("Can attack: {0}", collidedGameObject.name);
                        int enemyCurrentHP = enemyHungerComponent.CurrentStats.HealthPoints;
                        enemyCurrentHP -= this.AttackDamage;
                        if (enemyCurrentHP < 0)
                            enemyCurrentHP = 0;
                        enemyHungerComponent.CurrentStats.HealthPoints = enemyCurrentHP;
                        if (enemyHungerComponent.CurrentStats.HealthPoints == 0)
                            this.EnemyController.DefeatEnemy(enemyHungerComponent);
                    }
                }
            }
            else
                Debug.Log("Nothing in front");
            Debug.DrawRay(this.transform.position, fwd, Color.red);
            //Debug.LogFormat("{0},{1}, {2}", fwd.x, fwd.y, fwd.z);
            //this.transform.position += fwd;
        }
	}

    void OnGUI()
    {
        float currentHPPercentage = (float)this.CurrentStats.HealthPoints / (float)this.InitialHP;
        this.PlayerHealhPointsBar.value = currentHPPercentage;
    }
}
