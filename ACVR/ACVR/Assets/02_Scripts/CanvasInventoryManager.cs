using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInventoryManager : MonoBehaviour
{

    [SerializeField]
    private InventoryLoader source;
    private static LinkedList<GameObject> artInventory;
    private LinkedListNode<GameObject> currentElement;

    // Use this for initialization
    void Start()
    {
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
        //get number of items, 'k,' from InventoryLoader
        int k = 0; // = ?
        //Find largest multiple of 9 that will hold 'k' items, subtract 1 for 0-indexing
        int size = (((k % 9) + 1) * 9) - 1;
        //1D array for all artwork
        GameObject[] galleryMenu = new GameObject[size];
        //Load inventory from Inventory Manager
    }

    void SetToStart()
    {
        currentElement = artInventory.First;
    }

    public GameObject GetCurrentInventoryObject()
    {
        return currentElement.Value;

       
    }

 //Each page is a state
        //Loads index by multiples of 9
        //Do not need physical pages
        //"Next" button increments page, but next page just changes which index are loaded
        //Instantiates prefab for each inventory button based on index		
        //NEED PREFAB FOR EACH ARTWORK

}