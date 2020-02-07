using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;
using System.Threading;

//  Manages the file menu on left hand and allows player to choose from 
//  this menu.
//  Entry action: LThumbstick is pressed once


public class PlayerStateFileMenu : PlayerState
{
    private bool selectedFile = false;
    
    private bool wasLeft = false;       // Tracks if thumbstick was left
    private bool wasRight = false;      // Tracks if thumbstick was right
    private bool wasPressed = false;    // Tracks if X button has been pressed
    private bool canExit = false;

    private bool canOpenFile = false;   
    private string fileName, folderName;

    private bool wasUp = false;         // Tracks if thumbstick was up
    private bool wasDown = false;       // Tracks if thumbstick was down
    private int currentPage = 0;        // Keeps track of what page of dir we're in; 
                                        // 0 -> main dir; 1 -> looking at files; 2 -> opened file             

    private bool previewingObject = false;
    private bool hasPlacedObject = false;
    

    public PlayerStateFileMenu(StateData init_data) : base(init_data) { ; }

    protected override void EntryAction()
    {
        Debug.Log("Arrived in PlayerStateFileMenu");
        
        stateData.fileBrowser.SetVisibility(true);
        AllowMovement(false);
    }

    public override void StepState()
    {
        if (!LThumbstickIsPressed())
            canExit = true;
        if (!LOneButtonIsDown())
            wasPressed = false;


        SaveInfo(currentPage);
        if (LThumbstickIsUp() && !wasUp)
        {
            stateData.fileBrowser.AdvanceUp();
            ScrollArt();
        }
        if (LThumbstickIsDown() && !wasDown)
        {
            stateData.fileBrowser.AdvanceDown();
            ScrollArt();
        }
        if (LOneButtonIsDown() && stateData.fileBrowser.GetSelection() < 2 && !wasPressed) // flip to next page
        {
            wasPressed = true;
            currentPage += 1;
            stateData.fileBrowser.NextStage();
        }

        //allows for previewing
        if (currentPage == 1 && previewingObject && !canOpenFile)
        {
            stateData.pocketInventory.SetOpenObject();
            previewingObject = !previewingObject; // to allow scrolling through objects
            canOpenFile = true;
        }

        // has a file been selected?
        if (LOneButtonIsDown() && previewingObject && !wasPressed)
        {
            // open object display of file
            //wasPressed = true;
            //canOpenFile = false;
            previewingObject = !previewingObject;
            hasPlacedObject = true;
        }


        wasPressed = LThumbstickIsPressed();
        wasUp = LThumbstickIsUp();
        wasDown = LThumbstickIsDown();
    }

    public override Pair<bool, StateEnum> CheckExitConditions()
    {

        Pair<bool, StateEnum> pair = new Pair<bool, StateEnum>(false, StateEnum.Default);
        if (selectedFile && stateData.grabber.CheckHit())
            pair = new Pair<bool, StateEnum>(true, StateEnum.Hold);
        else if ((LThumbstickIsPressed() && canExit) || hasPlacedObject)
            pair = new Pair<bool, StateEnum>(true, StateEnum.Default);

        return pair;
    }

    public override void ExitAction(StateEnum destinationState)
    {
        stateData.fileBrowser.SetVisibility(false);

        if (previewingObject)
            stateData.pocketInventory.DeleteOpenObject();
        else
            stateData.pocketInventory.RemovedOpenObject();
    }

    //////////////////////////////////////////////////////////////////


    // saves the folder and file names to find them later
    private void SaveInfo(int stage)
    {
        if (stage == 0)
        {
            folderName = stateData.fileBrowser.GetFileName();
            //stateData.pocketInventory.SetFilePath(Application.dataPath + "/Resources/Artwork/" + folderName);
        }
        else if (stage == 1)
        {
            fileName = stateData.fileBrowser.GetFileName();
            stateData.pocketInventory.SetFilePath(Application.dataPath + "/Resources/Artwork/" + folderName + "/" + fileName);
            previewingObject = true;
        }
    }
    
    //if we are already previwing an artwork, allows user to scroll through others with menu & LThumbstick
    private void ScrollArt()
    {
        stateData.pocketInventory.DeleteOpenObject();
        if (previewingObject)
        {
            fileName = stateData.fileBrowser.GetFileName();
            stateData.pocketInventory.SetFilePath(Application.dataPath + "/Resources/Artwork/" + folderName + "/" + fileName);
            stateData.pocketInventory.SetOpenObject();
        }
    }
}
