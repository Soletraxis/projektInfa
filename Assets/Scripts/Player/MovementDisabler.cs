using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDisabler : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !PlayerIsInvincible())
        {
            collision.GetComponent<Rigidbody2D>().isKinematic = true;
            collision.GetComponent<EnemyController>().speed = 0.0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Rigidbody2D>().isKinematic = false;
            collision.GetComponent<EnemyController>().speed = collision.GetComponent<EnemyController>().initialSpeed;
        }
    }

    private bool PlayerIsInvincible()
    {
        if (GetComponentInParent<PlayerController>().gameObject.layer == 14)/*layer 14 = "invincible"*/
        {
            return true;
        }
        else { return false; }
    }
}
