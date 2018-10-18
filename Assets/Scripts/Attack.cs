using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public float weaponDMG = 0;
    public float weaponCoolDown = 0;
    public float weaponComboTimer = 0;

    public void AttackMethod(Collider2D other)
    {
        print(weaponDMG);
        other.transform.root.GetComponent<EnemyController>().TakeDamage(weaponDMG);
    }

    public void WeaponUpgrade()
    {
        if(transform.childCount != 0)
        {
            weaponDMG = GetComponentInChildren<WeaponStats>(true).weaponDMG;
            weaponCoolDown = GetComponentInChildren<WeaponStats>(true).weaponCoolDown;
            weaponComboTimer = GetComponentInChildren<WeaponStats>(true).weaponComboTimer;
        }
    }
}
