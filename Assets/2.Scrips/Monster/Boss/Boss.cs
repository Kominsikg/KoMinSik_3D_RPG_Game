using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    protected float distance;
    protected Boss_Anim anim;
    protected NavMeshAgent agent;
    protected FSM fsm;
    public AllEnum.Boss_State curstate;
    public AllStruct.Enemy_Stat B_Stat;
    protected float bonusDamage = 0;
    protected float totalDamamge = 0;
    protected float probability = 0;
    protected int playerlayer;
    protected bool isAlive = true;
    public int bossNum = 0;
    Coroutine cor = null;      
    
    protected virtual void Start()
    {
        playerlayer = 1 << LayerMask.NameToLayer("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Boss_Anim>();
        fsm = new FSM(new Boss_Idle(this));
        isAlive = true;
    }
    public void ChangeState(AllEnum.Boss_State state)
    {
        curstate = state;
                    
        switch (curstate)
        {
            case AllEnum.Boss_State.Idle:
                fsm.ChangeState(new Boss_Idle(this));
                break;
            case AllEnum.Boss_State.Chase:
                fsm.ChangeState(new Boss_Chase(this));
                break;
            case AllEnum.Boss_State.Scream:
                fsm.ChangeState(new Boss_Scream(this));
                break;
            case AllEnum.Boss_State.NearAttack:
                fsm.ChangeState(new Boss_NearAttack(this));
                break;
            case AllEnum.Boss_State.FarAttack:
                fsm.ChangeState(new Boss_FarAttack(this));
                break;                                  
            default:
                break;
        }       
    }
    public void Idle()
    {        
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        if (cor == null)
        {cor =  StartCoroutine(Healing());}       
    }
    public void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(PlayerManager.Instance.transform.position);
        anim.Move(true);
    }
    IEnumerator Healing()
    {
       if (isAlive && B_Stat.HP < B_Stat.MaxHP)
       {                        
            B_Stat.HP += 200;
            B_Stat.HP = Mathf.Min(B_Stat.MaxHP, B_Stat.HP);
            yield return new WaitForSeconds(5f);            
            cor = null;
       }        
    }
    
    public void NearAttack()
    {
        if (PlayerManager.Instance.S_Stat.HP <= 0)
        {return;}
        agent.isStopped = true;
        anim.Attack();        
    }
    public void FarAttack()
    {
        if (PlayerManager.Instance.S_Stat.HP <= 0)
        { return; }       
        agent.isStopped = true;        
        if (cor == null)
        {cor = StartCoroutine(FarAttackDelay());}        
    }
    IEnumerator FarAttackDelay()
    {
        anim.SkillAttack();
        yield return new WaitForSeconds(2f);
        cor = null;
    }
    public void Scream()
    {        
        if (PlayerManager.Instance.S_Stat.HP <= 0)
        { return; }
        anim.Scream();       
    }
    
    public bool CanSeePlayer()
    {
        return PlayerManager.Instance.transform.position.x <= -350f;
    }
    public bool CanAttackNear()
    {
        distance = Vector3.Distance(PlayerManager.Instance.transform.position, transform.position);
        return distance <= 10f;        
    }
    
}
