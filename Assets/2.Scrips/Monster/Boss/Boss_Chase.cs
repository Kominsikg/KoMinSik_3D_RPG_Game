using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Chase : State
{
    bool Scream = false;
    float dist = 0;
    public Boss_Chase(Boss boss) : base(boss)
    {
    }
    public override void OnstateEnter()
    {
        //Debug.Log("추적 상태 진입");        
    }

    public override void OnstateExit()
    {
    }

    public override void OnstateStay()
    {
      
        dist = Vector3.Distance(boss.transform.position, PlayerManager.Instance.transform.position);
        if (dist <= 20f)
        {
            boss.ChangeState(AllEnum.Boss_State.NearAttack);
        }
        else if(dist <= 25f && dist > 20f)
        {
            boss.ChangeState(AllEnum.Boss_State.FarAttack);
        }
    }
}
