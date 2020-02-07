using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberControls : MonoBehaviour {

    // Note on ignoring layer collisions. Physics.IgnoreLayerCollision(0, LayerMask.NameToLayer("Artwork"), true);

    private GameObject heldObject;
    private bool isHolding = false;
    private float currentYRotation = 0f;

    public OVRInput.Controller hand;
    public float grabRadius;
    public float distance = 1f;
    public float rotateSpeed = 10f;

    // Update is called once per frame
    void Update() {
        if (isHolding) {
            UpdateHeldObjectTransform();
        }
    }

    void UpdateHeldObjectTransform()
    {
        heldObject.transform.position = transform.position + transform.forward * distance;

        if (OVRInput.Get(OVRInput.Button.One, hand))
            currentYRotation += rotateSpeed;
        if (OVRInput.Get(OVRInput.Button.Two, hand))
            currentYRotation -= rotateSpeed;

        currentYRotation = currentYRotation % 360;
        heldObject.transform.rotation = transform.rotation;
        heldObject.transform.Rotate(Vector3.up, currentYRotation);

        if (heldObject.tag == "WallArt")
        {
            heldObject.transform.Rotate(Vector3.up, 90);
        }
    }

    public void AttemptGrab()
    {
        if (isHolding)
            return;

        RaycastHit[] hits;

        //Sphere of interaction around hand
        hits = Physics.SphereCastAll(transform.position, 0.1f, transform.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("Artwork"));

        //Collided
        if (hits.Length > 0)
        {
            //Grabs closest object
            bool found = false;
            int closestHit = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if (!found || hits[i].distance < hits[closestHit].distance)
                {
                    found = true;
                    closestHit = i;
                }
            }

            if (found)
            {
                GameObject item = hits[closestHit].transform.gameObject;
                GrabObject(item);
            }
        }
    }

    public void GrabObject(GameObject item)
    {
        while (item.transform.parent != null && (item.transform.parent.CompareTag("FloorArt") || item.transform.parent.CompareTag("WallArt")))
            item = item.transform.parent.gameObject;

        isHolding = true;
        heldObject = item;
        heldObject.transform.position = transform.position;
        heldObject.layer = LayerMask.NameToLayer("HeldObject");
        heldObject.GetComponent<Collider>().isTrigger = true;
        foreach (Transform child in item.transform)
            child.gameObject.layer = LayerMask.NameToLayer("HeldObject");
        Debug.Log("held the object named " + heldObject.ToString());
    }

    public void DropObject()
    {
        if (!isHolding)
            return;

        heldObject.layer = LayerMask.NameToLayer("Artwork");
        heldObject.GetComponent<Collider>().isTrigger = false;
        foreach (Transform child in heldObject.transform)
            child.gameObject.layer = LayerMask.NameToLayer("Artwork");
        isHolding = false;
        heldObject = null;
        currentYRotation = 0f;
    }

    public void AttemptSnap()
    {
        if (!isHolding)
            return;

        Debug.Log("Attempting Snap");
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("Environment"));

        int snapHitIndex = FindSnapHitIndex(hits);
        if (snapHitIndex == -1)
            return;

        Snap(hits[snapHitIndex], heldObject);
    }

    int FindSnapHitIndex(RaycastHit[] hits)
    {
        int i = 0;
        float currentDist = Mathf.Infinity;
        int currentHitIndex = -1;

        while (i < hits.Length)
        {
            if (IsAppropriateSurface(hits[i].transform.gameObject, heldObject))
            {
                float tempDist = Vector3.Distance(hits[i].transform.position, heldObject.transform.position);
                if (tempDist < currentDist)
                {
                    currentDist = tempDist;
                    currentHitIndex = i;
                }
            }
            ++i;
        }
        return currentHitIndex;
    }

    void Snap(RaycastHit targetHit, GameObject heldObject)
    {
        if(heldObject.tag == "FloorArt")
        {
            float displacement = heldObject.transform.lossyScale.y / 2;
            heldObject.transform.position = new Vector3(targetHit.point.x, targetHit.point.y + displacement, targetHit.point.z);
            
            heldObject.transform.forward = targetHit.transform.forward;
            heldObject.transform.up = targetHit.transform.up;
            heldObject.transform.Rotate(Vector3.up, currentYRotation);
        }
        else if(heldObject.tag == "WallArt")
        {
            heldObject.transform.position = targetHit.point;
            heldObject.transform.forward = targetHit.transform.forward;
            heldObject.transform.up = targetHit.transform.up;
        }
    }

    bool IsAppropriateSurface(GameObject surface, GameObject artwork)
    {
        bool matchesFloor = surface.tag == "Floor" && artwork.tag == "FloorArt";
        bool matchesWall = surface.tag == "Wall" && artwork.tag == "WallArt";
        return (matchesFloor || matchesWall);
    }

    public void SetHeldObject(GameObject targetObject)
    {
        GameObject currentObject = heldObject;
        DropObject();
        Destroy(currentObject);
        GameObject createdObject = Instantiate(targetObject, transform.position - new Vector3(0,10,0), Quaternion.identity);
        GrabObject(createdObject);
    }

    public GameObject HeldObject()
    {
        return heldObject;
    }

    public bool IsHolding()
    {
        return isHolding;
    }
}
