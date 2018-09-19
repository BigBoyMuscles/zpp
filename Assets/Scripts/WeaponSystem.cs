using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour {

    private Vector2 playerPosition;
    private float maxRange = 15f;
    public LayerMask targetMask;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        playerPosition = transform.position;
    }

    private void handleInput(Vector2 aimDirection)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(playerPosition, aimDirection, maxRange, targetMask);

            if (hit.collider != null)
            {
                // Hnadle hitscat weapons here.
                Debug.Log("Shots fired!");
            }
        }
    }
}
