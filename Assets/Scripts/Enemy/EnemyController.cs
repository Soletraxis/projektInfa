using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    #region Variables
    public float HP = 150.0f;
    public float initialSpeed;
    public float speed = 5f;
    public float enemyWidth;
    public float enemyHeight;
    public float idleFrameDuration;
    public float patrolFrameDuration;
    public float patrolDirection;
    public float chaseAfterLosingPlayerSightFrameDuration;
    public float patrolRange;
    public float sightRange = 5f;
    public float attackRange = 2f;
    public int lastAttack;
    public bool isGrounded;
    public bool isFacingWall;
    public bool playerInChaseRange = false;
    public bool playerInAttackRange = false;
    public bool sightOfPlayerLost = false;
    public Vector3 direction;
    public Vector2 patrolVect;
    public Vector3 groundLineCastPosition;
    public Vector3 wallLineCastPosition;
    public Vector3 chasedPlayerPosition;

    [SerializeField]
    public LayerMask groundLayer;

    [SerializeField]
    public LayerMask wallLayer;

    [SerializeField]
    public LayerMask playerLayer;

    [SerializeField]
    public Transform initialPatrolPosition;

    [SerializeField]
    public Transform playerPosition;

    [SerializeField]
    public Rigidbody2D enemyBody;

    [SerializeField]
    public CircleCollider2D sightRangeCollider;

    [SerializeField]
    public CircleCollider2D attackRangeCollider;
    #endregion

    private void Awake()
    {
        initialSpeed = speed;
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sightRangeCollider.radius = sightRange;
        attackRangeCollider.radius = attackRange;
    }

    public void TakeDamage(float DMG)
    {
        HP -= DMG;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void DeathCheck()
    {
        if (HP <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
