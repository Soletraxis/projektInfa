using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTouchDMG : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(20);
        }
    }
}
