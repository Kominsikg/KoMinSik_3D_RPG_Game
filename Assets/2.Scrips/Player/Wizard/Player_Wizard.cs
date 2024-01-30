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
    bool isAttack = false; //���� �����̸� �ֱ� ����
    bool isAlive = true;

    public AllStruct.individual_Stat Wizard_Stat;
    Vector3 moveVec = Vector3.zero;

    // �� ��ų�� ���� ��ٿ� Ÿ�̸�
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
        if (Input.GetKeyDown(KeyCode.Space)) //�����̽� ����
        {
            if (isJump) //������ �ѹ���
            { return; }
            isJump = true;
            anim.SetBool("isJump", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        { anim.SetBool("isJump", false); }

        if (PlayerManager.Instance.x == 0 && PlayerManager.Instance.z == 0) //����
        { speed = 0; }
        else // �ȱ�
        {
            if (Input.GetKey(KeyCode.LeftShift)) //����Ʈ ������ �޸���           
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
            else if (Input.GetMouseButtonDown(1)) // ���콺 ��Ŭ������ ��ų1 ���
            {
                if (skill1CooldownTimer <= 0)
                {                    
                    if (PlayerManager.Instance.S_Stat.MP < 10f)
                    {
                        UIManager.Instance.Notice("MP�� �����Ͽ� ��ų�� ����� �� �����ϴ�");
                        return; 
                    }
                    PlayerManager.Instance.ConsumeMP(10);
                    Skill1Casting();
                    skill1CooldownTimer = 5f;                                     
                }
                else
                { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(�����ð� : {(int)skill1CooldownTimer}��)");}               
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // Q Ű�� ��ų2 ���
            {
                if (PlayerManager.Instance.S_Stat.Level >= 3)
                {
                    if (skill2CooldownTimer <= 0)
                    {
                        if (PlayerManager.Instance.S_Stat.MP < 20f)
                        {
                            UIManager.Instance.Notice("MP�� �����Ͽ� ��ų�� ����� �� �����ϴ�");
                            return;
                        }
                        PlayerManager.Instance.ConsumeMP(20);
                        skill2CooldownTimer = 10f;
                        StartCoroutine(Skill2Casting());
                    }
                    else
                    { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(�����ð� {(int)skill2CooldownTimer}��)"); }
                }
                else
                { UIManager.Instance.Notice("���� ����� �� ���� �����Դϴ� ."); }
            }
            else if (Input.GetKeyDown(KeyCode.E)) // E Ű�� ��ų3 ���
            {
                if (PlayerManager.Instance.S_Stat.Level >= 6)
                {
                    if (skill3CooldownTimer <= 0)
                    {
                        if (PlayerManager.Instance.S_Stat.MP < 30f)
                        {
                            UIManager.Instance.Notice("MP�� �����Ͽ� ��ų�� ����� �� �����ϴ�");
                            return; 
                        }
                        PlayerManager.Instance.ConsumeMP(30);
                        skill3CooldownTimer = 20f;
                        Skill3Casting();
                    }
                    else
                    { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(�����ð� : {(int)skill3CooldownTimer}�� )"); }
                }
                else
                { UIManager.Instance.Notice("���� ����� �� ���� �����Դϴ� ."); }
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab)) //������ Ȱ��ȭ������ ���� �ʱ�ȭ
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
        yield return new WaitForSeconds(0.5f); // ���� �غ� �ð�        
        GameManager.Instance.GetMagicBall().Init(attTr, Att);        
        yield return new WaitForSeconds(0.5f); // ���� �� �����̽ð�        
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
        yield return new WaitForSeconds(1f); // ��ų ĳ���� �ð�
        SkillCasting.SetActive(false);
        W_SkillManager.Instance.GetSkill2().Init(skillZoneTr,Att);
    }

    void Skill3Casting()
    {
        anim.SetTrigger("SkillAttack3");
        W_SkillManager.Instance.GetSkill3().Init(skillZoneTr, Att);
    }
    
    public void Die() //ĳ���Ϳ����� �ִϸ��̼Ǹ� ����
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
