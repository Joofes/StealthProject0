using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Wall Running")]
    public LayerMask wallLayer;
    public float wallrunForce, wallRunTime, wallRunMaxSpeed, wallRunMinSpeed;
    float wallRunTimer;
    public bool isWallRight, isWallLeft;
    public bool isWallRunning;
    bool wallRunReset;
    public float maxWallRunCameraTilt, wallRunCameraTilt;
    GameObject currentWall;
    RaycastHit hit;
    [Header("Grapple")]
    public float grappleLength;
    public bool roofExist;
    public LayerMask roofLayer;
    public FixedJoint grappleJoint;
    public bool grappled = false;
    public float grappleMoveSpeed;
    public GameObject ropePrefab;
    public Transform hitBoxPos;
    public LayerMask ropeLayer;
    float ropeCheckSize = 1f;
    GameObject attachedGrapple;

    bool jumping;
  
    public KeyCode grappleKey;
    public KeyCode grappleAttach;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        grappled,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        if(attachedGrapple == null && grappled)
        {
            DetachGrapple();
        }
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if(!grappled && grappleJoint != null)
        {
            ResetAllGrapples();
        }
        if (Input.GetKeyDown(grappleKey) && !isWallRunning && !grappled)
        {
            CreateRope();
        }
        MyInput();
        SpeedControl();
        CheckForWall();
        WallRunInput();
        StateHandler();
        GrappleCheck();
        GrappleHandler();
        if (isWallRunning && rb.velocity.magnitude <= wallRunMinSpeed)
            StopWallRun();
        // handle drag
        if (jumping)
            rb.drag = 0;
        if (grounded && verticalInput == 0 && horizontalInput == 0 && !jumping)
        {
            rb.drag = 200;
        }
        else if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        if(isWallRunning)
        {
            wallRunTimer -= Time.deltaTime;
            if(wallRunTimer <= 0)
            {
                StopWallRun();
            }
        }
        if(grounded)
        {
            wallRunReset = true;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    void GrappleCheck()
    {
        if(Input.GetKeyDown(grappleAttach))
        {
            if(Physics.OverlapSphere(hitBoxPos.position, 2f) != null)
            {
                if (Physics.OverlapSphere(hitBoxPos.position, ropeCheckSize, ropeLayer).Length > 0)
                {
                    GameObject attached = Physics.OverlapSphere(hitBoxPos.position, ropeCheckSize, ropeLayer)[0].gameObject;
                    attachedGrapple = attached;
                    Transform ropeTop = attached.transform.Find("RopeTop");
                    Transform ropeBottom = attached.transform.Find("RopeBottom");
                    transform.position = attached.transform.position;
                    grappled = true;
                    ResetAllGrapples();
                    grappleJoint = gameObject.AddComponent<FixedJoint>();
                    grappleJoint.breakForce = Mathf.Infinity;
                    grappleJoint.connectedBody = ropeTop.GetComponent<Rigidbody>();
                }
               
            }
        }
            /*Physics.Raycast(transform.position, orientation.up, out hit, grappleLength, roofLayer);
            if(hit.collider != null)
            {
                grappled = true;
                grappleJoint = gameObject.AddComponent<FixedJoint>();
                grappleJoint.breakForce = Mathf.Infinity;
                grapplePoint.transform.position = hit.point;
                grappleJoint.connectedBody = grapplePoint.GetComponent<Rigidbody>();
                //rb.velocity = Vector3.zero;
            }*/
        
    }
    void CreateRope()
    {
        GameObject newRope = Instantiate(ropePrefab);     
        Transform ropeTop = newRope.transform.Find("RopeTop");
        Transform ropeBottom = newRope.transform.Find("RopeBottom");
        newRope.transform.position = transform.position;
        Physics.Raycast(transform.position, orientation.up, out hit, grappleLength, roofLayer);
        ropeTop.position = hit.point;
        Physics.Raycast(transform.position, -orientation.up, out hit, grappleLength, whatIsGround);
        ropeBottom.position = hit.point;
        LineRenderer ropeRender = newRope.GetComponentInChildren<LineRenderer>();
        ropeRender.SetPosition(0, ropeTop.position);
        ropeRender.SetPosition(1, ropeBottom.position);
    }
    void GrappleHandler()
    {
        if (grappled)
        {

            rb.useGravity = false;
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

            rb.velocity = verticalVelocity;
            //stop grapple
            if (Input.GetKeyDown(grappleKey) && grappled)
            {
                DetachGrapple();
                
            }
        }
    }
    public void DetachGrapple()
    {
        rb.useGravity = true;
        Destroy(grappleJoint);
        Vector3 forwardForce = orientation.transform.forward * 50;
        Vector3 upwardForce = orientation.transform.up * 50;
        rb.AddForce(orientation.up * jumpForce * 10);
        rb.AddForce(orientation.forward * jumpForce * 15);
        grappled = false;
        ResetAllGrapples();
    }
    void ResetAllGrapples()
    {
        foreach (Object obj in gameObject.GetComponents<FixedJoint>())
        {
            Destroy(obj);
        }
    }
    private void MyInput()
    {
        if(isWallRunning)
            horizontalInput = 0;
        else
            horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && (grounded || isWallRunning) && !grappled)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey) && !grappled)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey) && !grappled && !isWallRunning)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded && !grappled && !isWallRunning)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else if (grappled)
        {
            state = MovementState.grappled;
            moveSpeed = 0;
        }
        // Mode - Air
        else
        {
            state = MovementState.air;
            moveSpeed = walkSpeed;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        if(!isWallRunning && !grappled)
            rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        jumping = true;
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(!isWallRunning)
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        if(isWallRunning)
        {
            readyToJump = false;

            //normal jump
            if (isWallLeft && !Input.GetKey(KeyCode.D) || isWallRight && !Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.up * jumpForce * 20f);
            }

            //sidwards wallhop
            //if (isWallRight || isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) rb.AddForce(-orientation.up * jumpForce * 1f);
            if (isWallRight) rb.AddForce(-orientation.right * jumpForce * 80.2f);
            if (isWallLeft) rb.AddForce(orientation.right * jumpForce * 80.2f);

            //Always add forward force
            rb.AddForce(orientation.forward * jumpForce * 1f);
            wallRunReset = true;
            StopWallRun();
        }
        StartCoroutine(ResetJumpDrag());
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }
    IEnumerator ResetJumpDrag()
    {
        yield return new WaitForSeconds(1f);
        jumping = false;
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }


    void WallRunInput()
    {
        if(Input.GetKeyDown(KeyCode.D) && isWallRight && rb.velocity.magnitude > wallRunMinSpeed && !grounded && wallRunReset)
        {
            StartWallRun();
        }
        if (Input.GetKeyDown(KeyCode.A) && isWallLeft && rb.velocity.magnitude > wallRunMinSpeed && !grounded && wallRunReset)
        {
            StartWallRun();
        }
    }
    void StartWallRun()
    {
        wallRunReset = false;
        wallRunTimer = wallRunTime;
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        isWallRunning = true;

        if (rb.velocity.magnitude <= wallRunMaxSpeed)
        {
            rb.AddForce(orientation.forward * Time.deltaTime * wallrunForce);

            if (isWallRight)
            {
                rb.AddForce(orientation.right * wallrunForce / 5 * Time.deltaTime);
            }
            else if (isWallLeft)
            {
                rb.AddForce(-orientation.right * wallrunForce / 5 * Time.deltaTime);
            }
        }
    }
    void StopWallRun()
    {
        rb.useGravity = true;
        isWallRunning = false;
    }
    void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1.5f, wallLayer);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1.5f, wallLayer);

        if(isWallRight)
        {
            Physics.Raycast(transform.position, orientation.right, out hit, 1.5f, wallLayer);
        }
        if (isWallLeft)
        {
            Physics.Raycast(transform.position, -orientation.right, out hit, 1.5f, wallLayer);
        }
        if(hit.collider != null)
            currentWall = hit.collider.gameObject;
        if (!isWallLeft && !isWallRight)
        {
            StopWallRun();
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(hitBoxPos.position, ropeCheckSize);
    }
}