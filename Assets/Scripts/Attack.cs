using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public float weaponDMG = 0;
    public float weaponCoolDown = 0;
    public float weaponComboTimer = 0;

    public void AttackMethod()
    {
        print(weaponDMG);
    }

    private void FixedUpdate()
    {
        if(transform.childCount != 0)
        {
            weaponDMG = GetComponentInChildren<WeaponStats>(true).weaponDMG;
            weaponCoolDown = GetComponentInChildren<WeaponStats>(true).weaponCoolDown;
            weaponComboTimer = GetComponentInChildren<WeaponStats>(true).weaponComboTimer;
        }
    }
}
