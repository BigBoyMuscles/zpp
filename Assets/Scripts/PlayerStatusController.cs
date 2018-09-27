using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusController : MonoBehaviour {

    //Collider is probably not needed
    public CapsuleCollider2D playerCollider;

    public float maxHealth = 150;
    public float health;
    public float maxArmor = 0;
    public float armor;
    public float maxShield = 0;
    public float shield = 0;

    private bool dead = false;

	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            dead = true;
        }

        if (dead)
        {
            Debug.Log("Died");
        }
	}

    public void adjustHealth(float hpDif)
    {
        if(health + hpDif <= maxHealth)
        {
            health += hpDif;
        }
        
    }

}
