using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotion : MonoBehaviour
{
    [Header("Script References")]
    public BulletTarget bulletTarget;
    public GridManager gridManager;
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
    public int curShotsInWeapon;
    //private int curDamage;
    public GameObject targettedEnemy;
    public EnemyBehaviour targettedEnemyBehaviour;

    public float enemyDistance;

    public List<GameObject> areaTargettedEnemies;
    public List<EnemyBehaviour> areaEnemyBehaviours;
    public GameObject areaTargetBox;
    public GameObject meleeTargetBox;

    private float finesse;
    private float strength;

    // Start is called before the first frame update
    void Start()
    {
        curSpeed = walkSpeed;
        curHealth = characterStats.health;
        curShotsInWeapon = weaponList.weapons[equippedWeapon].shotsPerReload;
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

        //the way these character stats are accessed is really ineffecient, when we develop stats more we need to go back and improve how stats are accessed so it isn't in the Update all the time.
        finesse = characterStats.curFinesse;
        strength = characterStats.curStrength;

        if(weaponList.weapons[equippedWeapon].specialEffects[0] == "AutoTargeting") // handles attack hit and reload logic for autotargetting weapons
        {
            print(weaponList.weapons[equippedWeapon].baseWeaponAccuracy + ((weaponList.weapons[equippedWeapon].baseWeaponAccuracy * .1) * finesse));
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
                if (attackTimer >= attackDelay && curShotsInWeapon > 0)
                {
                    float hitRoll = Random.Range(0, 100);
                    if (hitRoll <= weaponList.weapons[equippedWeapon].baseWeaponAccuracy + ((weaponList.weapons[equippedWeapon].baseWeaponAccuracy * .1) * finesse))
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
                    curShotsInWeapon--;
                    attackTimer = 0;
                }

            }
        }

        if(weaponList.weapons[equippedWeapon].specialEffects[0] == "AreaTargeting") // handles attack hit and reload logic for area targetting weapons
        {
            areaTargetBox.SetActive(true);
            if(areaTargettedEnemies.Count > 0)
            {
                attackTimer += Time.deltaTime;
                if(attackTimer >= attackDelay && curShotsInWeapon > 0)
                {
                    for(int i = 0; i < areaTargettedEnemies.Count; i ++)
                    {
                        float hitRoll = Random.Range(0, 100);
                        if(hitRoll <= weaponList.weapons[equippedWeapon].baseWeaponAccuracy + ((weaponList.weapons[equippedWeapon].baseWeaponAccuracy * .1) * finesse))
                        {
                            float damageDealt = weaponList.weapons[equippedWeapon].damagePerShot + ((weaponList.weapons[equippedWeapon].damagePerShot * .1f) * strength);
                            AreaAttackEnemy(damageDealt, areaEnemyBehaviours[i]);
                            print(damageDealt);
                        }
                    }
                    curShotsInWeapon--;
                    attackTimer = 0;
                }
            }
        }
        else
        {
            areaTargetBox.SetActive(false);
        }

        if (weaponList.weapons[equippedWeapon].specialEffects[0] == "MeleeAreaTargeting") // handles attack hit and reload logic for area targetting weapons
        {
            meleeTargetBox.SetActive(true);
            if (areaTargettedEnemies.Count > 0)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackDelay && curShotsInWeapon > 0)
                {
                    for (int i = 0; i < areaTargettedEnemies.Count; i++)
                    {
                        float hitRoll = Random.Range(0, 100);
                        if (hitRoll <= weaponList.weapons[equippedWeapon].baseWeaponAccuracy + ((weaponList.weapons[equippedWeapon].baseWeaponAccuracy * .1) * finesse))
                        {
                            float damageDealt = weaponList.weapons[equippedWeapon].damagePerShot + ((weaponList.weapons[equippedWeapon].damagePerShot * .1f) * strength);
                            AreaAttackEnemy(damageDealt, areaEnemyBehaviours[i]);
                            print(damageDealt);
                        }
                    }
                    curShotsInWeapon--;
                    attackTimer = 0;
                }
            }
        }
        else
        {
            meleeTargetBox.SetActive(false);
        }

        if (curShotsInWeapon <= 0) // preforms reload logic when curShots runs out, for melee weapons this value will represent durability (unless we choose not to use durability)
        {
            if (gridManager.foundedItem == null)
            {
                gridManager.StartGridTraversal(weaponList.weapons[equippedWeapon].ammoType);
            }

            if (gridManager.foundedItem != null && gridManager.foundedItem.name == weaponList.weapons[equippedWeapon].ammoType)
            {
                curShotsInWeapon = weaponList.weapons[equippedWeapon].shotsPerReload;
                gridManager.DestoryGridItem(gridManager.foundedItemCoordinates);
                gridManager.foundedItem = null;
            }
        }


        if (curHealth <= 0)
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

    public void AttackEnemy(float curDamage)
    {
        targettedEnemyBehaviour.AffectHealth(curDamage);
    }

    public void AreaAttackEnemy(float curDamage, EnemyBehaviour areaTargetEnemy)
    {
        areaTargetEnemy.AffectHealth(curDamage);
    }

    public void AssignTarget(Vector3 newTargetPosition)
    {
        bulletTarget.AssignTarget(newTargetPosition);
        lineRenderer.SetPosition(1, newTargetPosition);
    }
}
