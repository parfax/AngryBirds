using UnityEngine;

public class Slingshot : MonoBehaviour
{
    #region Components

    public GameObject birdPrefab;
    public GameObject launchPoint, bird;
    public Vector3 launchPos;
    private FollowCam _camera;

    #endregion
    
    
    public bool aimingMode;
    public float velocityMult;

    private void Start()
    {
        _camera = Camera.main.GetComponent<FollowCam>();
        var launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
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
        aimingMode = true;
        
        bird = Instantiate(birdPrefab);
        bird.transform.position = launchPos;
        bird.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode) return;
        
        var mousePos2D = Input.mousePosition;
        
        mousePos2D.z = -Camera.main.transform.position.z;
        var mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        
        var mouseDelta = mousePos3D - launchPos;
        
        var maxMagnitude = GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        
        var projPos = launchPos + mouseDelta;
        bird.transform.position = projPos;
        
        if (!Input.GetMouseButtonUp(0)) return;
        // The mouse has been released
        aimingMode = false;
        bird.GetComponent<Rigidbody>().isKinematic = false;
        bird.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
        bird.GetComponent<TrailRenderer>().enabled = true;
        _camera.target = bird;
        bird = null;
    }
}
