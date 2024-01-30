using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    State state;
    public FSM(State _state)
    {
        state = _state;
    }

    public void ChangeState(State nextState)
    {
        if (nextState == state)
        {return;}
        if (state != null)
        {state.OnstateExit();}

        state = nextState;
        state.OnstateEnter();
    }

    public void UpdateState()
    {
        if (state != null)
        {state.OnstateStay();}
    }

}
