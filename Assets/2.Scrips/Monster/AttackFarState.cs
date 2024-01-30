using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFarState : State
{
  
    public AttackFarState(Monster monster) : base(monster)
    {}

    public override void OnstateEnter()
    {
        //Debug.Log("AttackFarState");
    }

    public override void OnstateExit()
    {
        
    }

    public override void OnstateStay()
    {
        if (monster.CanSeePlayer() == false)
        {monster.ChangeState(AllEnum.State.Patrol);}
        else if (monster.CanSeePlayer())
        {monster.ChangeState(AllEnum.State.Chase);}
        else if(monster.CanNearAttackPlayer())
        {monster.ChangeState(AllEnum.State.NearAttack);}
    }
}
