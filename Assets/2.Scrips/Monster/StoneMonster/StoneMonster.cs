using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMonster : Monster,IHit
{

    Vector3 Attackpos = new Vector3(0, 1f, 1f);
    Vector3 Attacksize = new Vector3(2f, 1.5f, 2f);
    [SerializeField] Transform Damagepos;
    int EXP = 0;    
    protected override void Start()
    {
        base.Start();
        if(TargetTr == null)
        TargetTr = PlayerManager.Instance.transform;
        curstate = AllEnum.State.Idle;        
        E_Stat = new AllStruct.Enemy_Stat(5, 200, 150, 1500);
        monsternum = 2;
    }

    void Update()
    {
        if (isAlive == false)
        { return; }

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
            default:
                break;
        }
        fsm.UpdateState();
    }

    public void StoneMonsterAttack()
    {
        Vector3 attpos = transform.forward * Attackpos.z;
        attpos *= Attackpos.y;
        Collider[] col = Physics.OverlapBox(transform.position + attpos, Attacksize, Quaternion.identity, playerlayer);
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(E_Stat.Att); }
        }
    }

    public void Hit(float damage)
    {
        if (isAlive == false)
        { return; }                 
        UIManager.Instance.MonsterInfo(false);
        bonusDamage = damage * ((PlayerManager.Instance.S_Stat.Level - E_Stat.Level) * 0.1f);        
        totalDamamge = Mathf.Max(1, (damage - E_Stat.Def + bonusDamage));       
        E_Stat.HP = Mathf.Max(0, E_Stat.HP - totalDamamge);
        if (totalDamamge >= 300)
        {anim.Damage();}
        UIManager.Instance.Damage(totalDamamge, Damagepos.position);
        UIManager.Instance.MonsterInfo(true);
        UIManager.Instance.monsterHPbar.fillAmount = E_Stat.HP / E_Stat.MaxHP;
        UIManager.Instance.monsterInfo(E_Stat.Level, "ÆÄÀÌ¾î ½ºÅæ", E_Stat.HP, E_Stat.MaxHP);
        if (E_Stat.HP <= 0)
        { StartCoroutine(Dead()); }        
    }

    IEnumerator Dead()
    {
        isAlive = false;
        anim.Dead();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;                
        if (PlayerManager.Instance.S_Stat.Level >= 7)
        { EXP = 24; }
        else
        {EXP = Random.Range(40, 56);}
        PlayerManager.Instance.IncreaseExperience(EXP);
        yield return new WaitForSeconds(5f);
        float probability = Random.Range(0f, 1f);
        if (probability >= 0.8f)
        { GameManager.Instance.GetHPpotion().Init(transform); }
        else if (probability <= 0.2f)
        { GameManager.Instance.GetMPpotion().Init(transform); }
        stonemonsterzen();
        GameManager.Instance.ReturnStonemonster(this);
    }
    void stonemonsterzen()
    {
        isAlive = true;
        E_Stat.HP = E_Stat.MaxHP;
        int randompoint = Random.Range(0, GameManager.Instance.stonemonsterspwnpoint.Length);
        transform.position = GameManager.Instance.stonemonsterspwnpoint[randompoint].position;
        anim.Revival();
        curstate = AllEnum.State.Idle;
    }


}
