using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Two_Phase : Boss,IHit
{
    public GameObject One_Phase;
    public GameObject Flame;
    Vector3 Attackpos = Vector3.zero;
    Vector3 Attacksize = Vector3.zero;
    [SerializeField] Transform Damagepos;
    public GameObject firePrefab;
    [SerializeField] GameObject[] ReturnButton;
    int maxFireCount = 20;   
    float fireSpawnInterval = 2f;
    bool explnotice = false;
    bool firenotice = false;

    private List<GameObject> fireObjects = new List<GameObject>();
    protected override void Start()
    {
        base.Start();        
        B_Stat = new AllStruct.Enemy_Stat(10, 500, 400, 30000);
        curstate = AllEnum.Boss_State.Idle;
        Flame.SetActive(false);        
        InitializeFireObjects();
        bossNum = 2;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            LookUI();
            if (isAlive == false)
            { return; }
            transform.LookAt(PlayerManager.Instance.transform, Vector3.up);
        }
        if (PlayerManager.Instance.S_Stat.HP <= 0)
        {
            firenotice = false;
            explnotice = false;
            Flame.SetActive(false);
            B_Stat.HP = B_Stat.MaxHP;
            One_Phase.SetActive(true);
            isAlive = true;
            gameObject.SetActive(false);
        }                            
        if (explnotice == false && B_Stat.HP <= 15000)
        {
            explnotice = true;
            UIManager.Instance.Notice("레드 드래곤이 폭주합니다.");
        }

        switch (curstate)
        {
            case AllEnum.Boss_State.Idle:
                Idle();
                break;
            case AllEnum.Boss_State.Chase:
                Chase();
                break;           
            case AllEnum.Boss_State.NearAttack:
                NearAttack();
                break;
            case AllEnum.Boss_State.FarAttack:
                FarAttack();
                anim.Move(false);
                break;
            default:
                break;
        }
        fsm.UpdateState();
        if (B_Stat.HP <= 7000 && B_Stat.HP > 0)
        {
            if (firenotice == false)
            {
                UIManager.Instance.Notice("레드 드래곤 주위에 불길이 생성됩니다.");
                firenotice = true;
            }
            SpawnFire();
        }


    }
    private void InitializeFireObjects()
    {
        for (int i = 0; i < maxFireCount; i++)
        {
            GameObject fireObject = Instantiate(firePrefab, Vector3.zero, Quaternion.identity);
            fireObject.SetActive(false);
            fireObjects.Add(fireObject);
        }
    }
    private void SpawnFire()
    {
        StartCoroutine(SpawnFireCoroutine());
    }
    private IEnumerator SpawnFireCoroutine()
    {
        for (int i = 0; i < maxFireCount; i++)
        {
            GameObject fireObject = GetInactiveFireObject();
            if (fireObject != null)
            {
                           
                Vector3 randomPosition = new Vector3(Random.Range(-420f,-360), 0f, Random.Range(-25f, 25f));
                Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                fireObject.transform.position = randomPosition;
                fireObject.transform.rotation = randomRotation;

                fireObject.SetActive(true);
            }

            yield return new WaitForSeconds(fireSpawnInterval);
        }
    }

    
    private GameObject GetInactiveFireObject()
    {
        foreach (GameObject fireObject in fireObjects)
        {
            if (!fireObject.activeInHierarchy)
            {
                return fireObject;
            }
        }
        return null;
    }
    public void DragonNearAttack()
    {
        Attackpos = new Vector3(0, 2f, 4f);
        Attacksize = new Vector3(3f, 2f, 3f);
        Vector3 attpos = transform.forward * Attackpos.z;
        attpos *= Attackpos.y;
        Collider[] col = Physics.OverlapBox(transform.position + attpos, Attacksize, Quaternion.identity, playerlayer);
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(B_Stat.Att); }
        }
        if (B_Stat.HP <= 15000)
        { GameManager.Instance.GetExplosion().Init(gameObject.transform, 300);}
    }
    public void DragonNearAttackTwo()
    {
        Attackpos = new Vector3(0, 2f, 4f);
        Attacksize = new Vector3(4f, 2f, 12f);
        Vector3 attpos = transform.forward * Attackpos.z;
        attpos *= Attackpos.y;
        Collider[] col = Physics.OverlapBox(transform.position + attpos, Attacksize, Quaternion.identity, playerlayer);
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(B_Stat.Att * 1.5f); }
        }
        if (B_Stat.HP <= 15000)
        {GameManager.Instance.GetExplosion().Init(gameObject.transform, 300);}
    }
    
    public void DragonFarAttackOn()
    {
        Flame.SetActive(true);
        //Debug.Log("불on");
    }
    public void DragonFarAttackOff()
    {
        Flame.SetActive(false);
       // Debug.Log("불 off");
    }
   
    public void Hit(float damage)
    {
        if (isAlive == false)
        { return; }
        bonusDamage = damage * ((PlayerManager.Instance.S_Stat.Level - B_Stat.Level) * 0.1f);
        totalDamamge = Mathf.Max(1, (damage - B_Stat.Def + bonusDamage));
        B_Stat.HP = Mathf.Max(0, B_Stat.HP - totalDamamge);
        UIManager.Instance.Damage(totalDamamge, Damagepos.position);
        if (totalDamamge >= 1000)
        { anim.Hit(); }       
        if (B_Stat.HP <= 0)
        { StartCoroutine(Dead()); }

    }
    IEnumerator Dead()
    {
        isAlive = false;
        anim.Die();
        StopCoroutine(SpawnFireCoroutine());
        UIManager.Instance.Notice("레드 드래곤을 해치웠습니다.");        
        yield return new WaitForSeconds(10f);
        UIManager.Instance.Notice("마을로 돌아가시겠습니까?");
        for (int i = 0; i < ReturnButton.Length; i++)
        {ReturnButton[i].SetActive(true);}              
        gameObject.SetActive(false);                          
    }
    void LookUI()
    {
        UIManager.Instance.MonsterInfo(true);
        B_Stat.HP = Mathf.Clamp(B_Stat.HP, 0, B_Stat.MaxHP);
        UIManager.Instance.monsterHPbar.fillAmount = B_Stat.HP / B_Stat.MaxHP;
        UIManager.Instance.monsterInfo(B_Stat.Level, "해방된 레드 드래곤", B_Stat.HP, B_Stat.MaxHP);
    }
}

