using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Object References")]
    public GameObject player;
    public GameObject levelManager;
    public GameObject nearLeftEnemy;
    public GameObject nearRightEnemy;
    private Vector3 previousPosition;
    public Transform projectileInstatiator;

    [Header("Script References")]
    public EnemyManager enemyManager;
    public EnemyStats enemyStats;
    public CharacterAnimationManager characterAnimationManager;

    [Header("Stats and States")]
    public float curEnemyHealth;
    public float curEnemySpeed;
    public bool stopped;
    public float preferredDistanceFromEnemies;
    public TextMesh textMesh;
    public float knockBackSpeed;
    public float knockbackDistance;
    private bool beingKnockedBack;
    private Vector3 knockBackStartPos;
    private bool isDead;
    [SerializeField] private bool debugMode;

    [Header ("Attack Ranges")]
    private float closeRange;
    private float midRange;
    private float farRange;
    private float attackTimer;
    public EnemyStats.EnemyAttack[] enemyAttacks;
    private CharacterMotion characterMotion;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterMotion = player.GetComponent<CharacterMotion>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        enemyManager = levelManager.GetComponent<EnemyManager>();
        //enemyManager.enemies.Add(gameObject);

        curEnemyHealth = enemyStats.enemyHealth;
        curEnemySpeed = enemyStats.enemySpeed;
        closeRange = enemyStats.closeRange;
        midRange = enemyStats.midRange;
        farRange = enemyStats.farRange;
        preferredDistanceFromEnemies = enemyStats.prefDistanceBetweenEnemies;

        enemyAttacks = new EnemyStats.EnemyAttack[enemyStats.enemyAttacks.Length];

        for(int i = 0; i < enemyAttacks.Length; i++)
        {
            enemyAttacks[i] = enemyStats.enemyAttacks[i];
        }

        /*if(curEnemyHealth <= 0)
        {
            Death();
        }*/
        if(debugMode == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        isDead = false;
    }
    private void Awake()
    {
        enemyManager.enemies.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach(GameObject enemy in enemyManager.enemies)
        {
            if(enemy != gameObject)
            {
                enemyManager.enemies.Add(gameObject);
            }
        }*/
        if(characterAnimationManager != null)
        {
            characterAnimationManager.characterVelocity = (transform.position - previousPosition).magnitude / Time.deltaTime;
            previousPosition = transform.position;
        }

        textMesh.text = curEnemyHealth.ToString() + "/" + enemyStats.enemyHealth.ToString();
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer =  Vector3.Distance(transform.position, player.transform.position);

        RaycastHit leftHit;
        RaycastHit rightHit;
        Ray rayLeft = new Ray(transform.position, -transform.right);
        Ray rayRight = new Ray(transform.position, transform.right);

        if(Physics.Raycast(rayLeft, out leftHit))
        {
            if(leftHit.transform.tag == "Enemy")
            {
                nearLeftEnemy = leftHit.transform.gameObject;
            }
        }

        if(Physics.Raycast(rayRight, out rightHit))
        {
            if (rightHit.transform.tag == "Enemy")
            {
                nearRightEnemy = rightHit.transform.gameObject;
            }
        }
        if(nearLeftEnemy != null)
        {
            if (Vector3.Distance(transform.position, nearLeftEnemy.transform.position) < preferredDistanceFromEnemies)
            {
                stopped = true;
            }
            if (Vector3.Distance(transform.position, nearLeftEnemy.transform.position) > preferredDistanceFromEnemies)
            {
                stopped = false;
            }
        }

        if (nearRightEnemy != null)
        {
            if (Vector3.Distance(transform.position, nearRightEnemy.transform.position) < preferredDistanceFromEnemies)
            {
                stopped = true;
            }
            if (Vector3.Distance(transform.position, nearRightEnemy.transform.position) > preferredDistanceFromEnemies)
            {
                stopped = false;
            }
        }


        if (distanceToPlayer > enemyStats.prefDistanceToPlayer && !stopped)
        {
            transform.Translate(directionToPlayer * curEnemySpeed * Time.deltaTime);
        }

        if(distanceToPlayer <= closeRange)
        {
            for(int i = 0; i < enemyAttacks.Length; i++)
            {
                if(enemyAttacks[i].attackRange == "Close")
                {
                    attackTimer += Time.deltaTime;
                    if(attackTimer >= enemyAttacks[i].timeBetweenAttacks)
                    {
                        characterAnimationManager.Attack();
                        float hitRoll = Random.Range(0, 100);
                        if(hitRoll <= enemyAttacks[i].attackAccuracy)
                        {
                            Attack(enemyAttacks[i].damage);
                        }
                        attackTimer = 0;
                    }
                }
            }
        }

        if(distanceToPlayer <= farRange)
        {
            for(int i = 0; i < enemyAttacks.Length; i++)
            {
                if (enemyAttacks[i].attackRange == "Far")
                {
                    if(enemyAttacks[i].attackType == "Projectile")
                    {
                        attackTimer += Time.deltaTime;
                        if(attackTimer >= enemyAttacks[i].timeBetweenAttacks)
                        {
                            Instantiate(enemyAttacks[i].projectile, projectileInstatiator.position, projectileInstatiator.rotation);
                            attackTimer = 0;
                        }

                    }
                }
            }
        }

        if (curEnemyHealth <= 0 && !isDead)
        {
            Death();
            isDead = true;
        }

        if (beingKnockedBack)
        {
            if(Vector3.Distance(knockBackStartPos, transform.position) < knockbackDistance)
            {
                transform.Translate(-directionToPlayer * knockBackSpeed * Time.deltaTime);
            }
            else
            {
                stopped = false;
                beingKnockedBack = false;
            }
        }
    }

    public void Attack(int damage)
    {
        characterMotion.TakeDamage(damage);
    }

    public void AffectHealth(float damage)
    {
        curEnemyHealth -= damage;
    }

    public void Knockback()
    {
        print("Knocked Back");
        knockBackStartPos = transform.position;
        stopped = true;
        beingKnockedBack = true;
    }

    private void Death()
    {
        characterAnimationManager.DeathAnimation();
        gameObject.tag = "Untagged";
    }

    public void DeathAnimComplete()
    {
        enemyManager.enemies.Remove(gameObject);
        Destroy(gameObject);
    }
}
