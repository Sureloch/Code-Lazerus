using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScripts : MonoBehaviour
{
    public bool readToMove;
    private Transform centerPosition;
    public float forceMultiplier;
    public float moveSpeed;

    private float maxHealth;
    public float health;

    private Rigidbody rb;
    public Vector3 targetPosition;
    public int bounces = 3;

    public enum MovementType
    {
        LAST_POSITION_CHASE, PLAYER_CHASE, LINEAR_MOVEMENT
    }
    public MovementType movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        centerPosition = GameObject.Find("Center").transform;
        rb.useGravity = false;
        targetPosition = GameObject.Find("PlayerBody(Clone)").transform.position; // Initialize the target to the current position.
        maxHealth = health;
    }
    void ChaseLastPosition()
    {
        if (Vector3.Distance(this.transform.position, targetPosition) <= Random.Range(1.5f, 4.5f))
        {
            --bounces;
            if (bounces <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                targetPosition = GameObject.Find("PlayerBody(Clone)").transform.position;
            }
        }
        Vector3 moveDirection = targetPosition - transform.position;
        Vector3 moveVector = moveDirection.normalized * moveSpeed;
        rb.velocity = moveVector;
    }
    void ChasePlayer()
    {
        targetPosition = GameObject.Find("PlayerBody(Clone)").transform.position;
        Vector3 moveDirection = targetPosition - transform.position;
        Vector3 moveVector = moveDirection.normalized * moveSpeed;
        rb.velocity = moveVector;
    }
    void MoveLinearly()
    {
        targetPosition = this.transform.position + 4 * transform.forward;
        Vector3 moveDirection = targetPosition - transform.position;
        Vector3 moveVector = moveDirection.normalized * moveSpeed;
        rb.velocity = moveVector;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckHealth();
        Vector3 center = (centerPosition.position - transform.position).normalized;
        Vector3 gravity = center * forceMultiplier;
        rb.AddForce(-gravity, ForceMode.Acceleration);
        if(movement == MovementType.LAST_POSITION_CHASE)
        {
            ChaseLastPosition();
        }else if(movement == MovementType.PLAYER_CHASE)
        {
            ChasePlayer();
        }else if(movement == MovementType.LINEAR_MOVEMENT)
        {
            MoveLinearly();
        }
        // Update rotation to look at the center and follow the sphere's surface.
        //This handles the correct rotation of the object
        this.transform.LookAt(center); //Look At Center but this is the forward Vector looking towards the center
        this.transform.RotateAround(transform.position, transform.right, 90f);//Due to the nature of the prefab, make the up vector face the Centre 
        Vector3 directionToFlag = targetPosition - transform.position; //Need to turn so that the forward componet of the gameobject faces the targetPosition                                                                      // Calculate the angle in degrees to rotate around the up axis.
        float angleToRotate = Vector3.SignedAngle(transform.forward, directionToFlag, transform.up);
        // Rotate the object by the calculated angle.
        this.transform.RotateAround(transform.position, transform.up, angleToRotate);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(10);
        }
    }
    void CheckHealth()
    {
        if(health <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

}