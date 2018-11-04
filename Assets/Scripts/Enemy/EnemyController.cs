using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    #region Variables
    public float HP = 150.0f;
    
    #endregion

    public void TakeDamage(float DMG)
    {
        HP -= DMG;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
