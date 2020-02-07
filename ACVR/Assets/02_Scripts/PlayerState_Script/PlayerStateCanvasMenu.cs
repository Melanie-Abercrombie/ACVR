using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public class PlayerStateCanvasMenu : PlayerState
{
    //Two approaches:
    // - Raycast to point to button, 
    // - Thumbstick to select button
    private bool wasLeft = false;       // Tracks if thumbstick was left
    private bool wasRight = false;      // Tracks if thumbstick was right
    private bool wasPressed = true;     // Tracks if thumbstick was pressed

    public PlayerStateCanvasMenu(StateData init_data) : base(init_data) {; }

    protected override void EntryAction()
    {
        Debug.Log("Arrived in PlayerStateCanvasMenu");
        AllowMovement(false);
        //stateData.canvasInventory.SetCurrentToTarget(stateData.grabber.GetHeldObject());
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
        else if (ThumbstickIsPressed() && !wasPressed)
            pair = new Pair<bool, StateEnum>(true, StateEnum.Default);

        return pair;
    }

    public override void ExitAction(StateEnum destinationState)
    {
        if (destinationState == StateEnum.Default)
            stateData.grabber.DeleteHeldObject();
    }
}
