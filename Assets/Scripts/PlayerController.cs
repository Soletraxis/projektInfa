using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //to do: Add animations, tweak player invincibility, to last until exit of an enemy collider
    //fix attack method

    #region Variables
    public float runningSpeed = 3.0f;
    public float jumpHeight = 3.0f;
    public float dodgeSpeed = 10.0f;
    public float dodgeInterval = 5.0f;
    public int dodgeIFrames = 15;
    public int lastDodgeFrames = 3;

    private float hAxis;
    private float jAxis;
    private float dodgeAxis;
    private float attackAxis;
    private float interactAxis;
    private float dodgeTimer;
    private int dodgeCount = 0;
    public bool hasDodged = false;
    private Vector2 movementVect;
    private Vector2 dodgeVect;
    private bool hasJumped = false;
    private bool hasAttacked = false;
    private bool hasInteracted = false;

    [SerializeField]
    private Rigidbody2D playerBody;

    [SerializeField]
    private LayerMask groundLayers;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private GameObject equippedWeapon;

    [SerializeField]
    private BoxCollider2D PlayerHitbox;
    #endregion

    #region Start
    void Start() {

    }
    #endregion

    #region FixedUpdate
    void FixedUpdate() {
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
    private void dodgeMethod()
    {
        dodgeTimer -= Time.deltaTime;
        if (Input.GetAxis("Dodge") == 1.0f && movementVect != Vector2.zero && dodgeCount == 0)
        {
            if (dodgeTimer <= 0.0f && hasDodged == false)
            {
                dodgeCount = dodgeIFrames;
                dodgeVect = new Vector2(movementVect.x, 0.0f).normalized;
            }
        }
        else
        {
            hasDodged = false;
        }
        if (dodgeCount != 0 && movementVect != Vector2.zero)
        {
            //dodge animation here
            dodgeCount -= 1;
            hasDodged = true;
            dodgeTimer = dodgeInterval;
            if (dodgeCount <= dodgeIFrames && dodgeCount >= lastDodgeFrames)
            {
                this.gameObject.layer = LayerMask.NameToLayer("Invincible");
                playerBody.velocity = dodgeVect * dodgeSpeed;
            }
            else if (dodgeCount < lastDodgeFrames)
            {
                this.gameObject.layer = LayerMask.NameToLayer("Player");
                playerBody.velocity = dodgeVect * dodgeSpeed / 2.0f;
            }
        }
    }
    #endregion

    #region IsGrounded
    private bool IsGrounded()
    {
        return Physics2D.OverlapPoint(groundCheck.position, groundLayers);
    }
    #endregion

    private void OnTriggerStay2D(Collider2D other)
    {
        #region Picking stuff up
        interactAxis = Input.GetAxis("Interact");
        if (other.CompareTag("Weapon") && interactAxis == 1.0f)
        {
            if (hasInteracted == false)
            {
                other.transform.root.gameObject.SetActive(false);
                other.transform.parent.SetParent(equippedWeapon.transform);
                hasInteracted = true;
                equippedWeapon.GetComponent<Attack>().WeaponUpgrade();
            }
        }
        else
        {
            hasInteracted = false;
        }
        #endregion

        #region Attack Method
        attackAxis = Input.GetAxis("Attack");
        if (other.CompareTag("Enemy") && attackAxis == 1.0f)
        {
            if (hasAttacked == false)
            {
                equippedWeapon.GetComponent<Attack>().AttackMethod(other);
                hasAttacked = true;
            }
        }
        else
        {
            hasAttacked = false;
        }
        #endregion
    }


}
