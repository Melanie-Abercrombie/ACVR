using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public class PlayerStatePocketMenu : PlayerState {

    private bool wasLeft = false;       // Tracks if thumbstick was left
    private bool wasRight = false;      // Tracks if thumbstick was right
    private bool wasPressed = true;     // Tracks if thumbstick was pressed

    public PlayerStatePocketMenu(StateData init_data) : base(init_data) { ; }

    protected override void EntryAction()
    {
        Debug.Log("Arrived in PlayerStatePocketMenu");
        AllowMovement(false);
        stateData.pocketInventory.FetchSource();
        stateData.pocketInventory.PopulateInventory();
        stateData.pocketInventory.SetToStart();
        //stateData.pocketInventory.SetToCustom();
        stateData.pocketInventory.SetCurrentToTarget(stateData.grabber.GetHeldObject());
    }

    public override void StepState()
    {
        
        if (ThumbstickIsRight() && !wasRight)
        {
            stateData.pocketInventory.ItemForward();
            stateData.grabber.SetHeldObject(stateData.pocketInventory.GetCurrentInventoryObject());
        }
        if (ThumbstickIsLeft() && !wasLeft)
        {
            stateData.pocketInventory.ItemBackward();
            stateData.grabber.SetHeldObject(stateData.pocketInventory.GetCurrentInventoryObject());
        }
        
        stateData.grabber.UpdateHeldObjectTransform(0);

        wasPressed = ThumbstickIsPressed();
        wasRight = ThumbstickIsRight();
        wasLeft = ThumbstickIsLeft();
    }

    public override Pair<bool, StateEnum> CheckExitConditions()
    {
        Pair<bool, StateEnum> pair = new Pair<bool, StateEnum>(false, StateEnum.Pocket);

        if (TriggerIsPressed())
            pair = new Pair<bool, StateEnum>(true, StateEnum.Hold);
        else if (!LTriggerIsPressed())  //ThumbstickIsPressed() && !wasPressed)
            pair = new Pair<bool, StateEnum>(true, StateEnum.Default);


        return pair;
    }

    public override void ExitAction(StateEnum destinationState)
    {
        if (destinationState == StateEnum.Default)
            stateData.grabber.DeleteHeldObject();
    }
}
