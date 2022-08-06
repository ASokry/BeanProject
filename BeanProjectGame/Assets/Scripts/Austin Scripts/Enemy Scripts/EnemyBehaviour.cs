using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header ("Object References")]
    public GameObject player;
    public GameObject levelManager;
    public GameObject nearLeftEnemy;
    public GameObject nearRightEnemy;

    [Header ("Script References")]
    public EnemyManager enemyManager;
    public EnemyStats enemyStats;

    [Header ("Stats and States")]
    public int curEnemyHealth;
    public float curEnemySpeed;
    public bool stopped;
    public float preferredDistanceFromEnemies;
    public TextMesh textMesh;

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

        if(curEnemyHealth <= 0)
        {
            Death();
        }

  
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

        if (curEnemyHealth <= 0)
        {
            Death();
        }

    }

    public void Attack(int damage)
    {
        characterMotion.TakeDamage(damage);
    }

    public void AffectHealth(int damage)
    {
        curEnemyHealth -= damage;
    }

    private void Death()
    {
        enemyManager.enemies.Remove(gameObject);
        Destroy(gameObject);
    }
}
