using UnityEngine;

public class Goal : MonoBehaviour
{
    // A static field accessible by code anywhere
    public bool goalMet;

    private void OnTriggerEnter(Collider col)
    {
        // When the trigger is hit by something
        // Check to see if it's a Projectile
        if (!col.CompareTag("Projectile")) return;
        
        // If so, set goalMet to true
        goalMet = true;
        // Also set the alpha of the color to higher opacity
        var c = GetComponent<Renderer>().material.color;
        c.a = .1f;
        GetComponent<Renderer>().material.color = c;
    }
}
