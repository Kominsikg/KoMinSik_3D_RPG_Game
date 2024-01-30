using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class one_Phase : Boss,IHit
{
    public Transform AttackTr;
    public GameObject Next_Phase;    
    Vector3 Attackpos = new Vector3(0, 1.5f, 5f);
    Vector3 Attacksize = new Vector3(4.5f, 3f, 4f);   
    bool scream = false;
    [SerializeField] Transform Damagepos;
    protected override void  Start()
    {
        base.Start();
        B_Stat = new AllStruct.Enemy_Stat(9,400,400,20000);        
        curstate = AllEnum.Boss_State.Idle;
        Next_Phase.SetActive(false);
        bossNum = 1;
    }

    void Update()
    {
        if (PlayerManager.Instance.S_Stat.HP <= 0)
        {
            B_Stat.HP = B_Stat.MaxHP;
            isAlive = true;
        }
        if (CanSeePlayer())
        {
            LookUI();
            if (isAlive == false)
            { return; }
            transform.LookAt(PlayerManager.Instance.transform, Vector3.up);
        }
        if (scream == false && B_Stat.HP <= 13000)
        {
            scream = true;
            UIManager.Instance.Notice("���� �巡���� �����մϴ�.");
        }

        switch (curstate)
        {
            case AllEnum.Boss_State.Idle:
                Idle();                               
                break;
            case AllEnum.Boss_State.Scream:
                Scream();
                break;
            case AllEnum.Boss_State.NearAttack:
                NearAttack();                
                break;
            case AllEnum.Boss_State.FarAttack:
                FarAttack();                
                break;
            default:
                break;
        }
        fsm.UpdateState();


    }
    public void DragonNearAttack()
    {
        Vector3 attpos = transform.forward * Attackpos.z;
        attpos *= Attackpos.y;
        Collider[] col = Physics.OverlapBox(transform.position + attpos, Attacksize, Quaternion.identity, playerlayer);
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(B_Stat.Att); }
        }
    }
    public void DragonFarAttack()
    {
        GameManager.Instance.GetBossFireBall().Init(AttackTr, B_Stat.Att);
    }
    public void Explosion()
    {
        GameManager.Instance.GetExplosion().Init(gameObject.transform, 500);
    }

    public void Hit(float damage)
    {
        if (isAlive == false)
        { return; }                
        bonusDamage = damage * ((PlayerManager.Instance.S_Stat.Level - B_Stat.Level) * 0.1f);
        totalDamamge = Mathf.Max(1, (damage - B_Stat.Def + bonusDamage));
        UIManager.Instance.Damage(totalDamamge, Damagepos.position);
        B_Stat.HP = Mathf.Max(0,B_Stat.HP - totalDamamge);
        if (totalDamamge >= 1000)
        { anim.Hit(); }        
        if (B_Stat.HP <= 0)
        {StartCoroutine(Dead());}

    }
    IEnumerator Dead()
    {
        isAlive = false;        
        anim.Die();       
        yield return new WaitForSeconds(5f);
        UIManager.Instance.Notice("���� �巡���� ������ ���� �Ǿ����ϴ�.");
        PlayerManager.Instance.transform.position = new Vector3(-360,0,0);
        gameObject.SetActive(false);        
        Next_Phase.SetActive(true);
    }
    void LookUI()
    {       
        UIManager.Instance.MonsterInfo(true);
        B_Stat.HP = Mathf.Clamp(B_Stat.HP, 0, B_Stat.MaxHP);
        UIManager.Instance.monsterHPbar.fillAmount = B_Stat.HP / B_Stat.MaxHP;
        UIManager.Instance.monsterInfo(B_Stat.Level, "���ε� ���� �巡��", B_Stat.HP, B_Stat.MaxHP);
    }
}
