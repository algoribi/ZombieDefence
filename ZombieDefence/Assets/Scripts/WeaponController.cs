using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public IWeapon weapon;
    public void ChangeWeapon(IWeapon weapon_)
    {
        weapon = weapon_;
    }
    public void Attack()
    {
        weapon.Attack();
    }
}
