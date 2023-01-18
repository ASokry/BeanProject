using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTargetting : MonoBehaviour
{
    public CharacterMotion characterMotion;
    public bool debugMode;
    // Start is called before the first frame update
    void Start()
    {
        if(debugMode == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            characterMotion.areaTargettedEnemies.Add(other.gameObject);
            characterMotion.areaEnemyBehaviours.Add(other.gameObject.GetComponent<EnemyBehaviour>());
        }

        if(other.tag == "Projectile")
        {
            characterMotion.areaProjectiles.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Enemy")
        {
            characterMotion.areaTargettedEnemies.Remove(other.gameObject);
            characterMotion.areaEnemyBehaviours.Remove(other.gameObject.GetComponent<EnemyBehaviour>());
        }

        if(other.tag == "Projectile")
        {
            characterMotion.areaProjectiles.Remove(other.gameObject);
        }
    }
}
