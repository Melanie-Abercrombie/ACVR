using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public class PlayerStateContextMenu : PlayerState {

    private GameObject targetObject;
    private GameObject contextMenu;
    private ContextMenuController contextMenuController;
    private static GameObject[] contextMenuPrefabs = new GameObject[2];
    private bool wasUp = false;
    private bool wasDown = false;


    public PlayerStateContextMenu(StateData init_data) : base(init_data) {
    }

    protected override void EntryAction()
    {
        Debug.Log("Arrived in PlayerStateContextMenu");
        contextMenuPrefabs[0] = Resources.Load("PaintingContextMenu") as GameObject;
        contextMenuPrefabs[1] = Resources.Load("SculptureContextMenu") as GameObject;
        targetObject = stateData.grabber.CheckHit();
        AllowMovement(false);
        targetObject = FindRootArtwork(targetObject);
        contextMenu = DisplayContextMenuOn(targetObject);
        contextMenuController = contextMenu.GetComponent<ContextMenuController>();
    }
    

    public override void StepState()
    {
        if (ThumbstickIsUp() && !wasUp)
        {
            contextMenuController.AdvanceUp();
        }
        if(ThumbstickIsDown() && !wasDown)
        {
            contextMenuController.AdvanceDown();
        }
        wasUp = ThumbstickIsUp();
        wasDown = ThumbstickIsDown();
      
    }

    public override Pair<bool, StateEnum> CheckExitConditions()
    {
        Pair<bool, StateEnum> pair = new Pair<bool, StateEnum>(false, StateEnum.Context);
        if (!TriggerIsPressed())
        { 
            pair = new Pair<bool, StateEnum>(true, StateEnum.Default);
        }
        else if (OneButtonIsDown())
        {
            int selection = HandleSelection();
            if (selection == 0)
                pair = new Pair<bool, StateEnum>(true, StateEnum.Hold);
            else if (selection == 1)
                pair = new Pair<bool, StateEnum>(true, StateEnum.Default);
        }

        return pair;
    }

    public override void ExitAction(StateEnum destinationState)
    {
        GameObject.Destroy(contextMenu);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    private GameObject DisplayContextMenuOn(GameObject targetObject)
    {
        GameObject menu = null;
        if (targetObject.tag == "WallArt")
            menu = GameObject.Instantiate(contextMenuPrefabs[0]);
        else
            menu = GameObject.Instantiate(contextMenuPrefabs[1]);

        menu.transform.position = targetObject.transform.position;
        menu.transform.rotation = targetObject.transform.rotation;
        menu.transform.RotateAround(menu.transform.position, menu.transform.up, 90);
        
        return menu;
    }

    private int HandleSelection()
    {
        if (contextMenuController.GetSelection() == 0)
        {
            stateData.grabber.GrabObject(targetObject);
            return 0;
        }
        if (contextMenuController.GetSelection() == 1)
        {
            GameObject.Destroy(targetObject);
            return 1;
        }

        return contextMenuController.GetSelection();
    }

    private GameObject FindRootArtwork(GameObject item)
    {
        while (item.transform.parent != null && (item.transform.parent.CompareTag("FloorArt") || item.transform.parent.CompareTag("WallArt")))
            item = item.transform.parent.gameObject;
        return item;
    }
}
