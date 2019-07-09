﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    #region Variables
    private float healthSliderTimer;
    public float healthSliderInitialTime = 1f;
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
    public float startTimeBtwAttack;
    public float timeBtwAttack;
    public int lastAttack;
    public bool isGrounded;
    public bool isFacingWall;
    public bool playerInChaseRange = false;
    public bool playerInAttackRange = false;
    public bool sightOfPlayerLost = false;
    public bool isFighting = false;
    public bool isFightingRange = false;
    public bool isFightingJump = false;
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

    [SerializeField]
    public Slider healthSlider;
    #endregion

    private void Awake()
    {
        initialSpeed = speed;
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sightRangeCollider.radius = sightRange;
        attackRangeCollider.radius = attackRange;
    }

    private void Start()
    {
        healthSlider.maxValue = HP;
        healthSlider.value = HP;
        healthSliderTimer = -1;
        healthSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        healthSliderTimer -= Time.deltaTime;
        if(healthSliderTimer > 0.0f)
        {
            healthSlider.gameObject.SetActive(true);
        }
        else if(healthSliderTimer < 0.0f)
        {
            healthSlider.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float DMG)
    {
        healthSliderTimer = healthSliderInitialTime;
        HP -= DMG;
        healthSlider.value = HP;
        DeathCheck();
    }

    private void DeathCheck()
    {
        if (HP <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
