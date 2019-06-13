using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public class PlayerStateHolding : PlayerState {

    public PlayerStateHolding(StateData init_data) : base(init_data) { ; }

    protected override void EntryAction()
    {
        Debug.Log("Arrived in PlayerStateHolding");
        AllowMovement(true);
    }

    public override void StepState()
    {
        if (OneButtonIsDown())
            stateData.grabber.UpdateHeldObjectTransform(1);
        else if (TwoButtonIsDown())
            stateData.grabber.UpdateHeldObjectTransform(-1);
        else
            stateData.grabber.UpdateHeldObjectTransform(0);

        if (GripIsPressed())
            stateData.grabber.AttemptSnap();
    }

    public override Pair<bool, StateEnum> CheckExitConditions()
    {
        Pair<bool, StateEnum> pair = new Pair<bool, StateEnum>(false, StateEnum.Hold);

        if (!TriggerIsPressed())
            pair = new Pair<bool, StateEnum>(true, StateEnum.Default);

        return pair;
    }

    public override void ExitAction(StateEnum destinationState)
    {
        if (destinationState == StateEnum.Default)
            stateData.grabber.DropObject();
    }
}
