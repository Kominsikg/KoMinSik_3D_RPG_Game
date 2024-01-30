using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    float idleTime = 0f; 

    public IdleState(Monster monster) : base(monster)
    {}

    public override void OnstateEnter()
    {
        //Debug.Log("IdleState ÁøÀÔ");
        idleTime = 0f; 
    }

    public override void OnstateExit()
    {
        idleTime = 0f;   
    }

    public override void OnstateStay()
    {
        if (monster != null && monster.TargetTr != null)
        {                       
            if (idleTime >= 0.5f)
            {
                if (monster.monsternum == 3)
                { MiniDragon();}
                else if(monster.monsternum == 2)
                {StoneMonster();}
                else if(monster.monsternum == 1)
                {Mushroom();}
            }
            else
            {idleTime += Time.deltaTime; }
        }
    }
    void MiniDragon()
    {
        if (monster.CanNearAttackPlayer())
        { monster.ChangeState(AllEnum.State.NearAttack); }
        else if (monster.CanSeePlayer() && monster.CanNearAttackPlayer() == false)
        { monster.ChangeState(AllEnum.State.Chase); }
        else if(monster.CanSeePlayer() == false)
        { monster.ChangeState(AllEnum.State.Patrol); }
        else if (monster.CanFarAttackPlayer())
        { monster.FarAttack(); }
    }
    void StoneMonster()
    {
        if (monster.CanNearAttackPlayer())
        { monster.ChangeState(AllEnum.State.NearAttack); }
        else if (monster.CanSeePlayer() && monster.CanNearAttackPlayer() == false)
        { monster.ChangeState(AllEnum.State.Chase); }
        else if(monster.CanSeePlayer() == false)
        {monster.ChangeState(AllEnum.State.Patrol);}
    }
    void Mushroom()
    {
        if (monster.CanNearAttackPlayer())
        { monster.ChangeState(AllEnum.State.NearAttack);}
        else if (monster.CanSeePlayer() && monster.CanNearAttackPlayer() == false)
        { monster.ChangeState(AllEnum.State.Chase); }
    }
    
}
