using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform TargetTr;
    protected float distance;
    protected Monstar_anim anim;
    protected NavMeshAgent agent;
    protected FSM fsm;
    public AllEnum.State curstate;    
    public AllStruct.Enemy_Stat E_Stat;
    protected float bonusDamage = 0;
    protected float totalDamamge = 0;
    protected float probability = 0;
    protected int playerlayer;
    protected bool isAlive = true;
    public int monsternum = 0;
    public Vector3 targetPatrolPoint;
    Coroutine cor = null;    

    State state;
    protected virtual void Start()
    {
        playerlayer = 1 << LayerMask.NameToLayer("Player");        
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Monstar_anim>();
        fsm = new FSM(new IdleState(this));
        isAlive = true;
    }  

    public void Idle()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        anim.Move(false);
    }

    public void Chase(Vector3 vec)
    {
        agent.isStopped = false;
        agent.SetDestination(vec);
        anim.Move(true);
    }

    public void Attack()
    {
        if (PlayerManager.Instance.S_Stat.HP <= 0)
        {return;}
        agent.isStopped = true;
        transform.LookAt(TargetTr,Vector3.up);
        anim.Attack();
    }
    public void FarAttack()
    {
        if (PlayerManager.Instance.S_Stat.HP <= 0)
        { return; }
        agent.isStopped = true;
        transform.LookAt(TargetTr, Vector3.up);
        anim.SkillAttack();
    }
    public void Patrol(Monster monster)
    {          
        if (cor == null)
        { cor = StartCoroutine(PatrolTime(monster)); }        
        monster.Chase(targetPatrolPoint); 
    }
    IEnumerator PatrolTime(Monster monster)
    {        
        yield return new WaitForSeconds(5f);
        if (monster is MiniDragon)
        { targetPatrolPoint = GameManager.Instance.minidragonspwnpoint[Random.Range(0, GameManager.Instance.minidragonspwnpoint.Length)].position; }
        else if (monster is StoneMonster)
        { targetPatrolPoint = GameManager.Instance.stonemonsterspwnpoint[Random.Range(0, GameManager.Instance.stonemonsterspwnpoint.Length)].position; }
        cor = null;
    }
    public void ChangeState(AllEnum.State nextState)
    {        
        curstate = nextState;

        State newState = null;
        switch (curstate)
        {
            case AllEnum.State.Idle:
                newState = new IdleState(this);
                break;
            case AllEnum.State.Patrol:
                newState = new PatrolState(this);
                break;
            case AllEnum.State.Chase:
                newState = new ChaseState(this);
                break;
            case AllEnum.State.NearAttack:
                newState = new AttackNearState(this);
                break;
            case AllEnum.State.FarAttack:
                newState = new AttackFarState(this);
                break;
            default:
                break;
        }

        if (newState != null)
        {fsm.ChangeState(newState);}
    }
    public bool CanSeePlayer()
    {
        distance = Vector3.Distance(transform.position, TargetTr.position);
        return distance <= 15f;
    }

    public bool CanNearAttackPlayer()
    {
        distance = Vector3.Distance(transform.position, TargetTr.position);
        return distance <= 3f;
    }
    public bool CanFarAttackPlayer()
    {
        distance = Vector3.Distance(transform.position, TargetTr.position);
        return distance > 3f && distance <= 10f;
    }
    
    
}
