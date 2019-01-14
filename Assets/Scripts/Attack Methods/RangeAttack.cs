using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour {

    public float offset;

    [SerializeField]
    private EnemyController AI;

    [SerializeField]
    private GameObject projectile;

    // Use this for initialization
    void Start () {
        AI = GetComponent<EnemyController>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        attackMethod();
	}

    #region Attack Method
    void attackMethod()
    {
        Vector3 difference = AI.playerPosition.position - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (AI.timeBtwAttack <= 0)
        {
            if (AI.isFighting)
            {
                AI.timeBtwAttack = AI.startTimeBtwAttack;
                Instantiate(projectile, transform.position, Quaternion.Euler(0.0f, 0.0f, rotZ + offset));
            }
        }
        else
        {
            AI.timeBtwAttack -= Time.deltaTime;
        }
    }
    #endregion
}
