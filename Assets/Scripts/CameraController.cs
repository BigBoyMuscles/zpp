using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float dampTime;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private Camera cam;


    private void Awake()
    {
        cam = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        updateCameraPosition();
    }

    void updateCameraPosition()
    {
        if (target)
        {
            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}