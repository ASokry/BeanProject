using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrePlacedEnemyActivator : MonoBehaviour
{
    public GameObject[] enemies;
    public bool debugMode;
    // Start is called before the first frame update
    void Start()
    {
        if(debugMode == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(false);
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
            foreach(GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }
        }
    }
}
