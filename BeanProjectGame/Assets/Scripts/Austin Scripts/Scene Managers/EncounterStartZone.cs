using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterStartZone : MonoBehaviour
{
    public CharacterMotion characterMotion;
    public Transform encounterEnd;
    public GameObject camera;
    public Transform cameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        camera.transform.position = cameraPosition.position;
        if(other.tag == "Player")
        {
            characterMotion.encounterStart = transform;
            characterMotion.encounterEnd = this.encounterEnd;
        }
    }
}
