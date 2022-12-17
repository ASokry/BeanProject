using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmItem : ItemObject
{
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmo;

    public int GetMaxAmmo() { return maxAmmo; }
    public int GetCurrentAmmo() { return currentAmmo; }


    public void SetMaxAmmo(int ammo)
    {
        maxAmmo = ammo > 0 ? ammo : 0;
        WeaponObject.WeaponType ex = WeaponObject.WeaponType.Pistol;
        if (ex == WeaponObject.WeaponType.Pistol)
        {

        }
    }

    public void SetCurrentAmmo(int ammo)
    {
        currentAmmo = ammo > maxAmmo ? maxAmmo : ammo;
        if (currentAmmo < 0) currentAmmo = 0;
    }

    public void TransferAmmo(int ammo, FirearmItem firearm)
    {
        firearm.SetCurrentAmmo(ammo);
    }
}
