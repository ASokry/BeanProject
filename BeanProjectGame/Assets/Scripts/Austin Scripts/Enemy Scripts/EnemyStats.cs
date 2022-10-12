using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stat", menuName = "Enemy Stats / New EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float enemyHealth;
    public float enemySpeed;
    public float closeRange;
    public float midRange;
    public float farRange;
    public float prefDistanceBetweenEnemies;
    public float prefDistanceToPlayer;
    public EnemyAttack[] enemyAttacks;

    [System.Serializable]
    public class EnemyAttack
    {
        public string attackRange;
        public string attackType;
        public int damage;
        public float timeBetweenAttacks;
        public float attackAccuracy;
        public GameObject projectile;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /* [CreateAssetMenu(fileName = "New Enemy Stat", menuName = "Enemy Stats / New EnemyStats")]
    public class EnemyStatsData : EnemyStats { }*/
}
