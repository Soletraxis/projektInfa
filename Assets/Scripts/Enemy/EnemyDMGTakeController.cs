﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDMGTakeController : MonoBehaviour {

    [SerializeField]
    private BoxCollider2D parentCollider2D;

    void Awake()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(parentCollider2D.size.x, parentCollider2D.size.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            //GetComponentInParent<PlayerController>().HP -= collision.GetComponentInParent<EnemyController>().weaponDMG;
        }
    }
}
