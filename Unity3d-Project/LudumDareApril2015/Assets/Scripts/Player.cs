using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Stats CurrentStats;
    public int AttackDamage = 10;
    public EnemisController EnemyController;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            var fwd = transform.rotation * Vector3.forward;
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
}
