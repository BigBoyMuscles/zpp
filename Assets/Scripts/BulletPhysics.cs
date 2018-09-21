using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviour {

    public float maxSpeed = 4f;
    public float rampUp = .03f;

    private Rigidbody2D rBody;

    private Vector2 angle; 

	// Use this for initialization
	void Awake () {
        rBody = GetComponent<Rigidbody2D>();		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
