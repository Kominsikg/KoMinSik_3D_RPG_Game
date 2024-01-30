using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDragon : Monster, IHit
{
    int EXP = 0;
    Vector3 Attackpos = new Vector3(0, 0.5f, 1f);
    Vector3 Attacksize = new Vector3(2f, 1.5f, 2f);
    public Transform AttackTr;
    [SerializeField] Transform Damagepos;
    protected override void Start()
    {
        base.Start();
        curstate = AllEnum.State.Idle;
        if (TargetTr == null)
            TargetTr = PlayerManager.Instance.transform;
        E_Stat = new AllStruct.Enemy_Stat(7, 250, 200, 2000);
        monsternum = 3;
    }

    void Update()
    {
        if (isAlive == false)
        {
            return;
        }
        switch (curstate)
        {
            case AllEnum.State.Idle:
                Idle();
                break;
            case AllEnum.State.Patrol:
                Patrol(this);
                break;
            case AllEnum.State.Chase:
                Chase(TargetTr.position);
                break;
            case AllEnum.State.NearAttack:
                Attack();
                break;
            case AllEnum.State.FarAttack:
                FarAttack();
                break;
            default:
                break;
        }

        fsm.UpdateState();
    }

    public void MiniDragonNearAttack()
    {
        Vector3 attpos = transform.forward * Attackpos.z;
        attpos *= Attackpos.y;
        Collider[] col = Physics.OverlapBox(transform.position + attpos, Attacksize, Quaternion.identity, playerlayer);
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            {hit.Hit(E_Stat.Att);}
        }
    }

    public void  MiniDragonFarAttack()
    {                      
        GameManager.Instance.GetFireBall().Init(AttackTr, E_Stat.Att);
    }

    public void Hit(float damage)
    {
        if (isAlive == false)
        { return;}      
        UIManager.Instance.MonsterInfo(false);
        bonusDamage = damage * ((PlayerManager.Instance.S_Stat.Level - E_Stat.Level) * 0.1f);
        totalDamamge = Mathf.Max(1, (damage - E_Stat.Def + bonusDamage));
        E_Stat.HP = Mathf.Max(0, E_Stat.HP - totalDamamge);
        if (totalDamamge >= 500)
        { anim.Damage(); }
        UIManager.Instance.Damage(totalDamamge, Damagepos.position);
        UIManager.Instance.MonsterInfo(true);
        UIManager.Instance.monsterHPbar.fillAmount = E_Stat.HP / E_Stat.MaxHP;
        UIManager.Instance.monsterInfo(E_Stat.Level, "¹Ì´Ï µå·¡°ï", E_Stat.HP, E_Stat.MaxHP);
        if (E_Stat.HP <= 0)
        {StartCoroutine(Dead());}       
    }

    IEnumerator Dead()
    {
        isAlive = false;
        anim.Dead();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        EXP = Random.Range(50, 66);
        PlayerManager.Instance.IncreaseExperience(EXP);
        yield return new WaitForSeconds(5f);
        float probability = Random.Range(0f, 1f);
        if (probability >= 0.8f)
        { GameManager.Instance.GetHPpotion().Init(transform); }
        else if (probability <= 0.2f)
        { GameManager.Instance.GetMPpotion().Init(transform); }
        minidragonzen();
        GameManager.Instance.ReturnMiniDragon(this);
    }

    void minidragonzen()
    {
        isAlive = true;
        E_Stat.HP = E_Stat.MaxHP;
        int randompoint = Random.Range(0, GameManager.Instance.minidragonspwnpoint.Length);
        transform.position = GameManager.Instance.minidragonspwnpoint[randompoint].position;
        anim.Revival();
        curstate = AllEnum.State.Idle;
    }
}



