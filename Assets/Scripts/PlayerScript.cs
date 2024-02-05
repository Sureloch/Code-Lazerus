using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public NormalizeGravity gravity;
    public float health;
    public float bombRadius;
    private float moveSpeed;
    public int numberOfBombs = 3;
    public float graceTimer;
    private bool isGracePeriod = false;
    private float timer;
    private Vector3 vectorModifier = Vector3.zero;

    public float damage;
    public float range;


    public LayerMask enemyLayer;
    public LayerMask noLayer;
    // Start is called before the first frame update
    private void Start()
    {
        gravity = GetComponent<NormalizeGravity>();
        moveSpeed = gravity.moveSpeed;
        timer = graceTimer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerAttack();
        GracePeriod();
        if(health <= 0.0f)
        {
            Destroy(this.gameObject);
        }
        if(numberOfBombs > 0)
        {
            ExpelEnemies();
        }
    }
    public void TakeDamage(int damage)
    {
        if(isGracePeriod)
        {
            return;
        }else
        {
            health -= damage;
            timer = graceTimer;
            isGracePeriod = true;
        }
    }
    void GracePeriod()
    {
        if(isGracePeriod)
        {
            this.GetComponent<CapsuleCollider>().excludeLayers = enemyLayer;
            timer -= Time.fixedDeltaTime;
            if (timer < 0.0f)
            {
                isGracePeriod = false;
                this.GetComponent<CapsuleCollider>().excludeLayers = noLayer;
            }
        }
    }
    void ExpelEnemies()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            numberOfBombs--;
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, bombRadius);
            foreach(Collider col in colliders)
            {
                if(col.transform.tag.Equals("Enemy"))
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position + (5 * this.transform.forward + (2 * this.transform.up)), range);
    } 
    void PlayerAttack()
    {
      /*  if(Input.GetKeyDown(KeyCode.S))
        {
            vectorModifier = -this.transform.forward;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            vectorModifier = -this.transform.right;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            vectorModifier = this.transform.forward;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            vectorModifier = this.transform.right;
        }*/

        if (Input.GetKey(KeyCode.W))
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position + (5 * this.transform.forward + (2 * this.transform.up)), range);
            foreach (Collider col in colliders)
            {
                if (col.transform.tag.Equals("Enemy"))
                {
                    if(col.GetComponent<SnakeBoss>() == null)
                    {
                        if(col.GetComponent<BodyPartScript>() == null)
                        {
                            col.GetComponent<EnemyScripts>().health -= damage;
                        }else
                        {
                            col.GetComponent<BodyPartScript>().health -= damage;
                        }
                    }else
                    {
                        col.GetComponent<SnakeBoss>().health -= damage;
                    }
                    
                }
            }
        }
    }

}
