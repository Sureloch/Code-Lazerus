using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizeGravity : MonoBehaviour
{
    private Transform centerPositon;
    public float forceMultiplier;
    public float moveSpeed;


    public Camera cam;
    private Rigidbody rb;
    public Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        centerPositon = GameObject.Find("Center").transform;
        rb.useGravity = false;
        targetPosition = transform.position; // Initialize the target to the current position.
        cam = GameObject.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 center = (centerPositon.position - transform.position).normalized;
        Vector3 gravity = center * forceMultiplier;
        rb.AddForce(-gravity, ForceMode.Acceleration);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.tag.Equals("Area"))
                    {
                    targetPosition = hit.point;
                    }
                }
            }
        
        Vector3 moveDirection = targetPosition - transform.position;
        Vector3 moveVector = moveDirection.normalized * moveSpeed;
        rb.velocity = moveVector;

        // Update rotation to look at the center and follow the sphere's surface.
        //This handles the correct rotation of the object
        this.transform.LookAt(center); //Look At Center but this is the forward Vector looking towards the center
        this.transform.RotateAround(transform.position, transform.right, 90f);//Due to the nature of the prefab, make the up vector face the Centre 
        Vector3 directionToFlag = targetPosition - transform.position; //Need to turn so that the forward componet of the gameobject faces the targetPosition
                                                                       // Calculate the angle in degrees to rotate around the up axis.
        float angleToRotate = Vector3.SignedAngle(transform.forward, directionToFlag, transform.up);
        // Rotate the object by the calculated angle.
        this.transform.RotateAround(transform.position, transform.up, angleToRotate);
    }
}
