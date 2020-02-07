using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {

    private GameObject heldObject;
    private bool isHolding = false;
    private float currentRotation = 0f;
    public OVRInput.Controller hand;

    private static float grabRadius = .05f;
    private static float distance = 1f;
    private static float rotateSpeed = 1f;

    public GameObject CheckHit()
    {
        GameObject item = null;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 0.1f, transform.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("Artwork"));
        
        if (hits.Length > 0)
        {
            // Collided so find closest hit artwork
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
                item = hits[closestHit].transform.gameObject;
            }
        }
        return item;
    }

    public GameObject GrabObject(GameObject item)
    {
        if(!isHolding)
        {
            while (item.transform.parent != null && (item.transform.parent.CompareTag("FloorArt") || item.transform.parent.CompareTag("WallArt")))
                item = item.transform.parent.gameObject;

            isHolding = true;
            heldObject = item;
            heldObject.transform.position = transform.position;
            //heldObject.layer = LayerMask.NameToLayer("HeldObject");
            heldObject.layer = LayerMask.NameToLayer("Artwork");
            heldObject.GetComponent<Collider>().isTrigger = true;
            foreach (Transform child in item.transform)
                child.gameObject.layer = LayerMask.NameToLayer("Artwork");
                //child.gameObject.layer = LayerMask.NameToLayer("HeldObject");
            Debug.Log("Grabbed the object named " + heldObject.ToString());
        }
        return heldObject;
    }

    public GameObject DropObject()
    {
        if (!isHolding)
            return null;

        
            GameObject oldHeld = heldObject;
            heldObject.layer = LayerMask.NameToLayer("Artwork");
            heldObject.GetComponent<Collider>().isTrigger = false;
            foreach (Transform child in heldObject.transform)
                child.gameObject.layer = LayerMask.NameToLayer("Artwork");
            isHolding = false;
            heldObject = null;
            currentRotation = 0f;
            return oldHeld;
        
    }

    public void AttemptSnap()
    {
        if (!isHolding)
            return;
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("Environment"));

        int snapHitIndex = FindSnapHitIndex(hits);
        if (snapHitIndex == -1)
            return;

        Snap(hits[snapHitIndex], heldObject);
    }

    private int FindSnapHitIndex(RaycastHit[] hits)
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

    private void Snap(RaycastHit targetHit, GameObject heldObject)
    {
        
        if (heldObject.tag == "FloorArt")
        {
              float displacement = heldObject.transform.lossyScale.y / 2;
                heldObject.transform.position = new Vector3(targetHit.point.x, targetHit.point.y + displacement, targetHit.point.z);

                heldObject.transform.forward = targetHit.transform.forward;
                heldObject.transform.up = targetHit.transform.up;
                heldObject.transform.Rotate(Vector3.up, currentRotation);
                heldObject.GetComponent<Collider>().isTrigger = false;
        }
        else if (heldObject.tag == "WallArt")
        {
                heldObject.transform.position = targetHit.point;
                heldObject.transform.up = targetHit.transform.up;
                heldObject.transform.forward = targetHit.transform.forward;
                Debug.Log(targetHit.transform.forward + "   " + heldObject.transform.forward);
                heldObject.GetComponent<Collider>().isTrigger = false;
        }

        
    }

    private bool IsAppropriateSurface(GameObject surface, GameObject artwork)
    {
        bool matchesFloor = surface.tag == "Floor" && artwork.tag == "FloorArt";
        bool matchesWall = surface.tag == "Wall" && artwork.tag == "WallArt";
        return (matchesFloor || matchesWall);
    }

    public void SetHeldObject(GameObject targetObject)
    {
        DeleteHeldObject();
        GameObject createdObject = Instantiate(targetObject, transform.position - new Vector3(0, 10, 0), Quaternion.identity);
        GrabObject(createdObject);
        createdObject.SetActive(true); //used because clones are initially invisible
    }

    public void DeleteHeldObject()
    {
        GameObject currentObject = heldObject;
        DropObject();
        Destroy(currentObject);
    }

    /// <summary>
    /// updates the transform of the held object with the desired rotation: 
    /// -1 = subtract rotation, 1 = add rotation, otherwise no rotation
    /// </summary>
    public void UpdateHeldObjectTransform(int rotateDirection)
    {
        if (!IsHolding())
            return;

        heldObject.transform.position = transform.position + transform.forward * distance;

        if (rotateDirection == 1)
            currentRotation += rotateSpeed;
        else if (rotateDirection == -1)
            currentRotation -= rotateSpeed;

        currentRotation = currentRotation % 360;
        heldObject.transform.rotation = transform.rotation;
        heldObject.transform.Rotate(Vector3.up, currentRotation);

        if (heldObject.tag == "WallArt")
        {
            heldObject.transform.Rotate(Vector3.up, 90);
        }
    }

    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    public bool IsHolding()
    {
        return isHolding;
    }

    public bool CheckCanDrop()
    {
        ColliderDetector cd = heldObject.GetComponent<ColliderDetector>();

        if (cd.GetCollisionStatus() == false)
        {
            return true;
        }
        return false;
    }
}
