using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class Slingshot : MonoBehaviour
{
    public GameObject birdPrefab;
    public GameObject launchPoint, bird;
    public bool aimingMode;
    public Vector3 launchPos;
    public float velocityMult;
    static public Slingshot S;

    private void Start()
    {
        var launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
        S = this;
    }

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        launchPoint.SetActive(true);
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        // Instantiate a bird
        bird = Instantiate(birdPrefab);
        // Start it at the launchPoint
        bird.transform.position = launchPos;
        // Set it to isKinematic for now
        bird.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode) return;
        // Get the current mouse position in 2D screen coordinates
        var mousePos2D = Input.mousePosition;
        // Convert the mouse position to 3D world coordinates
        mousePos2D.z = -Camera.main.transform.position.z;
        var mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        // Find the delta from the launchPos to the mousePos3D
        var mouseDelta = mousePos3D - launchPos;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider
        var maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // Move the bird to this new position
        var projPos = launchPos + mouseDelta;
        bird.transform.position = projPos;
        
        if (!Input.GetMouseButtonUp(0)) return;
        // The mouse has been released
        aimingMode = false;
        bird.GetComponent<Rigidbody>().isKinematic = false;
        bird.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
        bird.GetComponent<TrailRenderer>().enabled = true;
        FollowCam.S.poi = bird;
        bird = null;
    }
}
