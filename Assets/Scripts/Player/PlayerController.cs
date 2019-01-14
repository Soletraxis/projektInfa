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
    public float jumpPower = 3.0f;
    public float dodgeSpeed = 10.0f;
    public float dodgeInterval = 5.0f;
    public float dodgeTimer;
    public float startTimeBtwAttack;
    public float attackRange;
    public float maxPlayerHealth = 100;
    public float currentPlayerHealth = 1;

    public int dodgeIFrames = 15;
    public int lastDodgeFrames = 3;
    //public float xd = 3;

    public Animator animator;

    private float hAxis;
    private float dodgeAxis;
    private float attackAxis;
    private float interactAxis;
    private float timeBtwAttack;
    private int dodgeCount = 0;
    private bool hasDodged = false;
    private Vector2 movementVect;
    private Vector2 dodgeVect;
    private bool hasAttacked = false;
    private bool hasInteracted = false;
    private int jumpCount = 2;

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
        DeathCheck();
        attackMethod();
    }
    #endregion

    #region Walk Method
    private void walkMethod()
    {
        hAxis = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(hAxis));
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
        float groundCheckDelay = 0.01f;
        groundCheckDelay -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && jumpCount == 2)
        {
            groundCheckDelay = 0.5f;
            groundCheck.gameObject.SetActive(false);
            jumpCount--;
            //Debug.Log("First jump!");
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpPower);
            //playerBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
        } // to switch back to force based jumping, uncomment the Addforce, and set velocity.y to 0.0f
        else if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded() && jumpCount > 0)
        {
            jumpCount -= 2;
            //Debug.Log("Second jump!");
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpPower);
            //playerBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
        }
        else if (IsGrounded() && jumpCount != 2 && groundCheckDelay < 0.0f)
        {
            animator.SetTrigger("landing");
            jumpCount = 2;
            groundCheck.gameObject.SetActive(true);
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
                if (GetComponentInChildren<PlayerContactChecker>().IsInContact())
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

    #region Attack Method
    private void attackMethod()
    {
        if (timeBtwAttack <= 0)
        {
            attackAxis = Input.GetAxis("Attack");
            if (attackAxis > 0.0f)
            {
                timeBtwAttack = startTimeBtwAttack;
                animator.SetTrigger("attack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(playerBody.position, attackRange, enemyLayers);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(equippedWeapon.GetComponent<Attack>().weaponDMG);
                }
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

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
        /*
        attackAxis = Input.GetAxis("Attack");
        if (other.CompareTag("Enemy") && attackAxis > 0.0f)
        {
            if (hasAttacked == false)
            {
                equippedWeapon.GetComponent<Attack>().AttackMethod(other.GetComponent<Collider2D>());
                hasAttacked = true;
                animator.SetBool("attack", true);
            }
        }
        else
        {
            hasAttacked = false;
        }
        */
        #endregion
    }
    #endregion

    #region DeathCheck
    private void DeathCheck()
    {
        if (currentPlayerHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
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


    #region OnDrawGizmosSelected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerBody.position, attackRange);
    }
    #endregion

    public void TakeDamage(float DMG)
    {
        currentPlayerHealth -= DMG;
        DeathCheck();
    }
}
