using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

#region Variables
    public float runningSpeed = 3.0f;
    public float jumpHeight = 3.0f;
    public float dodgeSpeed = 10.0f;
    public float dodgeInterval = 5.0f;

    private float hAxis;
    private float jAxis;
    private float dodgeAxis;
    private float dodgeTimer;
    private int dodgeCount = 0;
    public bool hasDodged = false;
    private Vector2 movementVect;
    private Vector2 dodgeVect;
    private bool hasJumped = false;

    [SerializeField]
    private Rigidbody2D playerBody;

    [SerializeField]
    private Collider2D playerCollider;

    [SerializeField]
    private LayerMask groundLayers;

    [SerializeField]
    private Transform groundCheck;
#endregion

    #region Start
    void Start () {

	}
#endregion

    #region FixedUpdate
    void FixedUpdate () {
        walkMethod();
        jumpMethod();
        dodgeMethod();
    }
#endregion

    #region Walk Method
    private void walkMethod()
    {
        hAxis = Input.GetAxis("Horizontal");
        if (hAxis != 0.0f)
        {
            movementVect = new Vector2(hAxis * runningSpeed, playerBody.velocity.y);
            playerBody.velocity = movementVect;
        }

        //rotation to keep the groundcheck behind the player, and rotate the sprite
        if (hAxis != 0.0f)
        {
            Vector3 direction = new Vector3(0.0f, 0.0f, hAxis);
            playerBody.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
#endregion

    #region Jump Method
    private void jumpMethod()
    {
        jAxis = Input.GetAxis("Jump");
        if (jAxis > 0.0f)
        {
            bool isGrounded = IsGrounded();
            if (isGrounded == true && hasJumped == false)
            {
                playerBody.AddForce(Vector2.up * jAxis * jumpHeight, ForceMode2D.Impulse);
                hasJumped = true;
            }
        }
        else
        {
            hasJumped = false;
        }

    }
#endregion

    #region Dodge Method
    //no immunity while dodging
    private void dodgeMethod()
    {
        dodgeTimer -= Time.deltaTime;
        if (Input.GetAxis("Dodge") == 1.0f && movementVect != Vector2.zero && dodgeCount == 0)
        {
            if (dodgeTimer <= 0.0f && hasDodged == false)
            {
                dodgeCount = 15;
                dodgeVect = new Vector2(movementVect.x, 0.0f).normalized;
            }
        }
        else
        {
            hasDodged = false;
        }
        if (dodgeCount != 0 && movementVect != Vector2.zero)
        {
            dodgeCount -= 1;
            playerBody.velocity = dodgeVect * dodgeSpeed;
            //dodge animation here
            hasDodged = true;
            dodgeTimer = dodgeInterval;
        }
    }
    #endregion

    #region IsGrounded
    private bool IsGrounded()
    {
        return Physics2D.OverlapPoint(groundCheck.position, groundLayers);
    }
    #endregion

}
