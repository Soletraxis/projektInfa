using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //to do: Add animations, tweak player invincibility, to last until exit of an enemy collider
    //fix attack method
    //add changing weapon stats and image at the beginning
    //fix double jump

    #region Variables
    public float runningSpeed = 3.0f;
    public float jumpHeight = 3.0f;
    public float dodgeSpeed = 10.0f;
    public float dodgeInterval = 5.0f;
    public float dodgeTimer;

    public int dodgeIFrames = 15;
    public int lastDodgeFrames = 3;
    public int maxPlayerHealth = 100;
    public int currentPlayerHealth = 1;
    //public float xd = 3;

    private float hAxis;
    private bool jAxis;
    private float dodgeAxis;
    private float attackAxis;
    private float interactAxis;
    private int dodgeCount = 0;
    private bool hasDodged = false;
    private Vector2 movementVect;
    private Vector2 dodgeVect;
    private bool hasJumped = false;
    private bool canDoubleJump = false;
    private bool canPressJump = true;
    private bool hasAttacked = false;
    private bool hasInteracted = false;
    private int jumpCount = 0;

    [SerializeField]
    private Rigidbody2D playerBody;

    [SerializeField]
    private LayerMask groundLayers;

    [SerializeField]
    private LayerMask enemyLayers;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform equippedWeapon;
    #endregion

    #region Start
    void Start() {
        //ADD STATS AND IMAGE OF DEFAULT WEAPON
        currentPlayerHealth = maxPlayerHealth;
        equippedWeapon.transform.GetChild(0).gameObject.SetActive(true);
        equippedWeapon.GetComponent<Attack>().WeaponUpgrade();
        GameManager.instance.hudManager.heldWeaponDisplay.GetComponent<Image>().sprite = equippedWeapon.transform.GetComponentInChildren<SpriteRenderer>().sprite;
        equippedWeapon.transform.GetChild(0).gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region FixedUpdate
    void FixedUpdate() {
        walkMethod();
        jumpMethod();
        dodgeMethod();
        //EnemyCollisionExit();
    }
    #endregion

    #region Walk Method
    private void walkMethod()
    {
        hAxis = Input.GetAxis("Horizontal");
        if (hAxis != 0.0f)
        {
            movementVect = new Vector2(hAxis * runningSpeed, playerBody.velocity.y);
            //rotation to keep the groundcheck behind the player, and rotate the sprite
            Vector3 direction = new Vector3(0.0f, 0.0f, hAxis);
            playerBody.transform.rotation = Quaternion.LookRotation(direction);
            playerBody.velocity = movementVect;
        }
    }
    #endregion

    #region Jump Method
    private void jumpMethod()
    {
        bool isGrounded = IsGrounded();
        if (isGrounded)
        {
            jumpCount = 0;
        }

        jAxis = Input.GetKeyDown(KeyCode.Space);
        float jumpForce = 1f;
        if (jAxis)
        {
            if (canPressJump && jumpCount < 1)
            {
                print(jumpCount);
                if(playerBody.velocity.y < 0)
                {
                    jumpForce *= 2;
                }
                playerBody.velocity.Set(playerBody.velocity.x, 0);
                playerBody.AddForce(Vector2.up * jumpForce* jumpHeight, ForceMode2D.Impulse);
                hasJumped = true;
                canDoubleJump = true;
                canPressJump = false;
                jumpCount += 1;
            } else
            {
                if(canDoubleJump)
                {
                    canDoubleJump = false;
                    playerBody.AddForce(Vector2.up * jumpForce * jumpHeight, ForceMode2D.Impulse);
                    jumpCount += 1;
                }
            }
        } else
        {
            canPressJump = true;
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
            this.gameObject.layer = LayerMask.NameToLayer("Invincible");
            if (dodgeCount <= dodgeIFrames && dodgeCount >= lastDodgeFrames)
            {
                //dodge extension when on an enemy
                if (GetComponentInChildren<playerContactChecker>().isInContact())
                {
                    dodgeCount++;
                }
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

    #region OnTriggerStay
    private void OnTriggerStay2D(Collider2D other)
    {
        #region Picking stuff up
        interactAxis = Input.GetAxis("Interact");
        if (other.CompareTag("Weapon") && interactAxis == 1.0f)
        {
            if (hasInteracted == false)
            {
                equippedWeapon.transform.GetChild(0).gameObject.SetActive(true);
                equippedWeapon.GetComponentInChildren<WeaponStats>().transform.parent = null;
                GameManager.instance.hudManager.heldWeaponDisplay.GetComponent<Image>().sprite = other.GetComponentInParent<SpriteRenderer>().sprite;
                other.transform.root.gameObject.SetActive(false);
                other.transform.parent.SetParent(equippedWeapon.transform);
                other.transform.parent.localPosition = Vector2.zero;
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
        if (other.CompareTag("Enemy") && attackAxis > 0.0f)
        {
            if (hasAttacked == false)
            {
                equippedWeapon.GetComponent<Attack>().AttackMethod(other.GetComponent<Collider2D>());
                hasAttacked = true;
            }
        }
        else
        {
            hasAttacked = false;
        }
        #endregion
    }
    #endregion

    #region OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit"))
        {
            GameManager.instance.NextLevel();
        }
    }
    #endregion

    /*#region Collision Methods

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy") && playerBody.isKinematic == false)
        {
            playerBody.isKinematic = true;
            playerBody.velocity = Vector2.zero;
            other.collider.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            playerBody.isKinematic = true;
        }
    }

    private void EnemyCollisionExit()
    {
        if (playerBody.isKinematic == true)
        {
            playerBody.isKinematic = false;
        }
    }
    #endregion*/
}
