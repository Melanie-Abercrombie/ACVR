using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour {

    public Vector3 vecRotation;
    public Vector3 halfExtents;

    private bool isWallHanging;
    private Quaternion quatRotation;

    void Start()
    {
        isWallHanging = (gameObject.tag == "WallArt");
    }

    public void AttemptSnap()
    {
    /*
        Call fron grabber controller

        Raycast in direction of controller
        If nearest wall/floor hit is the appropriate wall/floor
        Align object with surface RaycastHit.point
            Should be easy for statues just match up with up
            Should also be easy for paintings just align normal and match up with up
    */
    }

    /*
    void Update () {
        //Use spherecast?
        RaycastHit hit;
        float rayRange = Mathf.Infinity;
        //Layermask floor art vs. wall art

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayRange))
        {

        }

        //Passed from RaycastGrabber

        //If Raycast grabber hits surface,
        //then is let go,
        //SurfaceSnap(hit.collider); COMMENTED FOR TESTING
    }
    */

    //Attaches an artwork to its appropriate surface when close
    public void SurfaceSnap(Collider col)
    {
        if (col.CompareTag("WallArt"))
        {
            Debug.Log("Wall art");
            Debug.Log(col.name);
            //Sets artwork orientation to match the surface
            //transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        }

        if (col.CompareTag("FloorArt"))
        {
            Debug.Log("Floor art");
            Debug.Log(col.name);
            //Sets artwork orientation to match the surface
            //transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }

        //Physics raycast small distance - get normal of what is hit - determines forward-facing vector
    }
}
