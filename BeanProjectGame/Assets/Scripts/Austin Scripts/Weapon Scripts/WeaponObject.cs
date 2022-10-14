using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : ItemObject
{
    public enum WeaponType {Pistol, Shotgun, Melee, None};
    public enum AimType {AutoTargeting, AreaTargeting, MeleeAreaTargeting, None};

    public WeaponType weaponType = WeaponType.None;
    public AimType aimType = AimType.None;
    public float damagePerShot;
    public string ammoType;
    public int clipSize;
    public int curAmmo;
    public float timeBetweenAttacks;
    public float baseWeaponAccuracy;
    public string[] specialEffects;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetCurAmmo(int ammoModifier)
    {
        curAmmo = curAmmo + ammoModifier;
        if(curAmmo > clipSize)
        {
            curAmmo = clipSize;
        }
        if(curAmmo < 0)
        {
            curAmmo = 0;
        }
    }
}
