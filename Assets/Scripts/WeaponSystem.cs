using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour {

    private Vector2 playerPosition;
    public Transform projectileSpawn;
    public LineRenderer line;
    public PlayerMovement playerMovement;

    // Use this for initialization
    void Start () {
        playerPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {


        if (Input.GetButtonDown("Fire1"))
        {if(!playerMovement.isSwinging)
            {
                StartCoroutine(Shoot());
            }
            
        }


    }

    IEnumerator Shoot()
    {
        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);

        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        playerPosition = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(projectileSpawn.position, aimDirection);
        if (hit)
        {
            Debug.Log(hit.transform.name);
            line.SetPosition(0, projectileSpawn.position);
            line.SetPosition(1, hit.point);
            
        }

        line.enabled = true;
        yield return new WaitForSeconds(0.2f);
        line.enabled = false;


    }
}
