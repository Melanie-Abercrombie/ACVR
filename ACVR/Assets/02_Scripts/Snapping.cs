using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Use spherecast?
        RaycastHit hit;
        float rayRange = Mathf.Infinity;
        //Layermask floor art vs. wall art

        /*if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayRange))
        {

        }*/

        //Passed from RaycastGrabber

        //If Raycast grabber hits surface,
        //then is let go,
        SurfaceSnap(hit.collider);
    }

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
