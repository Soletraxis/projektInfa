using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContactChecker : MonoBehaviour {

	public bool IsInContact()
    {
        return Physics2D.OverlapCircle(transform.position, GetComponentInParent<BoxCollider2D>().size.x / 2, LayerMask.GetMask("Enemy"));
    }
}
