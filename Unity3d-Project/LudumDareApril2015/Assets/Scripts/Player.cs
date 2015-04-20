using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public enum LookingTo
{
    Left,
    Right
}
public class Player : MonoBehaviour
{
    public LookingTo CurrentLookingTo;
    public Stats CurrentStats;
    public int AttackDamage = 10;
    public EnemisController EnemyController;
    private int InitialHP;
    public Slider PlayerHealhPointsBar;
    public int Speed = 1;
    public int ImpulseForceOnEnemy = 10;
    private Animator[] AnimatorControllers;
    public SceneController SceneController;
    // Use this for initialization
    void Start()
    {
        this.InitialHP = this.CurrentStats.HealthPoints;
        this.AnimatorControllers = this.GetComponentsInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (GameController.CurrentGamePlayStatus != GameController.GameplayStatus.Play)
            return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = this.transform.localScale;
        if ( (h != 0 || v != 0) && Input.GetKeyUp(KeyCode.Z) == false )
        {
            SetTrigger("Mover");
        }
        if (h < 0 && this.CurrentLookingTo != LookingTo.Left)
        {
            this.CurrentLookingTo = LookingTo.Left;
            //this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
            transform.localEulerAngles = new Vector3(0, -90, 0);
            //direction = new Vector3(this.transform.localScale.x*-1, 1, 1);
            //this.transform.localScale = direction;
        }
        else 
            if (h > 00 && this.CurrentLookingTo != LookingTo.Right)
            {
                this.CurrentLookingTo = LookingTo.Right;
                //this.gameObject.GetComponent<Renderer>().material.color = Color.red;
                transform.localEulerAngles = new Vector3(0, 90, 0);
                //direction = new Vector3(this.transform.localScale.x*-1,1,1);
                //this.transform.localScale = direction;
            }
        Vector3 dest = this.transform.position;
        dest.x += h;
        dest.z += v;
        //Debug.LogFormat("Moving To: {0},{1}, {2}", dest.x, dest.y, dest.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, dest, Speed * Time.deltaTime);
        var fwd = transform.rotation * Vector3.forward;
        Debug.DrawRay(this.transform.position, fwd, Color.black);
        if (Input.GetKeyUp(KeyCode.Z))
        {
            SetTrigger("Atacar");
            EnemyHunger closestEnemy = this.EnemyController.SpawnedEnemies.OrderBy(d => d.GetDistanceToTarget()).FirstOrDefault();
            if (closestEnemy != null && closestEnemy.IsValidTarget)
            {
                ApplyAttack(fwd, closestEnemy.gameObject, closestEnemy);
            }
            Ray newRay = new Ray(this.transform.position, fwd);
            RaycastHit raycastHitInfo = new RaycastHit();
            //AttackByRaycasting(ref fwd, ref newRay, ref raycastHitInfo);
            //Debug.LogFormat("{0},{1}, {2}", fwd.x, fwd.y, fwd.z);
            //this.transform.position += fwd;
        }
	}

    private void SetTrigger(string trigger)
    {
        foreach (Animator singleAnimator in this.AnimatorControllers)
        {
            if (singleAnimator.runtimeAnimatorController != null && singleAnimator.isActiveAndEnabled)
            {
                Debug.Log(trigger);
                singleAnimator.SetTrigger(trigger);
            }
        }
    }

    private void AttackByRaycasting(ref Vector3 fwd, ref Ray newRay, ref RaycastHit raycastHitInfo)
    {
        bool willCollide = Physics.Raycast(newRay, out raycastHitInfo);
        if (willCollide)
        {
            if (raycastHitInfo.collider != null)
            {
                var collidedGameObject = raycastHitInfo.collider.gameObject;
                EnemyHunger enemyHungerComponent = collidedGameObject.GetComponent<EnemyHunger>();
                ApplyAttack(fwd, collidedGameObject, enemyHungerComponent);
            }
        }
        else
            Debug.Log("Nothing in front");
    }

    private void ApplyAttack(Vector3 fwd, GameObject collidedGameObject, EnemyHunger enemyHungerComponent)
    {
        if (enemyHungerComponent != null)
        {
            Rigidbody enemyRigidBody = collidedGameObject.GetComponent<Rigidbody>();
            enemyRigidBody.AddForce(fwd * ImpulseForceOnEnemy, ForceMode.Impulse);
            Debug.LogFormat("Can attack: {0}", collidedGameObject.name);
            int enemyCurrentHP = enemyHungerComponent.CurrentStats.HealthPoints;
            enemyCurrentHP -= this.AttackDamage;
            if (enemyCurrentHP < 0)
                enemyCurrentHP = 0;
            enemyHungerComponent.CurrentStats.HealthPoints = enemyCurrentHP;
            if (enemyHungerComponent.CurrentStats.HealthPoints == 0)
            {
                this.EnemyController.DefeatEnemy(enemyHungerComponent);
            }
        }
    }

    void OnGUI()
    {
        float currentHPPercentage = (float)this.CurrentStats.HealthPoints / (float)this.InitialHP;
        this.PlayerHealhPointsBar.value = currentHPPercentage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            EnemyHunger enemyHungerComponent = other.gameObject.GetComponent<EnemyHunger>();
            Debug.LogFormat("Collided: {0}", other.name);
            if (enemyHungerComponent != null)
            {
                this.EnemyController.SetValidTarget(enemyHungerComponent, true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null)
        {
            EnemyHunger enemyHungerComponent = other.gameObject.GetComponent<EnemyHunger>();
            Debug.LogFormat("Collided: {0}", other.name);
            if (enemyHungerComponent != null)
            {
                this.EnemyController.SetValidTarget(enemyHungerComponent, false);
            }
        }
    }




}
