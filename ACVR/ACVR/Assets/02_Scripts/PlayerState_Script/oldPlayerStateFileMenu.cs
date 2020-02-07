using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public class oldPlayerStateFileMenu : PlayerState {
    
    private GameObject fileMenu;

    private bool wasUp = false;
    private bool wasDown = false;
    private int currentPage = 0;

    private bool wasPressed = true;
    private string fileName, folderName;


    public oldPlayerStateFileMenu(StateData init_data) : base(init_data)
    {
    }

    protected override void EntryAction()
    {
        Debug.Log("Arrived in PlayerStateFileMenu");

        stateData.fileBrowser.SetVisibility(true);
        AllowMovement(false);
        
    }

    public override void StepState()
    {
        if (LThumbstickIsUp() && !wasUp)
        {
            stateData.fileBrowser.AdvanceUp();
        }
        if (LThumbstickIsDown() && !wasDown)
        {
            stateData.fileBrowser.AdvanceDown();
        }
        if (LOneButtonIsDown() && stateData.fileBrowser.GetSelection() < 2)
        {
            //saves the folder and file names to find them later
            if (currentPage == 0)
            {
                folderName = stateData.fileBrowser.GetFileName();
                stateData.pocketInventory.SetFilePath(Application.dataPath + "/Resources/Artwork/" + folderName);
            }
            else
            {
                fileName = stateData.fileBrowser.GetFileName();
                //stateData.pocketInventory.SetFilePath(Application.dataPath + "/Resources/Artwork/" + folderName + "/" + fileName);
            }

            currentPage += 1;
            stateData.fileBrowser.NextStage();
        }
        

        wasUp = LThumbstickIsUp();
        wasDown = LThumbstickIsDown();
        wasPressed = LThumbstickIsPressed();
    }

    public override Pair<bool, StateEnum> CheckExitConditions()
    {
        Pair<bool, StateEnum> pair = new Pair<bool, StateEnum>(false, StateEnum.File);

        if (LOneButtonIsDown() && stateData.fileBrowser.GetSelection() == 2)
            pair = new Pair<bool, StateEnum>(true, StateEnum.Pocket);
        else if (!LTriggerIsPressed())
            pair = new Pair<bool, StateEnum>(true, StateEnum.Default);
        return pair;
    }

    public override void ExitAction(StateEnum destinationState)
    {
        stateData.fileBrowser.SetVisibility(false);
    }

    ///////////////////////////////////////////////////////////////////////////////
    
    
}
