using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    [SerializeField]
    private EnemyController AI;

    // Use this for initialization
    void Start()
    {
        AI = GetComponent<EnemyController>();
    }

    void FixedUpdate () {
        attackMethod();
	}

    #region Attack Method
    public void attackMethod()
    {
        if (AI.timeBtwAttack <= 0)
        {
            if (AI.isFighting)
            {
                AI.timeBtwAttack = AI.startTimeBtwAttack;
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(AI.enemyBody.position, AI.attackRange, AI.playerLayer);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<PlayerController>().TakeDamage(5.0f);
                }
            }

        }
        else
        {
            AI.timeBtwAttack -= Time.deltaTime;
        }

    }
    #endregion
}
