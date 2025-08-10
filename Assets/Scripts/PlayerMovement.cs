using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController control;

    float movementSpeed;
    public float walkSpeed, sprintSpeed, crouchSpeed;
    public float jumpForce = 5f;        
    public float gravity = 9.8f;
    float verticalSpeed;

    public bool grounded;

    [Header("Grapple")]
    float grappleLength = 800;
    public bool roofExist;
    
    public FixedJoint grappleJoint;
    public bool grappled = false;
    public float grappleMoveSpeed;
    public GameObject ropePrefab;
    public Transform hitBoxPos;
 
    float ropeCheckSize = 1f;
    GameObject attachedGrapple;

    public KeyCode grappleKey;
    public KeyCode grappleAttach;
    public KeyCode sprintKey;
    public KeyCode crouchKey;


    public LayerMask roofLayer;
    public LayerMask ropeLayer;
    public LayerMask groundLayer;

    RaycastHit hit;

    public Transform cameraFollow;

    public States state;
    public enum States
    {
        walking,
        crouching,
        sprinting
    }
    private void Start()
    {
        control = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.down, 1.2f, groundLayer) || Physics.Raycast(transform.position, Vector3.down, 1.2f, roofLayer))
            grounded = true;    
        else
            grounded = false;
        if(!grappled)
            MovementHandler();
        GrappleCheck();
        GrappleHandler();
        StateCheck();
        CameraHeight();
    }
    void MovementHandler()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement = transform.TransformDirection(movement);
        movement *= movementSpeed * Time.deltaTime;
        
        if (!grounded)
        {
            verticalSpeed -= gravity * Time.deltaTime;
        }
        else
        {
            verticalSpeed = 0f;
        }
        if (grounded && Input.GetButtonDown("Jump"))
        {
            verticalSpeed = jumpForce;
        }

        movement.y = verticalSpeed;

        control.Move(movement);
    }
    void CameraHeight()
    {
        if (state == States.crouching)
            cameraFollow.localPosition = new Vector3(0, 0, 0);
        else
            cameraFollow.localPosition = new Vector3(0, .412f, 0);
    }
    void StateCheck()
    {
        if(grounded && Input.GetKey(sprintKey))
        {
            movementSpeed = sprintSpeed;
            state = States.sprinting;
        } 
        else if(grounded && Input.GetKey(crouchKey))
        {
            movementSpeed = crouchSpeed;
            state = States.crouching;
        }
        else
        {
            movementSpeed = walkSpeed;
            state = States.walking;
        }

    }

    #region GrappleLogic
    void GrappleCheck()
    {
        if (Input.GetKeyDown(grappleAttach) && Physics.OverlapSphere(hitBoxPos.position, ropeCheckSize, ropeLayer).Length > 0)
        {
                    foreach (Object obj in gameObject.GetComponents<FixedJoint>())
                    {
                        Destroy(obj);
                    }
                    attachedGrapple = Physics.OverlapSphere(hitBoxPos.position, ropeCheckSize, ropeLayer)[0].gameObject;
                    Transform ropeTop = attachedGrapple.transform.Find("RopeTop");
                    Transform ropeBottom = attachedGrapple.transform.Find("RopeBottom");
                    transform.position = attachedGrapple.transform.position;
                    grappled = true;               
        }
    }

    void CreateRope()
    {
        GameObject newRope = Instantiate(ropePrefab);
        Transform ropeTop = newRope.transform.Find("RopeTop");
        Transform ropeBottom = newRope.transform.Find("RopeBottom");
        newRope.transform.position = transform.position;
        Physics.Raycast(transform.position, gameObject.transform.up, out hit, grappleLength, roofLayer);
        ropeTop.position = hit.point;
        Physics.Raycast(transform.position, -gameObject.transform.up, out hit, grappleLength, groundLayer);
        ropeBottom.position = hit.point;
        LineRenderer ropeRender = newRope.GetComponentInChildren<LineRenderer>();
        ropeRender.SetPosition(0, ropeTop.position);
        ropeRender.SetPosition(1, ropeBottom.position);
    }

    void GrappleHandler()
    {
        if (Input.GetKeyDown(grappleKey) && !grappled)
        {
            CreateRope();
        }
        if (grappled)
        {
            float verticalInput = 0f;
            if (Input.GetKey(KeyCode.W))
            {
                verticalInput = 1f; // Move up
            }
            else if (Input.GetKey(KeyCode.S))
            {
                verticalInput = -1f; // Move down
            }
            Vector3 verticalVelocity = Vector3.up * verticalInput * grappleMoveSpeed;
            control.Move(verticalVelocity);
            if (Input.GetKeyDown(grappleKey))
            {
                DetachGrapple();
            }
        }
    }

    public void DetachGrapple()
    {      
        Destroy(grappleJoint);
        Destroy(GetComponent<Rigidbody>());
        grappled = false;
        foreach (Object obj in gameObject.GetComponents<FixedJoint>())
        {
            Destroy(obj);
        }
    }
    #endregion
}
