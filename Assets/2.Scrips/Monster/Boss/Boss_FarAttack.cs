using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FarAttack : State
{
    bool Scream = false;
    float dist;
    public Boss_FarAttack(Boss boss) : base(boss)
    {
    }

    public override void OnstateEnter()
    {
        //Debug.Log("원거리 공격 상태 진입");
    }

    public override void OnstateExit()
    {       
    }

    public override void OnstateStay()
    {
        if(boss.bossNum == 1)
        { BossOne();}
        else if (boss.bossNum == 2)
        { BossTwo(); }
    }
    void BossOne()
    {
        if (boss.B_Stat.HP <= 13000 && Scream == false && boss.CanSeePlayer())
        {
            Scream = true;
            boss.ChangeState(AllEnum.Boss_State.Scream);
            return;
        }
        if (boss.CanAttackNear())
        {
            boss.ChangeState(AllEnum.Boss_State.NearAttack);
        }
        else if (boss.CanSeePlayer() == false)
        {
            boss.ChangeState(AllEnum.Boss_State.Idle);
        }
    }
    void BossTwo()
    {
        
        dist = Vector3.Distance(boss.transform.position,PlayerManager.Instance.transform.position);
        if (dist <= 20f)
        {
            boss.ChangeState(AllEnum.Boss_State.NearAttack);
        }
        else if (dist > 25f)
        {
            boss.ChangeState(AllEnum.Boss_State.Chase);
        }
        else if (boss.CanSeePlayer() == false)
        {
            boss.ChangeState(AllEnum.Boss_State.Idle);
        }
    }
}

