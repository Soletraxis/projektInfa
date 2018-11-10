using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSphereColliderController : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerHiddenByObstacles())
        {
            GetComponentInParent<EnemyController>().playerInAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<EnemyController>().playerInAttackRange = false;
        }
    }

    bool PlayerHiddenByObstacles()
    {
        float distanceToPlayer = Vector2.Distance(GetComponentInParent<EnemyController>().transform.position, GetComponentInParent<EnemyController>().playerPosition.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(GetComponentInParent<EnemyController>().transform.position, GetComponentInParent<EnemyController>().playerPosition.position - GetComponentInParent<EnemyController>().transform.position, distanceToPlayer);

        foreach (RaycastHit2D hit in hits)
        {
            // ignore the enemy's own colliders (and other enemies)
            if (hit.transform.tag == "Enemy")
                continue;

            // if anything other than the player is hit then it must be between the player and the enemy's eyes (since the player can only see as far as the player)
            if (hit.transform.tag != "Player")
            {
                return true;
            }
        }
        // if no objects were closer to the enemy than the player return false (player is not hidden by an object)
        return false;

    }
}
