using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    Vector3 targetPatrolPoint;
    float dist = 0;

    public PatrolState(Monster monster) : base(monster)
    {        
       
    }

    public override void OnstateEnter()
    {
        //Debug.Log("PatrolState ¡¯¿‘");
    }

    public override void OnstateExit()
    {
       
    }

    public override void OnstateStay()
    {
        if(monster.monsternum == 2)
        { stonemonster(); }
        else if(monster.monsternum == 3)
        { minidragon(); }
    }
    
    void stonemonster()
    {
        if (monster.CanSeePlayer())
        { monster.ChangeState(AllEnum.State.Chase);}
        //else if(monster.CanNearAttackPlayer())
        //{ monster.ChangeState(AllEnum.State.NearAttack); }
    }
    void minidragon()
    {
        if (monster.CanSeePlayer())
        {monster.ChangeState(AllEnum.State.Chase);}
        //else if (monster.CanNearAttackPlayer())
        //{ monster.ChangeState(AllEnum.State.NearAttack); }
        else if(monster.CanFarAttackPlayer())
        { monster.ChangeState(AllEnum.State.FarAttack); }
    }
}
