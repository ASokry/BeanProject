using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject>enemies;
    public List<GameObject> disabledEnemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject enemy in enemies)
        {
            if(!enemy.activeInHierarchy)
            {
                enemies.Remove(enemy);
                disabledEnemies.Add(enemy);
            }
        }
        foreach(GameObject disabledEnemy in disabledEnemies)
        {
            if (disabledEnemy.activeInHierarchy)
            {
                disabledEnemies.Remove(disabledEnemy);
                enemies.Add(disabledEnemy);
            }
        }
    }
}
