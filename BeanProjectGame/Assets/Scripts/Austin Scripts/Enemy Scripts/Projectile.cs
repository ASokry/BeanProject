using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool mayMoveForward;
    public float speed;
    public int damage;
    public float maxHealth;
    public float curHealth;
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
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (mayMoveForward)
        {
            transform.Translate(-transform.right * speed * Time.deltaTime);
        }

        if(curHealth <= 0)
        {
            characterMotion.areaProjectiles.Remove(gameObject);
            Destroy(gameObject);
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
            print("Damage");
            characterMotion.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void DamageProjectile(float damage)
    {
        curHealth -= damage;
    }
}
