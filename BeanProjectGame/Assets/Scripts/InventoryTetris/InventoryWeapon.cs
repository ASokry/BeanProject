using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWeapon : InventoryItem
{
    public enum WeaponType { Pistol, Shotgun, Specialty, Melee, None };
    public enum AimType { AutoTargeting, AreaTargeting, None };

    public WeaponType weaponType = WeaponType.None;
    public AimType aimType = AimType.None;
    public float range;
    public float minnimumRange;
    public float damagePerShot;
    public string ammoType;
    public int clipSize;
    public int curAmmo;
    public float timeBetweenAttacks;
    public float baseWeaponAccuracy;
    public string[] specialEffects;
    private bool isEquipped = false;

    public virtual void SetCurAmmo(int ammoModifier)
    {
        curAmmo = curAmmo + ammoModifier;
        if (curAmmo > clipSize)
        {
            curAmmo = clipSize;
        }
        if (curAmmo < 0)
        {
            curAmmo = 0;
        }
    }

    public void SetEquippedState(bool b)
    {
        isEquipped = b;
    }

    public bool GetEquippedState()
    {
        return isEquipped;
    }
}
