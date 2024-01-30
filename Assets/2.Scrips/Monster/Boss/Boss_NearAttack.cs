using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_NearAttack : State
{
    bool Scream = false;
    float dist = 0;
    public Boss_NearAttack(Boss boss) : base(boss)
    {
    }

    public override void OnstateEnter()
    {
        //Debug.Log("근접공격 상태 진입");
    }

    public override void OnstateExit()
    {
       
    }

    public override void OnstateStay()
    {
        if (boss.bossNum == 1)
        { BossOne();}
        else if (boss.bossNum == 2)
        { BossTwo();}
    }
    void BossOne()
    {
        if (boss.B_Stat.HP <= 13000 && Scream == false && boss.CanSeePlayer())
        {
            Scream = true;
            boss.ChangeState(AllEnum.Boss_State.Scream);
            return;
        }
        else if (boss.CanSeePlayer() && boss.CanAttackNear() == false)
        {
            boss.ChangeState(AllEnum.Boss_State.FarAttack);
        }
    }
    void BossTwo()
    {
        dist = Vector3.Distance(boss.transform.position, PlayerManager.Instance.transform.position);
        if (dist > 20f && dist <= 25)
        {boss.ChangeState(AllEnum.Boss_State.FarAttack);}
        else if(dist > 25f)
        {boss.ChangeState(AllEnum.Boss_State.Chase);}        
    }
}
