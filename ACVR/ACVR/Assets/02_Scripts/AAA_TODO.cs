


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAA_TODO {
    /*
    ---To-do list---
    
    improve context menu creation (rotation, scale, general look)
    collision detection on snapping
    inventory manager/state
    inventory loading from file
   
    model structure
    model artwork

    visual feedback (hands / ray)

    update documentation



    ---MVP cuttoff---
    
    model structure
    inventory (pocket held items)
    make more artwork
    make pedastals and frames
    
    visual feedback (hands / ray)

    */

    /*
    --- feature discussion ---
    inventory interaction can be done by pressing in on the controller and pressing side to side
    inventory interaction can also be done by pressing the menu button which will pull up a menu they can select from
    while interacting with the inventory your movement will be locked

    when you point and click an item it should pull up a context menu with the things they can do
        grab, delete, (change frame,) cancel...
    
     should be able to read in files (pictures and modles) from a preset path
     in this path will be a file specifying which files should be read in, what type of artwork they are, intended size (at least for paintings), and any other relevant tags
     likely use the directory path Application.dataPath + "/StreamingAssets/Artwork", but need to check if that is accessable after building
     */

    //////////////////
    /* 
    ---Known bugs--- 

    held artworks can be placed inside others


    ---Up next---
    
    update documentation
    improving context menu
    implementing inventory / state

    */
}
