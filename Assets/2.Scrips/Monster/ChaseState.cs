using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public ChaseState(Monster monster) : base(monster)
    { }

    public override void OnstateEnter()
    {
        //Debug.Log("ChaseState ¡¯¿‘");
    }

    public override void OnstateExit()
    {
    }

    public override void OnstateStay()
    {
        if (monster.monsternum == 1)
        {Mushroom();}
        else if (monster.monsternum == 2)
        {Stonemonster();}
        else if(monster.monsternum == 3)
        { MiniDragon();}
    }

    void Mushroom()
    {
        if (monster.CanNearAttackPlayer())
        {monster.ChangeState(AllEnum.State.NearAttack);}
        else if(monster.CanSeePlayer() == false)
        { monster.ChangeState(AllEnum.State.Idle); }
    }
    void Stonemonster()
    {
        if(monster.CanSeePlayer() == false)
        { monster.ChangeState(AllEnum.State.Patrol); }
        else if(monster.CanNearAttackPlayer())
        { monster.ChangeState(AllEnum.State.NearAttack); }
    }
    void MiniDragon()
    {
        if (monster.CanSeePlayer() == false)
        { monster.ChangeState(AllEnum.State.Patrol); }
        else if(monster.CanFarAttackPlayer())
        { monster.ChangeState(AllEnum.State.FarAttack); }
        else if (monster.CanNearAttackPlayer())
        { monster.ChangeState(AllEnum.State.NearAttack); }
    }
}
