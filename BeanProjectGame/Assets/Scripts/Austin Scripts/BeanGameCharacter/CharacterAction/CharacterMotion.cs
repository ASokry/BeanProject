using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotion : MonoBehaviour
{
    [Header("Script References")]
    public EnemyManager enemyManager;
    public WeaponsList weaponList;
    public CharacterStats characterStats;

    [Header("Character Speed")]
    private float curSpeed;
    public float walkSpeed = 3;

    [Header("Character Health")]
    public int curHealth;

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

    // Start is called before the first frame update
    void Start()
    {
        curSpeed = walkSpeed;
        curHealth = characterStats.health;
    }

    // Update is called once per frame
    void Update()
    {
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
            float[] curEnemyDistance = new float[enemyManager.enemies.Count];
            //print("AutoTargeting");
            for (int i = 0; i < enemyManager.enemies.Count; i++)
            {
             
                curEnemyDistance[i]  = Vector3.Distance(transform.position, enemyManager.enemies[i].transform.position);

               if(curEnemyDistance[i] <= enemyDistance || enemyDistance == 0 )
               {
                    targettedEnemy = enemyManager.enemies[i];
                    enemyDistance = curEnemyDistance[i];
                    //print("working");
               }
            }
        }

        if(weaponList.weapons[equippedWeapon].specialEffects[0] == "AreaTargeting")
        {

        }
        

        attackDelay = weaponList.weapons[equippedWeapon].timeBetweenAttacks;

        if(targettedEnemy != null)
        {
            targettedEnemyBehaviour = targettedEnemy.GetComponent<EnemyBehaviour>();
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackDelay)
            {
                float hitRoll = Random.Range(0, 100);
                if(hitRoll <= weaponList.weapons[equippedWeapon].baseWeaponAccuracy)
                {
                    AttackEnemy(weaponList.weapons[equippedWeapon].damagePerShot);
                }

                attackTimer = 0;
            }

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
}
