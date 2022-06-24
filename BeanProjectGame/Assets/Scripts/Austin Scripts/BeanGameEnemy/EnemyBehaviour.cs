using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header ("Script References")]
    public GameObject player;
    public GameObject levelManager;
    public EnemyManager enemyManager;
    public EnemyStats enemyStats;

    [Header ("Stats and States")]
    public int curEnemyHealth;
    public float curEnemySpeed;
    public bool stopped;
    public float[] distancesFromEnemies;
    public TextMesh textMesh;

    [Header ("Attack Ranges")]
    public float closeRange;
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
        enemyManager.enemies.Add(gameObject);

        curEnemyHealth = enemyStats.enemyHealth;
        curEnemySpeed = enemyStats.enemySpeed;
        closeRange = enemyStats.closeRange;
        midRange = enemyStats.midRange;
        farRange = enemyStats.farRange;

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

    // Update is called once per frame
    void Update()
    {

        textMesh.text = curEnemyHealth.ToString() + "/" + enemyStats.enemyHealth.ToString();
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer =  Vector3.Distance(transform.position, player.transform.position);


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

        distancesFromEnemies = new float[enemyManager.enemies.Count];



        if (curEnemyHealth <= 0)
        {
            Death();
        }

        for(int i = 0; i < enemyManager.enemies.Count; i++)
        {
            distancesFromEnemies[i] = Vector3.Distance(transform.position, enemyManager.enemies[i].transform.position);

            if (enemyManager.enemies[i] != this)
            {
                print(distancesFromEnemies[i]);
                if (distancesFromEnemies[i] < enemyStats.prefDistanceBetweenEnemies)
                {
                    stopped = true;
                    //transform.Translate((transform.position - enemyManager.enemies[i].transform.position) * curEnemySpeed * Time.deltaTime);
                }
                else
                {
                    stopped = false;
                }
            }
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
