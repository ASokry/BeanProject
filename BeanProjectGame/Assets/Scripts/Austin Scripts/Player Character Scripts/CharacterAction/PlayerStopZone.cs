using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopZone : MonoBehaviour
{
    public CharacterMotion characterMotion;
    public bool debug;
    // if an enemy enters the stop zone, the player must stop
    // Start is called before the first frame update
    void Start()
    {
        if(debug == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
        }
    }
    private void OnTriggerStay(Collider other)
    {
            if (other.tag == "Enemy")
            {
                characterMotion.SetStop(true);
            }
            else
            {
                characterMotion.SetStop(false);
            }

    }
}
