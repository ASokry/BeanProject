using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool mayMoveForward;
    public float speed;
    public int damage;
    private CharacterMotion characterMotion;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterMotion = player.GetComponent<CharacterMotion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mayMoveForward)
        {
            transform.Translate(-transform.right * speed * Time.deltaTime);
        }

    }

    public void BugEmerged()
    {
        mayMoveForward = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print("Dammage");
            characterMotion.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
