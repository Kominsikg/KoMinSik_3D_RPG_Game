using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Monster, IHit
{
    public Material[] Skin;
    int skinnum =0;
    int EXP = 0;
    int num = 0;
    Vector3 Attackpos = new Vector3(0, 1.5f, 3f);
    Vector3 Attacksize = new Vector3(2f, 1.5f, 3f);
    public Transform Damagepos;
    
    protected override void Start()
    {
        base.Start();
        if (TargetTr == null)
            TargetTr = PlayerManager.Instance.transform;
        CreateMushroom();
        monsternum = 1;
    }

    void Update()
    {   
        if (isAlive == false)
        {return;}
        
        switch (curstate)
        {
            case AllEnum.State.Idle:
                Idle();
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

    public void MushroomAttack()
    {
        Vector3 attPos = transform.forward * Attackpos.z;
        attPos.y += Attackpos.y;
        Collider[] col = Physics.OverlapBox(transform.position + attPos, Attacksize, Quaternion.identity,playerlayer);        
       
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();          
            if (hit != null)
            { hit.Hit(E_Stat.Att);}
        }
    }

    public void Hit(float damage)
    {        
        if (isAlive == false)
        { return;}                      
        UIManager.Instance.MonsterInfo(false);
        bonusDamage = damage * ((PlayerManager.Instance.S_Stat.Level - E_Stat.Level) * 0.1f);
        totalDamamge = Mathf.Max(1, (damage - E_Stat.Def + bonusDamage));
        E_Stat.HP = Mathf.Max(0, E_Stat.HP - totalDamamge);
        UIManager.Instance.Damage(totalDamamge,Damagepos.position);
        if (totalDamamge >= 200)
        { anim.Damage(); }
        UIManager.Instance.monsterHP.SetActive(false);
        UIManager.Instance.MonsterInfo(true);
        UIManager.Instance.monsterHPbar.fillAmount = E_Stat.HP / E_Stat.MaxHP;
        if(skinnum == 0)
        { UIManager.Instance.monsterInfo(E_Stat.Level, "ºí·ç ¸Ó½¬·ë", E_Stat.HP, E_Stat.MaxHP); }
        else if (skinnum == 1)
        { UIManager.Instance.monsterInfo(E_Stat.Level, "±×¸° ¸Ó½¬·ë", E_Stat.HP, E_Stat.MaxHP); }
        else if (skinnum == 2)
        { UIManager.Instance.monsterInfo(E_Stat.Level, "·¹µå ¸Ó½¬·ë", E_Stat.HP, E_Stat.MaxHP); }
        if (E_Stat.HP <= 0)
        {StartCoroutine(Dead());}        
    }
   
    IEnumerator Dead()
    {
        isAlive = false;
        anim.Dead();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        if (skinnum == 0)
        { EXP = Random.Range(15, 26); }
        else if (skinnum == 1)
        { EXP = Random.Range(20, 31); }
        else
        { 
            EXP = Random.Range(25, 36);
            if (PlayerManager.Instance.S_Stat.Level == 1)
            { EXP = 100; }
        }
        if (PlayerManager.Instance.S_Stat.Level >= 5)
        { EXP = 20; }        
        PlayerManager.Instance.IncreaseExperience(EXP);
        yield return new WaitForSeconds(5f);
        float probability = Random.Range(0f, 1f);       
        if (probability >= 0.8f)
        { GameManager.Instance.GetHPpotion().Init(transform); }
        else if (probability <= 0.2f)
        { GameManager.Instance.GetMPpotion().Init(transform); }
        mushroomzen();
        GameManager.Instance.Returnmushroom(this);       
    }
    void mushroomzen()
    {
        isAlive = true;        
        CreateMushroom();
        float randomposionx = (Random.Range(67f,132f)) * -1;
        float randomposionz = Random.Range(-25f, 25f);
        transform.position = new Vector3(randomposionx, 0, randomposionz);              
    }
    void CreateMushroom()
    {
        curstate = AllEnum.State.Idle;
        anim.Move(false);
        num = Random.Range(0, 10);
        if (num >= 0 && num <= 4)
        {
            E_Stat = new AllStruct.Enemy_Stat(1, 60, 0, 500);
            skinnum = 0;
        }
        else if (num >= 5 && num <= 7)
        {
            E_Stat = new AllStruct.Enemy_Stat(2, 70, 20, 700);
            skinnum = 1;
        }
        else
        {
            E_Stat = new AllStruct.Enemy_Stat(3, 90, 40, 1000);
            skinnum = 2;
        }
        transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = Skin[skinnum];
    }



}
