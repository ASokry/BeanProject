using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Core Attribute : Health")]
    public int health;
    public int curHealth;
    [Header("Core Attribute : Speed")]
    public int speed = 1;
    public int curSpeed;
   [Header("Core Attribute : Luck")]
    public int luck = 1;
    public int curLuck;
   [Header("Core Attribute : Strength")]
    public int strength = 1;
    public int curStrength;
   [Header("Core Attribute : Finesse")]
    public int finesse = 1;
    public int curFinesse;
   [Header("Core Attribute : Coordination")]
    public int coordination = 1;
    public int curCoordination;
   [Header("Core Attribute : Adaptability")]
    public int adaptability = 1;
    public int curAdaptability;
   [Header("Core Attribute : Resistance")]
    public int resistance = 1;
    public int curResistance;
    //Start is called before the first frame update
    void Start()
    {
        //Just sets stats to base stats for now, in the future we'll use a function to set stats according to inventory items.
        //curStats are the base stats plus modifications, base stats are improved through events and such, they're the permanent base stats, whereas curStats are effected by items and equipment.
        curHealth = health;
        curSpeed = speed;
        curLuck = luck;
        curStrength = strength;
        curFinesse = finesse;
        curCoordination = coordination;
        curAdaptability = adaptability;
        curResistance = resistance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
