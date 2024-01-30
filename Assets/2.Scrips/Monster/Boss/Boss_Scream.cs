using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Scream : State
{
    float dist = 0;
    float ScreamTime = 0;
    public Boss_Scream(Boss boss) : base(boss)
    {
    }

    public override void OnstateEnter()
    {
        //Debug.Log("스크림 상태 진입");
    }

    public override void OnstateExit()
    {
        
        
    }

    public override void OnstateStay()
    {
        if (boss.bossNum == 1)
        {BossOne();}       
        
    }
    void BossOne()
    {
        if (ScreamTime >= 10f)
        {
            if (boss.CanAttackNear())
            { boss.ChangeState(AllEnum.Boss_State.NearAttack); }
            else if (boss.CanSeePlayer() && boss.CanAttackNear() == false)
            { boss.ChangeState(AllEnum.Boss_State.FarAttack); }
            else
            { boss.ChangeState(AllEnum.Boss_State.Idle); }
        }
        else { ScreamTime += Time.deltaTime; }

    }
    
}
