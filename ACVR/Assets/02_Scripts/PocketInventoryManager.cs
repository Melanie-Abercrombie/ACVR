using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketInventoryManager : MonoBehaviour {

    [SerializeField]
    private InventoryLoader source;
    private static LinkedList<GameObject> artInventory;
    private LinkedListNode<GameObject> currentElement;

    // Use this for initialization
    void Start() {
        FetchSource();
        PopulateInventory();
        SetToStart();
    }

    void FetchSource()
    {
        source = gameObject.GetComponent<InventoryLoader>();
    }

    void PopulateInventory()
    {
        // TODO: populate inventory. initially should load from a preset, but in the future should populate inventory from the contents in a file path
        // note: remember to keep one element (preferably the first, as nothing for users who dont want to hold anything)?
        artInventory = source.FetchInventory();
    }

    public void SetCurrentToTarget(GameObject target)
    {
        currentElement = artInventory.Find(target);
        if (currentElement == null) // not found
            SetToStart();
    }

    void SetToStart()
    {
        currentElement = artInventory.First;
    }
    
    void SetToEnd()
    {
        currentElement = artInventory.Last;
    }

    public GameObject ItemForward()
    {
        if (currentElement == artInventory.Last)
            SetToStart();
        else
            currentElement = currentElement.Next;
        
        return (currentElement.Value);
    }

    public GameObject ItemBackward()
    {
        if (currentElement == artInventory.First)
            SetToEnd();
        else
            currentElement = currentElement.Previous;

        return (currentElement.Value);
    }

    void UpdateHeldItem()
    {
        Debug.Log(currentElement.Value.ToString());
        // TODO: update held item in the player manager and grabber controls
        // note: dont need to remove the item from their inventory as they can place everything multiple times
    }

    public GameObject GetCurrentInventoryObject()
    {
        return currentElement.Value;
    }
}

//  control interface:
//  trigger pressed (regardless of holding an item)
//      move stick side to side
//          changes item (puts new item in hand)

//  inventory layout:
//  circular doubly linked with hardcoded empty slot
//  each index holds an item

//  when trigger is first pressed add their current item to the dequeue
//  when moving the joystick simply manipulate the index and update their held item (will need to set grab if not grabbing anything yet)
//  when trigger is released remove current item from the dequeue (unless it is the empty slot)
