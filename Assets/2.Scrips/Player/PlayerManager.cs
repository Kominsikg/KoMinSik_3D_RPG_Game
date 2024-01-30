using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Singleton<PlayerManager>, IHit
{
    Rigidbody rigid;
    [HideInInspector]
    public GameObject[] characters; // �� ���� ĳ���͸� ���� �迭
    public int currentCharacterIndex = 0;
    [SerializeField] GameObject[] skill;
    [SerializeField] GameObject[] effects;
    [SerializeField] GameObject[] characterimg;
    [SerializeField] GameObject[] Skillcore;
    [SerializeField] GameObject[] SkillLock;
    [SerializeField] Text[] ShowCoolText;
    [SerializeField] Image[] ShowCoolImg;
    [SerializeField] Text[] potioncount;
    [SerializeField] GameObject[] UI;
    [SerializeField] Transform Damagepos;
    public int HPpotioncount = 3;
    public int MPpotioncount = 3;   
    float potionCooldownTimer = 0f;
    public float normalSpeed = 2.5f; // �⺻ �̵� �ӵ�
    public float sprintSpeed = 4.0f; // ����Ʈ Ű�� ���� ���� �̵� �ӵ�
    bool isJump = false;
    private float currentSpeed; // ���� �̵� �ӵ�
    Vector3 moveVec = Vector3.zero;
    public AllStruct.Share_Stat S_Stat;

    Player_Wizard PW;
    Player_Knight PK;

    public float def; // ���� ����
    bool isSwitching = true; //ĳ���� ���� ���� ����
    private bool isSwitchingCooldown = false;
    private float switchCooldownTime = 5f; //�ӽ÷� 1�� ���߿� 5�ʷ� �ٲܿ���
    private float switchCooldownTimer = 0f;
    public float x;
    public float z;
    public int MaxExp = 100;  
    public bool isAlive = true;   
    Vector3 Revivalpotion = new Vector3(28, 0.1f, 0);        
    void Start()
    {
        S_Stat = new AllStruct.Share_Stat(1, 200, 100,0);  // �ʱ� 1����/HP 100/MP 100
        PW = transform.GetChild(0).GetComponent<Player_Wizard>();
        PK = transform.GetChild(1).GetComponent<Player_Knight>();        
        def = PW.Def; //�ʱ� ĳ���� �������� ����
        ActivateCharacter(currentCharacterIndex);// ó���� ù ��° ĳ���͸� Ȱ��ȭ
        currentSpeed = normalSpeed;// �ʱ� �̵� �ӵ� ����
        rigid = GetComponent<Rigidbody>();
        characters[1].SetActive(false);
        UI[0].SetActive(false);        
    }

    void Update()
    {
        if (isAlive == false)
        { return; }
        // ĳ���� �̵�
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        MoveCharacter(x, z);
        moveVec.x = x;
        moveVec.z = z;

        if (isSwitchingCooldown)
        {
            switchCooldownTimer -= Time.deltaTime;
            // ��ٿ� �ð��� �������� Ȯ��
            if (switchCooldownTimer <= 0)
            { isSwitchingCooldown = false; }
        }
        // ĳ���� ��ȯ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isSwitchingCooldown)
            {
                if (isSwitching)
                {
                    // ĳ���� ���� �Է� Ȯ��
                    SwitchCharacter();
                    for (int i = 0; i < 2; i++) //ĳ���� ��������� �����ִ� ��ų ����Ʈ ����
                    { skill[i].SetActive(false); }
                    // ��ٿ� Ÿ�̸� ����
                    StartSwitchCooldown();
                }
            }
            else if (isSwitchingCooldown)
            { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(���� �ð� : {(int)switchCooldownTimer})"); }
        }
       
        
        // Shift Ű�� ������ ������ �̵� �ӵ��� ����
        if (Input.GetKey(KeyCode.LeftShift) && isJump == false)
        { currentSpeed = sprintSpeed; }
        else
        { currentSpeed = normalSpeed; }
        if (Input.GetKeyDown(KeyCode.Space)) //�����̽� ����
        {
            if (isJump) //������ �ѹ���
            { return; }
            isSwitching = false;
            isJump = true;
            rigid.AddForce(moveVec.normalized + Vector3.up * 10f, ForceMode.Impulse);
        }
        ShowCoolTime();
        //ĳ���� �ٲ𶧸��� ������ ���� ����
        if (currentCharacterIndex == 0)
        { def = PW.Def; }
        else if (currentCharacterIndex == 1)
        { def = PK.Def; }        
  
        ShowPotionCount();
        potionCooldownTimer = Mathf.Max(0f, potionCooldownTimer - Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {            
            if (HPpotioncount <= 0)
            {
                UIManager.Instance.Notice("����� �� �ִ� HP������ �����մϴ�.");
                return;
            }
            if (potionCooldownTimer <= 0)
            {
                S_Stat.HP = Mathf.Min(S_Stat.MaxHP, S_Stat.HP + S_Stat.MaxHP * 0.5f);
                HPpotioncount--;
                potionCooldownTimer = 10f;                                                                
                StartCoroutine(Healeffect());
                UIManager.instance.ShowHP(S_Stat.HP, S_Stat.MaxHP);
            }
            else
            { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(���� �ð� : {(int)potionCooldownTimer}��)"); }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {          
            if (MPpotioncount <= 0)
            {
                UIManager.Instance.Notice("����� �� �ִ� MP������ �����մϴ�.");
                return;
            }
            if (potionCooldownTimer <= 0)
            {
                S_Stat.MP = math.min(S_Stat.MaxMP, S_Stat.MP + S_Stat.MaxMP * 0.5f);
                MPpotioncount--;
                potionCooldownTimer = 10f;
                UIManager.instance.ShowMP(S_Stat.MP, S_Stat.MaxMP);
                StartCoroutine(Healeffect());
            }
            else
            { UIManager.Instance.Notice($"��Ÿ���Դϴ�.(���� �ð� : {(int)potionCooldownTimer}��)"); }
        }
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    S_Stat.Level += 9;
        //    S_Stat.MaxHP += 10000;
        //    S_Stat.HP = S_Stat.MaxHP;
        //    PW.Wizard_Stat.Att += 1000;
        //    transform.position = new Vector3(-340, 0, 0);
        //}
        if (Input.GetKeyDown(KeyCode.L))
        { IncreaseExperience(50); }
        if (gameObject.transform.position.y <= -10)
        { Hit(9999);}
    }   
    void SwitchCharacter()
    {
        // ���� ĳ���� ��Ȱ��ȭ
        characters[currentCharacterIndex].SetActive(false);
        characterimg[currentCharacterIndex].SetActive(false);
        Skillcore[currentCharacterIndex].SetActive(false);
        // ���� ĳ���� �ε��� ���
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Length;

        // ���� ĳ���� Ȱ��ȭ
        ActivateCharacter(currentCharacterIndex);
        //Debug.Log("���� ���� : " + def);
    }

    void ActivateCharacter(int index)
    {
        characters[index].SetActive(true);
        characterimg[index].SetActive(true);
        Skillcore[index].SetActive(true);
        // ��� ĳ���͵��� ��ġ�� �����ϰ� ����
        for (int i = 0; i < characters.Length; i++)
        {
            if (i != index)
            { characters[i].transform.position = characters[index].transform.position; }
        }
    }

    void MoveCharacter(float horizontal, float vertical)
    {
        Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical);
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime * 2);
        UIManager.instance.SkyBoxChange();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            isSwitching = true;
        }
    }    

    void StartSwitchCooldown()
    {
        isSwitchingCooldown = true;
        switchCooldownTimer = switchCooldownTime;
    }

    public void Hit(float damage) //�¾����� ������
    {
        if (isAlive == false)
        {return;}       
        if (currentCharacterIndex == 0)
        { PW.HitAnim(); }
        else if (currentCharacterIndex == 1)
        { PK.HitAnim(); }
        damage = Mathf.Max(10, (damage - def));           
        S_Stat.HP = Mathf.Max(0, S_Stat.HP - damage);
        UIManager.instance.ShowHP(S_Stat.HP, S_Stat.MaxHP);
        if (S_Stat.HP <= 0)
        { Die(); }
    }
    void Die() //���
    {
        if (currentCharacterIndex == 0)
        { PW.Die(); }
        else if (currentCharacterIndex == 1)
        { PK.Die(); }
        isAlive = false;
        UIManager.Instance.Notice("�÷��̾ ����Ͽ����ϴ�.");
        UI[0].SetActive(true);
        Cursor.visible = true;
        if (S_Stat.Level == 10)
        { return; }
        S_Stat.EXP = (int)Mathf.Max(0f, S_Stat.EXP - (int)(MaxExp * 0.1f));       
    }
    public void Revival() //��Ȱ
    {
        if (currentCharacterIndex == 0)
        { PW.Revival(); }
        else if (currentCharacterIndex == 1)
        { PK.Revival(); }
        UI[0].SetActive(false);
        S_Stat.HP = S_Stat.MaxHP;
        S_Stat.MP = S_Stat.MaxMP;
        if (transform.position.x <= -350f)
        { transform.position = new Vector3(-343, 0, 33); }
        else
        { transform.position = Revivalpotion; }        
        isAlive = true;
        StartCoroutine(Revivaleffect());
        Cursor.visible = false;
        UIManager.instance.ShowHP(S_Stat.HP, S_Stat.MaxHP);
        UIManager.instance.ShowMP(S_Stat.MP, S_Stat.MaxMP);
    }
    IEnumerator Revivaleffect() //��Ȱ ����Ʈ
    {
        effects[4].SetActive(true);
        yield return new WaitForSeconds(2f);
        effects[4].SetActive(false);
    }
    public void ConsumeMP(float amount) //MP�Ҹ�
    {       
        S_Stat.MP -= amount;
        UIManager.instance.ShowMP(S_Stat.MP, S_Stat.MaxMP);
    }
    public void IncreaseExperience(int amount)
    {
        if (S_Stat.Level == 10 )
        {return;}
        UIManager.Instance.Notice($"{amount}�� ����ġ�� ŉ���Ͽ����ϴ�");        
        S_Stat.EXP += amount; // ����ġ ����
        UIManager.instance.ShoWEXP(S_Stat.EXP,MaxExp,S_Stat.Level);
   
        // ������ üũ
        CheckLevelUp();
    }

    void CheckLevelUp() //������ üũ
    {  
        if (S_Stat.EXP >= MaxExp)
        { LevelUp(); }
    }

    void LevelUp() //������
    {  
        S_Stat.Level++;
        if (S_Stat.Level == 5)
        {UIManager.Instance.Notice("Level UP \n5���� �޼����� �������� ������ �� �ֽ��ϴ�.");}
        else if(S_Stat.Level == 3)
        { UIManager.Instance.Notice("Level UP \n3���� �޼����� Q��ų�� ����Ǿ����ϴ�."); }
        else if (S_Stat.Level == 6)
        { UIManager.Instance.Notice("Level UP \n6���� �޼����� E��ų�� ����Ǿ����ϴ�."); }
        else if (S_Stat.Level == 10)
        {
            UIManager.Instance.Notice("Level UP \n�ְ� ������ �޼��߽��ϴ�.");
            effects[1].SetActive(true);
        }        
        else
        { UIManager.Instance.Notice("Level UP");}
        UIManager.instance.Bosschallenge();
        StartCoroutine(LevelUPeffect());        
        UIManager.instance.ShowHP(S_Stat.HP,S_Stat.MaxHP);
        UIManager.instance.ShowMP(S_Stat.MP, S_Stat.MaxMP);
        S_Stat.EXP -= MaxExp;
        UIManager.instance.ShoWEXP(S_Stat.EXP, MaxExp, S_Stat.Level);
        // �ִ� ü�°� �ִ� ���� ����
        if (S_Stat.Level <= 5)
        {
            S_Stat.MaxHP += 300;
            S_Stat.MaxMP += 40;
            PW.StatPoint += 10;
            PK.StatPoint += 10;
            MaxExp += 50;
        }
        else if (S_Stat.Level == 10)
        {
            S_Stat.MaxHP += 1000;
            S_Stat.MaxHP += 200;
            PW.StatPoint += 20;
            PK.StatPoint += 20;
            S_Stat.EXP = MaxExp;
        }
        else
        {
            S_Stat.MaxHP += 350;
            S_Stat.MaxMP += 60;
            PW.StatPoint += 10;
            PK.StatPoint += 10;
            MaxExp += 100;
        }
        if (S_Stat.Level >= 3)
        {
            SkillLock[0].SetActive(false);
            if (S_Stat.Level >= 6)
            { SkillLock[1].SetActive(false); }
        }
        // ���� ü�°� ���� ������ �ִ밪���� �ʱ�ȭ
        S_Stat.HP = S_Stat.MaxHP;
        S_Stat.MP = S_Stat.MaxMP;
       
    }

    IEnumerator LevelUPeffect() //������ ����Ʈ
    {
        effects[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        effects[0].SetActive(false);
    }
    void ShowCoolTime() //��Ÿ�� ǥ��
    {
        ShowCoolText[0].text = $"{(int)switchCooldownTimer}";
        ShowCoolImg[0].fillAmount = switchCooldownTimer / switchCooldownTime;
        if (switchCooldownTimer <= 0)
        {ShowCoolText[0].gameObject.SetActive(false);}
        else
        { ShowCoolText[0].gameObject.SetActive(true); }

        ShowCoolText[1].text = $"{(int)potionCooldownTimer}";
        ShowCoolImg[1].fillAmount = potionCooldownTimer / 5;
        if (potionCooldownTimer <= 0)
        { ShowCoolText[1].gameObject.SetActive(false); }
        else
        { ShowCoolText[1].gameObject.SetActive(true); }

        ShowCoolText[2].text = $"{(int)potionCooldownTimer}";
        ShowCoolImg[2].fillAmount = potionCooldownTimer / 5;
        if (potionCooldownTimer <= 0)
        { ShowCoolText[2].gameObject.SetActive(false); }
        else
        { ShowCoolText[2].gameObject.SetActive(true); }
    }
    IEnumerator Healeffect()
    {
        effects[3].SetActive(true);
        yield return new WaitForSeconds(1f);
        effects[3].SetActive(false);
    }

    void ShowPotionCount() //���� ���� ǥ��
    {
        potioncount[0].text = $"x{HPpotioncount}";
        potioncount[1].text = $"x{MPpotioncount}";
    }
   

}

