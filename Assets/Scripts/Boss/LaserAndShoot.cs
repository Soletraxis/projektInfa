using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAndShoot : MonoBehaviour {

    public float offset;
    public float TimeOfLaser;

    [SerializeField]
    private EnemyController AI;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private GameObject ShootPosition;

    [SerializeField]
    private LineRenderer laser;

    // Use this for initialization
    void Start()
    {
        AI = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        attackMethod();
    }

    #region Attack Method
    void attackMethod()
    {
        Vector3 difference = AI.playerPosition.position - ShootPosition.transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (AI.timeBtwAttack <= 0)
        {
            laser.enabled = true;
            while (TimeOfLaser > 0)
            {
                Ray2D ray = new Ray2D(transform.position, AI.playerPosition.position);
                laser.SetPosition(0, ray.origin);
                laser.SetPosition(1, ray.GetPoint(100));
                TimeOfLaser -= Time.deltaTime;
            }

            laser.enabled = false;

            if (AI.isFightingRange)
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
