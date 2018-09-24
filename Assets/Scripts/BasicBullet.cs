using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour {
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public GameObject trailEffect;
    public float speed = 4;

	// Use this for initialization
	void Start () {
        rb.velocity = -transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
