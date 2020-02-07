using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public class PlayerStateInventoryMenu : PlayerState {

    public PlayerStateInventoryMenu(StateData init_data) : base(init_data) { ; }

    protected override void EntryAction()
    {

    }

    public override void StepState()
    {

    }

    public override Pair<bool, StateEnum> CheckExitConditions()
    {
        Pair<bool, StateEnum> pair = new Pair<bool, StateEnum>(false, StateEnum.Default);
        return pair;
    }

    public override void ExitAction(StateEnum destinationState)
    {

    }
}
