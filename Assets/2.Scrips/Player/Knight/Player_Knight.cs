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
    // �� ��ų�� ���� ��ٿ� Ÿ�̸�
    private float AttcooldownTimer = 0f;
    private float skill1CooldownTimer = 0f;
    private float skill2CooldownTimer = 0f;
    private float skill3CooldownTimer = 0f;
    bool isAlive = true;
    Vector3 moveVec = Vector3.zero;   
    public float Def  = 0; // ���� ����
    public float Att  = 0; // ���� ���ݷ�
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
        if (Input.GetKeyDown(KeyCode.Space)) //�����̽� ����
        {
            if (isJump) //������ �ѹ���
            { return; }
            isJump = true;
        }
       
        if (PlayerManager.Instance.x == 0 && PlayerManager.Instance.z == 0) //����
        { speed = 0; }
        else // �ȱ�
        {
            if (Input.GetKey(KeyCode.LeftShift)) //����Ʈ ������ �޸���           
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
                    AttcooldownTimer = 1f; // 1�� ��ٿ�
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
                            UIManager.Instance.Notice("MP�� �����Ͽ� ��ų�� ����� �� �����ϴ�");
                            return; 
                        }
                        PlayerManager.Instance.ConsumeMP(10);
                        anim.SetTrigger("SkillAttack1");
                        StartCoroutine(Skill1Casting());
                        skill2CooldownTimer = 20f; // 20�ʷ� �ٲ� ����
                    }
                    else
                    { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(�����ð� : {(int)skill2CooldownTimer}��)"); }
                }
                else
                { UIManager.Instance.Notice("���� ����� �� ���� �����Դϴ� ."); }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (status.activeSelf == true)
                { return; }
                if (skill1CooldownTimer <= 0f)
                {

                    if (PlayerManager.Instance.S_Stat.MP < 20)
                    {
                        UIManager.Instance.Notice("MP�� �����Ͽ� ��ų�� ����� �� �����ϴ�");
                        return;
                    }
                    PlayerManager.Instance.ConsumeMP(20);
                    anim.SetTrigger("SkillAttack2");
                    skill1CooldownTimer = 5f; // 5�ʷ� �ٲ� ����
                }
                else
                { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(�����ð� : {(int)skill1CooldownTimer}��)"); }
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
                            UIManager.Instance.Notice("MP�� �����Ͽ� ��ų�� ����� �� �����ϴ�");
                            return; 
                        }
                        PlayerManager.Instance.ConsumeMP(30);
                        anim.SetTrigger("SkillAttack3");
                        skill3CooldownTimer = 15f; // 15�ʷ� �ٲ� ����
                    }
                    else
                    { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(�����ð� :{(int)skill3CooldownTimer}��)"); }
                }
                else
                { UIManager.Instance.Notice("���� ����� �� ���� �����Դϴ� ."); }
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
        Def = Knight_Stat.Def * 2; // ���� �ð� ���� ���� �ι�
        //Debug.Log("���� ���� : " + Def);
        yield return new WaitForSeconds(10f);
        Magicshield.SetActive(false);
        Def *= 0.5f;// 10���� ���� ���󺹱� 
        //Debug.Log("���� ���� : " + Def);
    }
   
    //�Ʒ��� ���� ���� �Ǵ��� Ȯ�θ� �Ѱ� ���� ���Ϳ��� ������ �߰� ����
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
