using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNearState : State
{
    float dist = 0;
    public AttackNearState(Monster monster) : base(monster)
    { }

    public override void OnstateEnter()
    {
       // Debug.Log("AttackNearState");             
    }

    public override void OnstateExit()
    {
       
    }

    public override void OnstateStay()
    {
        if (monster.monsternum == 1)
        {Mushroom();}
        else if(monster.monsternum == 2)
        { Stonemonster(); }
        else if(monster.monsternum == 3)
        { MiniDragon(); }
    }
    void Mushroom()
    {
        if (monster.CanSeePlayer() && monster.CanNearAttackPlayer() == false)
        {monster.ChangeState(AllEnum.State.Chase);}
        else
        {monster.ChangeState(AllEnum.State.Idle);}
    }
    void Stonemonster()
    {
        if (monster.CanSeePlayer() && monster.CanNearAttackPlayer() == false)
        { monster.ChangeState(AllEnum.State.Chase); }
        else
        { monster.ChangeState(AllEnum.State.Idle); }
    }
    void MiniDragon()
    {
        if (monster.CanFarAttackPlayer())
        {monster.ChangeState(AllEnum.State.FarAttack);}
        else if (monster.CanSeePlayer() && monster.CanNearAttackPlayer() == false)
        {monster.ChangeState(AllEnum.State.Chase); }
        else
        { monster.ChangeState(AllEnum.State.Idle); }
    }
}

