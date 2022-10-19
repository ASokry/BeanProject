using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : ItemObject
{
    public enum WeaponType {Pistol, Shotgun, Specialty, Melee, None};

    public WeaponType weaponType = WeaponType.None;
    public string AmmoType;
    public int MaxAmmo;
    public int curAmmo;


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
        if(curAmmo > MaxAmmo)
        {
            curAmmo = MaxAmmo;
        }
        if(curAmmo < 0)
        {
            curAmmo = 0;
        }
    }
}
