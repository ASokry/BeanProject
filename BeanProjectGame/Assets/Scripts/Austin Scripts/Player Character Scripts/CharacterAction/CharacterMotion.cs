﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotion : MonoBehaviour
{
    [Header("Script References")]
    public BulletTarget bulletTarget;
    public EnemyManager enemyManager;
    public WeaponsList weaponList;
    public CharacterStats characterStats;

    [Header("Character Speed")]
    private float curSpeed;
    public float walkSpeed = 3;

    [Header("Character Health")]
    public int curHealth;

    [Header("Effects")]
    public float missModifier = 0.5f;
    public LineRenderer lineRenderer;

    [Header("Combnat Logic")]

    private float timer;
    private float localWaitTime;
    private float attackTimer;
    private float attackDelay;
    private bool stopped;
    public int equippedWeapon;
    //private int curDamage;
    public GameObject targettedEnemy;
    public EnemyBehaviour targettedEnemyBehaviour;

    public float enemyDistance;

    public List<GameObject> areaTargettedEnemies;
    public List<EnemyBehaviour> areaEnemyBehaviours;
    public GameObject areaTargetBox;

    // Start is called before the first frame update
    void Start()
    {
        curSpeed = walkSpeed;
        curHealth = characterStats.health;
    }

    // Update is called once per frame
    void Update()
    {

        attackDelay = weaponList.weapons[equippedWeapon].timeBetweenAttacks;

        transform.Translate(Vector3.right * curSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        //print(timer);

        if(timer <= localWaitTime)
        {
            stopped = false;
        }

        if (enemyManager.enemies.Count <= 0)
        {
            stopped = false;
        }

        if (stopped)
        {
            curSpeed = 0;

        }
        else
        {
            curSpeed = walkSpeed;
        }

        /*if (Input.GetKey(KeyCode.Mouse0))
        {
            Ray selectRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(selectRay, out mouseHit))
            {
                if (mouseHit.transform.tag == "Enemy")
                {
                    targettedEnemy = mouseHit.transform.gameObject;
                    targettedEnemyBehaviour = targettedEnemy.GetComponent<EnemyBehaviour>();
                }
            }

        }*/   

        if(weaponList.weapons[equippedWeapon].specialEffects[0] == "AutoTargeting")
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.right, out hit))
            {
                if(hit.transform.tag == "Enemy")
                {
                    targettedEnemy = hit.transform.gameObject;
                }
            }

            if (targettedEnemy != null)
            {
                targettedEnemyBehaviour = targettedEnemy.GetComponent<EnemyBehaviour>();
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackDelay)
                {
                    float hitRoll = Random.Range(0, 100);
                    if (hitRoll <= weaponList.weapons[equippedWeapon].baseWeaponAccuracy)
                    {
                        AttackEnemy(weaponList.weapons[equippedWeapon].damagePerShot);
                        AssignTarget(targettedEnemy.transform.position);
                    }
                    else
                    {
                        float missTargetYModifier = targettedEnemy.transform.position.y + hitRoll * missModifier;
                        Vector3 missTarget = new Vector3(targettedEnemy.transform.position.x, missTargetYModifier, targettedEnemy.transform.position.z);
                        AssignTarget(missTarget);
                    }

                    attackTimer = 0;
                }

            }

        }

        if(weaponList.weapons[equippedWeapon].specialEffects[0] == "AreaTargeting")
        {
            areaTargetBox.SetActive(true);
            if(areaTargettedEnemies.Count > 0)
            {
                attackTimer += Time.deltaTime;
                if(attackTimer >= attackDelay)
                {
                    for(int i = 0; i < areaTargettedEnemies.Count; i ++)
                    {
                        float hitRoll = Random.Range(0, 100);
                        if(hitRoll <= weaponList.weapons[equippedWeapon].baseWeaponAccuracy)
                        {
                            AreaAttackEnemy(weaponList.weapons[equippedWeapon].damagePerShot, areaEnemyBehaviours[i]);
                        }
                    }

                    attackTimer = 0;
                }
            }
        }
        else
        {
            areaTargetBox.SetActive(false);
        }
        



        if(curHealth <= 0)
        {
            PlayerDeath();
        }

    }

    public void PlayerDeath()
    {
        print("dead");
    }
    public void TakeDamage(int damage)
    {
        curHealth -= damage;
    }
    public void Stop(string waitType, float waitTime)
    {
        curSpeed = 0;
        if(waitType == "defeatEnemies")
        {
            stopped = true;
        }
        if(waitType == "stopForTime")
        {
            timer = 0;
            localWaitTime = waitTime;
            stopped = true;
        }
    }

    public void AttackEnemy(int curDamage)
    {
        targettedEnemyBehaviour.AffectHealth(curDamage);
    }

    public void AreaAttackEnemy(int curDamage, EnemyBehaviour areaTargetEnemy)
    {
        areaTargetEnemy.AffectHealth(curDamage);
    }

    public void AssignTarget(Vector3 newTargetPosition)
    {
        bulletTarget.AssignTarget(newTargetPosition);
        lineRenderer.SetPosition(1, newTargetPosition);
    }
}
