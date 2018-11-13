using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerContactChecker : MonoBehaviour {



    #region isInContact
    public bool isInContact()
    {
        return Physics2D.OverlapCircle(transform.position, GetComponentInParent<BoxCollider2D>().size.x/2, LayerMask.GetMask("Enemy"));

        /*return Physics2D.OverlapBox(new Vector2(-GetComponentInParent<BoxCollider2D>().bounds.extents.x / 2.0f, -GetComponentInParent<BoxCollider2D>().bounds.extents.y / 2.0f),
            new Vector2(GetComponentInParent<BoxCollider2D>().bounds.extents.x / 2.0f, GetComponentInParent<BoxCollider2D>().bounds.extents.y / 2.0f), LayerMask.GetMask("Enemy"));
    */
    }
    #endregion
}
