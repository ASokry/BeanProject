using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public CharacterMotion characterMotion;
    public EnemyBehaviour enemyBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DeathAnimComplete()
    {
        if(characterMotion!= null)
        {
            characterMotion.DeathAnimComplete();
        }

        if(enemyBehaviour != null)
        {
            enemyBehaviour.DeathAnimComplete();
        }
    }
}
