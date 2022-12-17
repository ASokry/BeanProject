﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopTrigger : MonoBehaviour
{
    public string waitType = "defeatEnemies";
    public float waitTime;
    public GameObject player;
    public CharacterMotion characterMotion;
    public bool debugMode;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterMotion = player.GetComponent<CharacterMotion>();
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
        if(other.tag == "Player")
        {
            characterMotion.Stop(waitType, waitTime);
        }
    }
}
