using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public class PlayerStateDefault : PlayerState {

    private bool enterFile = false;

    public PlayerStateDefault(StateData init_data) : base(init_data) { ; }

    protected override void EntryAction()
    {
        Debug.Log("Arrived in PlayerStateDefault");
        AllowMovement(true);
        
    }

    public override void StepState()
    {
        if (!LThumbstickIsPressed())
            enterFile = true;
    }

    public override Pair<bool, StateEnum> CheckExitConditions()
    {
        Pair<bool, StateEnum> pair = new Pair<bool, StateEnum>(false, StateEnum.Default);
        if (TriggerIsPressed() && stateData.grabber.CheckHit())
            pair = new Pair<bool, StateEnum>(true, StateEnum.Context);
        //else if (ThumbstickIsPressed()) 
        //    pair = new Pair<bool, StateEnum>(true, StateEnum.Pocket);
        else if (LThumbstickIsPressed() && enterFile)
            pair = new Pair<bool, StateEnum>(true, StateEnum.File);
        return pair;
    }

    public override void ExitAction(StateEnum destinationState)
    {

    }
}
