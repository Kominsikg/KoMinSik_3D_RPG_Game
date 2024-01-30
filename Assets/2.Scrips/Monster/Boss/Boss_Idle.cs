using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Idle : State
{    
    float idleTime = 0f;
    bool Scream = false;
    float dist = 0;
    public Boss_Idle(Boss boss) : base(boss)
    {

    }

    public override void OnstateEnter()
    {
        Scream = false;
        idleTime = 0;
        //Debug.Log("아이들 상태 진입");
    }

    public override void OnstateExit()
    {
        idleTime = 0;        
    }

    public override void OnstateStay()
    {
        if (boss.bossNum == 1)
        {BossOne();}
        else if (boss.bossNum == 2)
        {BossTwo();}

    }

    void BossOne()
    {
        if (boss.B_Stat.HP <= 13000 && Scream == false && boss.CanSeePlayer())
        {
            Scream = true;
            boss.ChangeState(AllEnum.Boss_State.Scream);            
        }
        if (idleTime >= 0.5f)
        {
            if (boss.CanAttackNear())
            { boss.ChangeState(AllEnum.Boss_State.NearAttack); }
            else if (boss.CanSeePlayer() && boss.CanAttackNear() == false)
            { boss.ChangeState(AllEnum.Boss_State.FarAttack); }
        }
        else
        { idleTime += Time.deltaTime; }
    }
    void BossTwo()
    {
        if (boss.B_Stat.HP <= 10000 && Scream == false && boss.CanSeePlayer())
        {
            Scream = true;
            boss.ChangeState(AllEnum.Boss_State.Scream);
            return;
        }
        dist = Vector3.Distance(boss.transform.position, PlayerManager.Instance.transform.position);
        if (idleTime >= 0.5f)
        {
            if (dist <= 20f)
            { boss.ChangeState(AllEnum.Boss_State.NearAttack); }
            else if (boss.CanSeePlayer())
            {
                if (dist <= 25f)
                {
                    boss.ChangeState(AllEnum.Boss_State.FarAttack);
                    return;
                }
                boss.ChangeState(AllEnum.Boss_State.Chase);                
            }
        }
        else
        { idleTime += Time.deltaTime; }
    }    
}

    

