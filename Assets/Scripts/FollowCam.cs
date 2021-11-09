using UnityEngine;

public class FollowCam : MonoBehaviour
{
    #region Components

    public GameObject target;
    private Camera _camera;
    public Vector3 minXY;
    private Vector3 defaultPosition;

    #endregion

    public float camZ, easing = 0.05f;

    private void Awake()
    {
        camZ = transform.position.z;
        defaultPosition = transform.position;
        _camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Vector3 destination;
        
        if (!target)
        {
            destination = defaultPosition;
        }
        else
        {
            // Get the position of the target
            destination = target.transform.position;
            
            // if (target.tag == "Projectile")
            // {
            //     // if it is sleeping (that is, not moving)
            //     if (target.GetComponent<Rigidbody>().IsSleeping())
            //     {
            //         // return to default view
            //         target = null;
            //         // in the next update
            //         return;
            //     }
            // }
        }
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        
        transform.position = destination;
        _camera.orthographicSize = destination.y + 10;
    }
}
