using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryLoader : MonoBehaviour {

    [SerializeField]
    public GameObject[] testArtList;        // temporarly in use for preloaded, hardcoded specified artwork

    private GameObject genericPainting;     // for possible future use when reading in data
    private GameObject genericSculpture;    // for possible future use when reading in data

    public LinkedList<GameObject> artInventory;

    private void Awake()
    {
        ReadInInventory();    
    }

    private LinkedList<GameObject> ReadInInventory()
    {
        // for use when we allow for dynamicly adding artwork to the game
        // this function should read in all of the desired artwork, create prefabs for them (with appropriate tags/layers) and use that to populate the artList

        artInventory =  ArrayToLinkedList(testArtList);  // note this 1 line implementation is just for testing purposes
        return artInventory;
    }

    public LinkedList<GameObject> FetchInventory()
    {
        return artInventory;
    }

    private LinkedList<GameObject> ArrayToLinkedList(GameObject[] array)
    {
        LinkedList<GameObject> linkedList = new LinkedList<GameObject>();

        foreach (GameObject element in array)
            linkedList.AddLast(element);

        return linkedList;
    }
}
