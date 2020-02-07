using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Util.Enum;
using StateMachine.Util.Data;
using Collections.Pair;

public class PlayerStateMachineController : MonoBehaviour {

    [SerializeField]
    StateData stateData;
    PlayerState currState;

    void Start () {
        ChangeTo(StateEnum.Default);
        InvokeRepeating("Update60", 0f, 1/120f);	
	}

    private void Update60()
    {
        StepState();
        CheckChangeState();
    }

    private void StepState()
    {
        currState.StepState();
    }

    private StateEnum CheckChangeState()
    {
        Pair<bool, StateEnum> result = currState.CheckExitConditions();
        if (result.First)
        {
            currState.ExitAction(result.Second);
            ChangeTo(result.Second);
        }
        return result.Second;
    }

    private PlayerState ChangeTo(StateEnum state)
    { 
        switch(state)
        {
            case StateEnum.Default:
                currState = new PlayerStateDefault(stateData);
                break;
            case StateEnum.Context:
                currState = new PlayerStateContextMenu(stateData);
                break;
            case StateEnum.Pocket:
                currState = new PlayerStatePocketMenu(stateData);
                break;
            case StateEnum.Canvas:
                currState = new PlayerStateCanvasMenu(stateData);
                break;
            case StateEnum.Inventory:
                currState = new PlayerStateInventoryMenu(stateData);
                break;
            case StateEnum.Hold:
                currState = new PlayerStateHolding(stateData);
                break;
            case StateEnum.File:
                currState = new PlayerStateFileMenu(stateData);
                break;
        }
        return currState;
    }
}
