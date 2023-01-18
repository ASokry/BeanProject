using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotion : MonoBehaviour
{
    [Header("Script References")]
    public BulletTarget bulletTarget;
    //public GridManager gridManager;
    public EnemyManager enemyManager;
    //public WeaponsList weaponList;
    public CharacterStats characterStats;
    public CharacterAnimationManager characterAnimationManager;

    [Header("Component References")]
    public Rigidbody playerRigidbody;
    private Vector3 previousPosition;

    [Header("Character Stats")]
    public UIBar healthBarUI;

    [Header("Character Stats")]
    public int curHealth;
    public float walkSpeed = 3;
    private float curSpeed;
    public bool isDead;

    [Header("Visual Effects")]
    public float missModifier = 0.5f; //The factor that's multiplied against the difference between the check for an accurate shot and the actual roll, basically determines how severe a missed shot will appear to miss by.
    public LineRenderer lineRenderer; //The line renderer currently used for bullet trail rendering, may be switched out later.

    [Header("Combnat Logic")]

    private float timer;
    private float localWaitTime;
    private float attackTimer;
    private float attackDelay;
    private bool stopped;
    public InventoryWeapon weaponObject;
    public ConsumableObject consumableObject;
    //private int curDamage;
    public GameObject targettedEnemy;
    public EnemyBehaviour targettedEnemyBehaviour;

    public float enemyDistance;

    [HideInInspector] public List<GameObject> areaTargettedEnemies;
    [HideInInspector] public List<EnemyBehaviour> areaEnemyBehaviours;
    public List<GameObject> areaProjectiles;
    public GameObject areaTargetPivot;
    public GameObject areaTargetBox;
    public GameObject meleeTargetBox;

    private float finesse;
    private float strength;

    [SerializeField]private bool debugMode;

    // Start is called before the first frame update
    void Start()
    {
        curSpeed = walkSpeed;
        curHealth = characterStats.health;
        isDead = false;

        if (!debugMode)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        //healthBarUI.SetMaxHealth(curHealth);//Health UI by Anthony
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(this.transform.position, transform.right, color: Color.red);
        transform.Translate(Vector3.right * curSpeed * Time.deltaTime);

        characterAnimationManager.characterVelocity = (transform.position - previousPosition).magnitude / Time.deltaTime;
        previousPosition = transform.position;

        RaycastHit distanceHit;
        if (Physics.Raycast(transform.position, transform.right, out distanceHit, .5f))
        {
            if (distanceHit.transform.tag == "Enemy")
            {
                SetStop(true);
            }
        }

        //print(playerRigidbody.velocity.magnitude);

        //timer += Time.deltaTime;
        //print(timer);


        /*if(timer <= localWaitTime)
        {
            stopped = false;
        }

        /*if (enemyManager.enemies.Count <= 0)
        {
            stopped = false;
        }*/

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
        if(weaponObject != null /*&& stopped*/)
        {
            attackDelay = weaponObject.timeBetweenAttacks;

            if (weaponObject.aimType == InventoryWeapon.AimType.AutoTargeting) // handles attack hit and reload logic for autotargetting weapons
            {

                //print(weaponList.weapons[equippedWeapon].baseWeaponAccuracy + ((weaponList.weapons[equippedWeapon].baseWeaponAccuracy * .1) * finesse));
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.right, out hit))
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        targettedEnemy = hit.transform.gameObject;
                    }
                    if(hit.transform.tag == "Projectile")
                    {
                        targettedEnemy = hit.transform.gameObject;
                    }
                }

                if (targettedEnemy != null && Vector3.Distance(transform.position, targettedEnemy.transform.position) <= weaponObject.range)
                {
                    SetStop(true);
                    if(targettedEnemy.tag == "Enemy")
                    {
                        targettedEnemyBehaviour = targettedEnemy.GetComponent<EnemyBehaviour>();
                    }

                    attackTimer += Time.deltaTime;
                    if (attackTimer >= attackDelay && weaponObject.curAmmo > 0)
                    {
                        characterAnimationManager.Attack();
                        float hitRoll = Random.Range(0, 100);
                        if (hitRoll <= weaponObject.baseWeaponAccuracy + ((weaponObject.baseWeaponAccuracy * .1) * finesse))
                        {
                            if(targettedEnemyBehaviour != null)
                            {
                                AttackEnemy(weaponObject.damagePerShot);
                                AssignTarget(targettedEnemy.transform.position);
                                for (int e = 0; e < weaponObject.specialEffects.Length; e++)
                                {
                                    CallWeaponEffect(weaponObject.specialEffects[e], targettedEnemyBehaviour);
                                }
                            }
                            else
                            {
                                targettedEnemy.SendMessage("DamageProjectile", weaponObject.damagePerShot);
                                AssignTarget(targettedEnemy.transform.position);
                            }
                        }
                        else
                        {
                            float missTargetYModifier = targettedEnemy.transform.position.y + hitRoll * missModifier;
                            Vector3 missTarget = new Vector3(targettedEnemy.transform.position.x, missTargetYModifier, targettedEnemy.transform.position.z);
                            AssignTarget(missTarget);
                        }
                        weaponObject.SetCurAmmo(-1);
                        attackTimer = 0;
                    }

                }
                if(Vector3.Distance(transform.position, targettedEnemy.transform.position) >= weaponObject.range)
                {
                    //SetStop(false);
                }
            }

            if (weaponObject.aimType == InventoryWeapon.AimType.AreaTargeting) // handles attack hit and reload logic for area targetting weapons
            {
                areaTargetBox.SetActive(true);
                areaTargetPivot.gameObject.transform.localScale = new Vector3(weaponObject.range, areaTargetBox.transform.localScale.y, areaTargetBox.transform.localScale.z);
                if (areaTargettedEnemies.Count > 0 || areaProjectiles.Count > 0)
                {
                    //SetStop(true);
                    attackTimer += Time.deltaTime;
                    if (attackTimer >= attackDelay && weaponObject.curAmmo > 0)
                    {
                        if(areaProjectiles.Count > 0)
                        {
                            foreach(GameObject projectile in areaProjectiles)
                            {
                                float hitRoll = Random.Range(0, 100);
                                if (hitRoll <= weaponObject.baseWeaponAccuracy + ((weaponObject.baseWeaponAccuracy * .1) * finesse))
                                {
                                    projectile.SendMessage("DamageProjectile", weaponObject.damagePerShot + ((weaponObject.damagePerShot * .1f) * strength));
                                }
                            }
                        }
                        /*if(Vector3.Distance(transform.position, targettedEnemy.transform.position) <= weaponObject.range)
                        {

                        }*/
                        characterAnimationManager.Attack();
                        for (int i = 0; i < areaTargettedEnemies.Count; i++)
                        {
                            float hitRoll = Random.Range(0, 100);
                            if (hitRoll <= weaponObject.baseWeaponAccuracy + ((weaponObject.baseWeaponAccuracy * .1) * finesse))
                            {
                                float damageDealt = weaponObject.damagePerShot + ((weaponObject.damagePerShot * .1f) * strength);
                                AreaAttackEnemy(damageDealt, areaEnemyBehaviours[i]);
                                for(int e = 0; e < weaponObject.specialEffects.Length; e++)
                                {
                                    CallWeaponEffect(weaponObject.specialEffects[e], areaEnemyBehaviours[i]);      
                                }
                                //print(damageDealt);
                            }
                        }
                        weaponObject.SetCurAmmo(-1);
                        attackTimer = 0;
                    }
                }
            }
            else
            {
                areaTargetBox.SetActive(false);
                //SetStop(false);
            }

            /*if (weaponObject.aimType == InventoryWeapon.AimType.MeleeAreaTargeting) // handles attack hit and reload logic for area targetting weapons
            {
                meleeTargetBox.SetActive(true);
                if (areaTargettedEnemies.Count > 0)
                {
                    //SetStop(true);
                    attackTimer += Time.deltaTime;
                    if (attackTimer >= attackDelay && weaponObject.curAmmo > 0)
                    {
                        for (int i = 0; i < areaTargettedEnemies.Count; i++)
                        {
                            float hitRoll = Random.Range(0, 100);
                            if (hitRoll <= weaponObject.baseWeaponAccuracy + ((weaponObject.baseWeaponAccuracy * .1) * finesse))
                            {
                                float damageDealt = weaponObject.damagePerShot + ((weaponObject.damagePerShot * .1f) * strength);
                                AreaAttackEnemy(damageDealt, areaEnemyBehaviours[i]);
                                //print(damageDealt);
                                for (int e = 0; e < weaponObject.specialEffects.Length; e++)
                                {
                                    CallWeaponEffect(weaponObject.specialEffects[e], areaEnemyBehaviours[i]);
                                }
                            }
                        }
                        weaponObject.SetCurAmmo(-1);
                        attackTimer = 0;
                    }
                }
            }
            else
            {
                meleeTargetBox.SetActive(false);
               // SetStop(false);
            }*/

            if (weaponObject.curAmmo <= 0) // preforms reload logic when curShots runs out, for melee weapons this value will represent durability (unless we choose not to use durability)
            {
                if (InventorySearchSystem.Instance.foundItem == null)
                {
                    InventorySearchSystem.Instance.StartGridSearch(weaponObject.ammoType);
                }

                if (InventorySearchSystem.Instance.foundItem != null && InventorySearchSystem.Instance.foundItem.GetName() == weaponObject.ammoType)
                {
                    weaponObject.SetCurAmmo(weaponObject.clipSize);
                    //gridManager.DestoryGridItem(gridManager.foundedItemCoordinates);//Anthony
                    InventorySearchSystem.Instance.DestroyFoundItem();
                    InventorySearchSystem.Instance.ResetSearchSystem();
                }
            }
        }

        if(consumableObject != null) // if the script has a consumable object, imediately preform its effect and then set it to null and delete the object from the grid
        {
            InventorySearchSystem.Instance.StartGridSearch(consumableObject.name);

            if(InventorySearchSystem.Instance.foundItem != null && InventorySearchSystem.Instance.foundItem.GetName() == consumableObject.name)
            {
                if (consumableObject.consumableType == ConsumableObject.ConsumableType.Healing)
                {
                    if (curHealth < characterStats.health)
                    {
                        curHealth += consumableObject.healAmount;
                        //gridManager.DestoryGridItem(gridManager.foundedItemCoordinates);//Anthony
                        InventorySearchSystem.Instance.ResetSearchSystem();
                        consumableObject = null;
                    }
                }
            }

        }
       


        if (curHealth <= 0 && !isDead)
        {
            PlayerDeath();
            isDead = true;
        }

    }

    public void CallWeaponEffect(string effectName, EnemyBehaviour enemyBehaviour)
    {
        if(effectName == "Knockback")
        {
            enemyBehaviour.Knockback();
        }
    }

    public void PlayerDeath()
    {
        stopped = true;
        characterAnimationManager.DeathAnimation();
    }

    public void DeathAnimComplete()
    {
        
        print("dead");
    }
    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        //healthBarUI.SetHealth(curHealth);
    }
    public void Stop(string waitType, float waitTime)
    {
        curSpeed = 0;
        if(waitType == "defeatEnemies")
        {
            stopped = true;
        }
        else if(waitType == "stopForTime")
        {
            timer = 0;
            localWaitTime = waitTime;
            stopped = true;
        }
    }
    public void SetStop(bool isStopped)
    {
        stopped = isStopped;
    }
    public void WalkToggle()
    {
        if (!stopped)
        {
            stopped = true;
        }
        else
        {
            stopped = false;
        }
    }

    public void AttackEnemy(float curDamage)
    {
        targettedEnemyBehaviour.AffectHealth(curDamage);
    }

    public void AttackProjectile(float curDamage)
    {

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

    public void SetWeaponObject(InventoryWeapon weaponObject)
    {
        this.weaponObject = weaponObject;
    }

    public void SetConsumableObject(ConsumableObject consumableObject)
    {
        this.consumableObject = consumableObject;
    }
}
