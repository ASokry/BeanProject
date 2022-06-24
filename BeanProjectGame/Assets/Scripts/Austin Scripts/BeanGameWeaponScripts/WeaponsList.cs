using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponsList : ScriptableObject
{
    public Weapon[] weapons;
}

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public string weaponType;
    public int damagePerShot;
    public int shotsPerReload;
    public float timeBetweenAttacks;
    public float baseWeaponAccuracy;
    //Unsure how we should handle special effects, storing it as strings for now, ideally we could reference the strings elsewhere.
    public string[] specialEffects;
    public Image weaponThumbnail;
    //Weapon sprite is placeholder for future functionality, let's consider what's appropriate later.
    public Sprite weaponSprite;
}

[CreateAssetMenu(fileName = "New Weapon List", menuName = "Weapon List / New Weapon List")]
public class WeaponListData : WeaponsList { }
