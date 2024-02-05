using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{

    // Settings
    public float MoveSpeed;
    public float BodySpeed;
    public float SlowSpeed;
    public float NormSpeed;
    public int Gap = 10;
    public float forceMultiplier;
    public float chaseTimer;
    public bool slowed;
    public float health;
    // References
    public GameObject BodyPrefab;
    private Rigidbody rb;
    public Vector3 targetPosition;
    private Transform centerPosition;

    // Lists
    public List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        centerPosition = GameObject.Find("Center").transform;
        rb.useGravity = false;
        targetPosition = GameObject.Find("PlayerBody(Clone)").transform.position;
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
    }
    void ChasePlayer()
    {
        chaseTimer -= Time.fixedDeltaTime;
        targetPosition = GameObject.Find("PlayerBody(Clone)").transform.position;
        if (chaseTimer < 0.0f)
        {
            Destroy(this.gameObject);
        }
        Vector3 moveDirection = targetPosition - transform.position;
        Vector3 moveVector = moveDirection.normalized * MoveSpeed;
        rb.velocity = moveVector;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (slowed)
        {
            MoveSpeed = SlowSpeed;
        }
        else
        {
            MoveSpeed = NormSpeed;
        }
        CheckSnake();
        Vector3 center = (centerPosition.position - transform.position).normalized;
        Vector3 gravity = center * forceMultiplier;
        rb.AddForce(-gravity, ForceMode.Acceleration);
        ChasePlayer();
        this.transform.LookAt(center); //Look At Center but this is the forward Vector looking towards the center
        this.transform.RotateAround(transform.position, transform.right, 90f);//Due to the nature of the prefab, make the up vector face the Centre 
        Vector3 directionToFlag = targetPosition - transform.position; //Need to turn so that the forward componet of the gameobject faces the targetPosition                                                                      // Calculate the angle in degrees to rotate around the up axis.
        float angleToRotate = Vector3.SignedAngle(transform.forward, directionToFlag, transform.up);
        // Rotate the object by the calculated angle.
        this.transform.RotateAround(transform.position, transform.up, angleToRotate);
        // Store position history


        PositionsHistory.Insert(0, transform.position);
        // Move body parts
        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];
            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt(point);
            index++;
        }
    }

    private void GrowSnake()
    {
        // Instantiate body instance and
        // add it to the list
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
        body.GetComponent<BodyPartScript>().index = BodyParts.Count - 1;

    }
    public void CheckSnake()
    {
        for (int i = 0; i < BodyParts.Count; i++)
        {
            if (BodyParts[i].Equals(null))
            {
                BodyParts.RemoveRange(i, (BodyParts.Count - i));
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Player"))
        {
            foreach (GameObject body in BodyParts)
            {
                if (health <= 0.0f)
                {
                    Destroy(body);
                }
                else
                {
                    collision.gameObject.GetComponent<PlayerScript>().TakeDamage(10);
                }

            }
            if (health <= 0.0f)
            {
                Destroy(this.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerScript>().TakeDamage(10);
            }
        }
    }
}