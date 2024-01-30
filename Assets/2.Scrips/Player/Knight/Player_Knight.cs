using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player_Knight : MonoBehaviour
{    
    bool isJump = false;
    Animator anim;
    float speed = 0;
    float WalkSpeed = 0.5f;
    float RunSpeed = 1f;
    public AllStruct.individual_Stat Knight_Stat;    
    [SerializeField] GameObject Magicshield;
    [SerializeField] Text[] cooltexts;
    [SerializeField] Image[] ShowCool;
    [SerializeField] GameObject status;
    // 각 스킬에 대한 쿨다운 타이머
    private float AttcooldownTimer = 0f;
    private float skill1CooldownTimer = 0f;
    private float skill2CooldownTimer = 0f;
    private float skill3CooldownTimer = 0f;
    bool isAlive = true;
    Vector3 moveVec = Vector3.zero;   
    public float Def  = 0; // 현재 방어력
    public float Att  = 0; // 현재 공격력
    Collider[] col;
    public LayerMask enemyLayer;
    public int StatPoint = 0;    
    Vector3 Attackpos = new Vector3(0,1f,1.2f);
    Vector3 Attacksize = new Vector3(1.5f,1.5f,2f);
   
    void Start()
    {        
        Knight_Stat = new AllStruct.individual_Stat(40,60);        
        Def = Knight_Stat.Def;
        Att = Knight_Stat.Att;
        anim = GetComponent<Animator>();         
        Magicshield.SetActive(false);       
    }

    void Update()
    {
        if (isAlive == false)
        { return; }    
        col = Physics.OverlapBox(transform.position + Attackpos, Attacksize, Quaternion.identity, enemyLayer);
        if (Input.GetKeyDown(KeyCode.Space)) //스페이스 점프
        {
            if (isJump) //점프는 한번만
            { return; }
            isJump = true;
        }
       
        if (PlayerManager.Instance.x == 0 && PlayerManager.Instance.z == 0) //멈춤
        { speed = 0; }
        else // 걷기
        {
            if (Input.GetKey(KeyCode.LeftShift)) //시프트 누르면 달리기           
            { speed = RunSpeed; }
            else
            { speed = WalkSpeed; }

        }
        anim.SetFloat("Move", speed);

        AttcooldownTimer = Mathf.Max(0f, AttcooldownTimer - Time.deltaTime);
        skill1CooldownTimer = Mathf.Max(0f, skill1CooldownTimer - Time.deltaTime);
        skill2CooldownTimer = Mathf.Max(0f, skill2CooldownTimer - Time.deltaTime);
        skill3CooldownTimer = Mathf.Max(0f, skill3CooldownTimer - Time.deltaTime);

        if (isJump == false)
        {
            if (Cursor.visible == true)
            { return; }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X))
            {
                if (status.activeSelf == true)
                { return; }
                if (AttcooldownTimer <= 0f)
                {
                    anim.SetTrigger("Attack");
                    AttcooldownTimer = 1f; // 1초 쿨다운
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (status.activeSelf == true)
                { return; }
                if (PlayerManager.Instance.S_Stat.Level >= 3)
                {
                    if (skill2CooldownTimer <= 0f)
                    {
                        if (PlayerManager.Instance.S_Stat.MP < 10)
                        {
                            UIManager.Instance.Notice("MP가 부족하여 스킬을 사용할 수 없습니다");
                            return; 
                        }
                        PlayerManager.Instance.ConsumeMP(10);
                        anim.SetTrigger("SkillAttack1");
                        StartCoroutine(Skill1Casting());
                        skill2CooldownTimer = 20f; // 20초로 바꿀 예정
                    }
                    else
                    { UIManager.Instance.Notice($"쿨타임입니다.(남은시간 : {(int)skill2CooldownTimer}초)"); }
                }
                else
                { UIManager.Instance.Notice("현재 사용할 수 없는 레벨입니다 ."); }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (status.activeSelf == true)
                { return; }
                if (skill1CooldownTimer <= 0f)
                {

                    if (PlayerManager.Instance.S_Stat.MP < 20)
                    {
                        UIManager.Instance.Notice("MP가 부족하여 스킬을 사용할 수 없습니다");
                        return;
                    }
                    PlayerManager.Instance.ConsumeMP(20);
                    anim.SetTrigger("SkillAttack2");
                    skill1CooldownTimer = 5f; // 5초로 바꿀 예정
                }
                else
                { UIManager.Instance.Notice($"쿨타임입니다.(남은시간 : {(int)skill1CooldownTimer}초)"); }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (status.activeSelf == true)
                { return; }
                if (PlayerManager.Instance.S_Stat.Level >= 6)
                {
                    if (skill3CooldownTimer <= 0f)
                    {
                        if (PlayerManager.Instance.S_Stat.MP < 30)
                        {
                            UIManager.Instance.Notice("MP가 부족하여 스킬을 사용할 수 없습니다");
                            return; 
                        }
                        PlayerManager.Instance.ConsumeMP(30);
                        anim.SetTrigger("SkillAttack3");
                        skill3CooldownTimer = 15f; // 15초로 바꿀 예정
                    }
                    else
                    { UIManager.Instance.Notice($"쿨타임입니다.(남은시간 :{(int)skill3CooldownTimer}초)"); }
                }
                else
                { UIManager.Instance.Notice("현재 사용할 수 없는 레벨입니다 ."); }
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Def = Knight_Stat.Def;
            Att = Knight_Stat.Att;
        }
        ShowCoolTime();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        { isJump = false; }
    }
    
    IEnumerator Skill1Casting()
    {
        Magicshield.SetActive(true);
        Def = Knight_Stat.Def * 2; // 버프 시간 동안 방어력 두배
        //Debug.Log("현재 방어력 : " + Def);
        yield return new WaitForSeconds(10f);
        Magicshield.SetActive(false);
        Def *= 0.5f;// 10초후 방어력 원상복구 
        //Debug.Log("현재 방어력 : " + Def);
    }
   
    //아래는 근접 공격 되는지 확인만 한것 차후 몬스터에게 데미지 추가 예정
    public void Attack() 
    {
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(Knight_Stat.Att); }           
        }
    }
    public void Skill2Casting()
    {
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(Knight_Stat.Att * 1.5f); }
        }
    }
    public void Skill3Casting()
    {
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(Knight_Stat.Att * 2f); }
        }
    }
    public void Skill3Casting2()
    {
        for (int i = 0; i < col.Length; i++)
        {
            IHit hit = col[i].GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(Knight_Stat.Att * 3f); }
        }
    }
    public void Die() //캐릭터에서는 애니메이션만 출현
    {
        anim.SetTrigger("Die");
        isAlive = false;
    }
    public void Revival()
    {
        anim.SetTrigger("Revival");
        isAlive = true;
    }
    public void HitAnim()
    {
        anim.SetTrigger("Hit");
    }
    public void ShowCoolTime()
    {
        cooltexts[0].text = $"{(int)skill1CooldownTimer}";
        ShowCool[0].fillAmount = skill1CooldownTimer / 5;
        if (skill1CooldownTimer <= 0)
        {cooltexts[0].gameObject.SetActive(false);}
        else
        {cooltexts[0].gameObject.SetActive(true);}

        cooltexts[1].text = $"{(int)skill2CooldownTimer}";
        ShowCool[1].fillAmount = skill2CooldownTimer / 20;
        if (skill2CooldownTimer <= 0)
        { cooltexts[1].gameObject.SetActive(false); }
        else
        { cooltexts[1].gameObject.SetActive(true); }

        cooltexts[2].text = $"{(int)skill3CooldownTimer}";
        ShowCool[2].fillAmount = skill3CooldownTimer / 15;
        if (skill3CooldownTimer <= 0)
        { cooltexts[2].gameObject.SetActive(false); }
        else
        { cooltexts[2].gameObject.SetActive(true); }
    }
}
