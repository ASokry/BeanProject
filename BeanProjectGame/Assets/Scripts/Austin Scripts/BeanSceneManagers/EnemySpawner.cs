using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public EnemyType[] enemyTypes;
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyType;
        public int minnimumAmount;
        public int maximumAmount;
    }
    public bool debugMode;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if(debugMode == false)
        {
            meshRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            for(int i = 0; i < enemyTypes.Length; i++)
            {
                int spawnAmount = Random.Range(enemyTypes[i].minnimumAmount, enemyTypes[i].maximumAmount+1);

                //int chosenSpawnLocation = Random.Range(0, spawnAmount);

                //Instantiate(enemyTypes[i].enemyType, spawnPoints[chosenSpawnLocation].transform);

                for (int e = 0; e < spawnAmount; e++)
                {
                    int chosenSpawnLocation = Random.Range(0, spawnPoints.Length);
                    float pointRandomizer = Random.Range(1, 2);
                    Vector3 randomPoint = new Vector3(spawnPoints[chosenSpawnLocation].transform.position.x + pointRandomizer, spawnPoints[chosenSpawnLocation].transform.position.y, spawnPoints[chosenSpawnLocation].transform.position.z);
                    Instantiate(enemyTypes[i].enemyType, randomPoint, Quaternion.identity);
                }
            }
        }
    }
}
