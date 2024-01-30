using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Wizard : MonoBehaviour
{
    bool isJump = false;
    Animator anim;
    [SerializeField] Transform attTr;
    [SerializeField] Transform skillZoneTr;
    [SerializeField] GameObject SkillCasting;
    [SerializeField] Text[] cooltexts;
    [SerializeField] Image[] ShowCool;
    [SerializeField] GameObject Status;  

    float speed = 0;    
    bool isAttack = false; //공격 딜레이를 주기 위함
    bool isAlive = true;

    public AllStruct.individual_Stat Wizard_Stat;
    Vector3 moveVec = Vector3.zero;

    // 각 스킬에 대한 쿨다운 타이머
    private float AttcooldownTimer = 0f;
    private float skill1CooldownTimer = 0f;
    private float skill2CooldownTimer = 0f;
    private float skill3CooldownTimer = 0f;

    public float Att = 0;
    public float Def = 0;
    public int StatPoint = 0;
    void Start()
    {
        isAlive = true;
        Wizard_Stat = new AllStruct.individual_Stat(70, 30);
        Att = Wizard_Stat.Att;
        Def = Wizard_Stat.Def;
        anim = GetComponent<Animator>();
       
    }
    void Update()
    {   
        if (isAlive == false)
        { return; }       
        Att = Wizard_Stat.Att;
        Def = Wizard_Stat.Def;
        if (Input.GetKeyDown(KeyCode.Space)) //스페이스 점프
        {
            if (isJump) //점프는 한번만
            { return; }
            isJump = true;
            anim.SetBool("isJump", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        { anim.SetBool("isJump", false); }

        if (PlayerManager.Instance.x == 0 && PlayerManager.Instance.z == 0) //멈춤
        { speed = 0; }
        else // 걷기
        {
            if (Input.GetKey(KeyCode.LeftShift)) //시프트 누르면 달리기           
            { speed = PlayerManager.Instance.sprintSpeed; }
            else
            { speed = PlayerManager.Instance.normalSpeed; }

            anim.SetFloat("PosX", moveVec.x);
            anim.SetFloat("PosZ", moveVec.z);
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
                if (isAttack == false)
                {StartCoroutine(Attack());}
            }
            else if (Input.GetMouseButtonDown(1)) // 마우스 우클릭으로 스킬1 사용
            {
                if (skill1CooldownTimer <= 0)
                {                    
                    if (PlayerManager.Instance.S_Stat.MP < 10f)
                    {
                        UIManager.Instance.Notice("MP가 부족하여 스킬을 사용할 수 없습니다");
                        return; 
                    }
                    PlayerManager.Instance.ConsumeMP(10);
                    Skill1Casting();
                    skill1CooldownTimer = 5f;                                     
                }
                else
                { UIManager.Instance.Notice($"쿨타임입니다.(남은시간 : {(int)skill1CooldownTimer}초)");}               
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // Q 키로 스킬2 사용
            {
                if (PlayerManager.Instance.S_Stat.Level >= 3)
                {
                    if (skill2CooldownTimer <= 0)
                    {
                        if (PlayerManager.Instance.S_Stat.MP < 20f)
                        {
                            UIManager.Instance.Notice("MP가 부족하여 스킬을 사용할 수 없습니다");
                            return;
                        }
                        PlayerManager.Instance.ConsumeMP(20);
                        skill2CooldownTimer = 10f;
                        StartCoroutine(Skill2Casting());
                    }
                    else
                    { UIManager.Instance.Notice($"쿨타임입니다.(남은시간 {(int)skill2CooldownTimer}초)"); }
                }
                else
                { UIManager.Instance.Notice("현재 사용할 수 없는 레벨입니다 ."); }
            }
            else if (Input.GetKeyDown(KeyCode.E)) // E 키로 스킬3 사용
            {
                if (PlayerManager.Instance.S_Stat.Level >= 6)
                {
                    if (skill3CooldownTimer <= 0)
                    {
                        if (PlayerManager.Instance.S_Stat.MP < 30f)
                        {
                            UIManager.Instance.Notice("MP가 부족하여 스킬을 사용할 수 없습니다");
                            return; 
                        }
                        PlayerManager.Instance.ConsumeMP(30);
                        skill3CooldownTimer = 20f;
                        Skill3Casting();
                    }
                    else
                    { UIManager.Instance.Notice($"쿨타임입니다.(남은시간 : {(int)skill3CooldownTimer}초 )"); }
                }
                else
                { UIManager.Instance.Notice("현재 사용할 수 없는 레벨입니다 ."); }
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab)) //마법사 활성화했을때 공격 초기화
        { isAttack = false; }
        ShowCoolTime();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        { isJump = false; }
    }

    IEnumerator Attack()
    {
        isAttack = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f); // 공격 준비 시간        
        GameManager.Instance.GetMagicBall().Init(attTr, Att);        
        yield return new WaitForSeconds(0.5f); // 공격 후 딜레이시간        
        isAttack = false;
    }

    void Skill1Casting()
    {
        anim.SetTrigger("SkillAttack1");
        W_SkillManager.Instance.GetSkill1().Init(attTr,Att);       
    }

    IEnumerator Skill2Casting()
    {
        anim.SetTrigger("SkillAttack2");
        SkillCasting.SetActive(true);
        yield return new WaitForSeconds(1f); // 스킬 캐스팅 시간
        SkillCasting.SetActive(false);
        W_SkillManager.Instance.GetSkill2().Init(skillZoneTr,Att);
    }

    void Skill3Casting()
    {
        anim.SetTrigger("SkillAttack3");
        W_SkillManager.Instance.GetSkill3().Init(skillZoneTr, Att);
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
    void ShowCoolTime()
    {
        cooltexts[0].text = $"{(int)skill1CooldownTimer}";
        ShowCool[0].fillAmount = skill1CooldownTimer / 5;
        if (skill1CooldownTimer <= 0)
        { cooltexts[0].gameObject.SetActive(false); }
        else
        { cooltexts[0].gameObject.SetActive(true); }

        cooltexts[1].text = $"{(int)skill2CooldownTimer}";
        ShowCool[1].fillAmount = skill2CooldownTimer / 15;
        if (skill2CooldownTimer <= 0)
        { cooltexts[1].gameObject.SetActive(false); }
        else
        { cooltexts[1].gameObject.SetActive(true); }

        cooltexts[2].text = $"{(int)skill3CooldownTimer}";
        ShowCool[2].fillAmount = skill3CooldownTimer / 20;
        if (skill3CooldownTimer <= 0)
        { cooltexts[2].gameObject.SetActive(false); }
        else
        { cooltexts[2].gameObject.SetActive(true); }
    }
}
