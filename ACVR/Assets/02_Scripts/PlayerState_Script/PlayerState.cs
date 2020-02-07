using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public abstract class PlayerState { // Do I even need a monobehaviour?

    protected static StateData stateData;
    
    public PlayerState(StateData init_data) {
        stateData = init_data;
        EntryAction();
	}

    protected abstract void EntryAction();
    public abstract void StepState();
    public abstract Pair<bool,StateEnum> CheckExitConditions();
    public abstract void ExitAction(StateEnum destinationState);

    protected bool AllowMovement(bool desired)
    {
        stateData.OVRPlayer.EnableLinearMovement = desired;
        stateData.OVRPlayer.EnableRotation = desired;
        return (stateData.OVRPlayer.EnableLinearMovement);
    }

    protected bool TriggerIsPressed()
    {
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, stateData.hand) > 0.8f);
    }

    protected bool GripIsPressed()
    {
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, stateData.hand) > 0.8f);
    }

    protected bool ThumbstickIsPressed()
    {
        return (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, stateData.hand));
    }

    protected bool ThumbstickIsRight()
    {
        return (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, stateData.hand).x > 0.8);
    }

    protected bool ThumbstickIsLeft()
    {
        return (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, stateData.hand).x < -0.8);
    }

    protected bool ThumbstickIsUp()
    {
        return (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, stateData.hand).y > 0.8);
    }

    protected bool ThumbstickIsDown()
    {
        return (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, stateData.hand).y < -0.8);
    }

    protected bool StartButtonPressed()
    {
        //Since used for toggling menu, check change of state with GetDown() rather than current state
        return (OVRInput.GetDown(OVRInput.Button.Start, stateData.hand));
    }

    protected bool OneButtonIsDown()
    {
        return (OVRInput.Get(OVRInput.Button.One, stateData.hand));
    }

    protected bool TwoButtonIsDown()
    {
        return (OVRInput.Get(OVRInput.Button.Two, stateData.hand));
    }

    protected bool LThumbstickIsPressed()
    {
        return (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, stateData.hand2));
    }

    protected bool LThumbstickIsUp()
    {
        return (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, stateData.hand2).y > 0.8);
    }

    protected bool LThumbstickIsDown()
    {
        return (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, stateData.hand2).y < -0.8);
    }

    protected bool LTriggerIsPressed()
    {
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, stateData.hand2) > 0.8f);
    }

    protected bool LOneButtonIsDown()
    {
        return (OVRInput.GetDown(OVRInput.Button.One, stateData.hand2));
    }
    
}
