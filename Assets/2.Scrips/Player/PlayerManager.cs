using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Singleton<PlayerManager>, IHit
{
    Rigidbody rigid;
    [HideInInspector]
    public GameObject[] characters; // 두 개의 캐릭터를 담을 배열
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
    public float normalSpeed = 2.5f; // 기본 이동 속도
    public float sprintSpeed = 4.0f; // 쉬프트 키를 누를 때의 이동 속도
    bool isJump = false;
    private float currentSpeed; // 현재 이동 속도
    Vector3 moveVec = Vector3.zero;
    public AllStruct.Share_Stat S_Stat;

    Player_Wizard PW;
    Player_Knight PK;

    public float def; // 현재 방어력
    bool isSwitching = true; //캐릭터 변경 가는 여부
    private bool isSwitchingCooldown = false;
    private float switchCooldownTime = 5f; //임시로 1초 나중에 5초로 바꿀예정
    private float switchCooldownTimer = 0f;
    public float x;
    public float z;
    public int MaxExp = 100;  
    public bool isAlive = true;   
    Vector3 Revivalpotion = new Vector3(28, 0.1f, 0);        
    void Start()
    {
        S_Stat = new AllStruct.Share_Stat(1, 200, 100,0);  // 초기 1레벨/HP 100/MP 100
        PW = transform.GetChild(0).GetComponent<Player_Wizard>();
        PK = transform.GetChild(1).GetComponent<Player_Knight>();        
        def = PW.Def; //초기 캐릭터 마법사의 방어력
        ActivateCharacter(currentCharacterIndex);// 처음에 첫 번째 캐릭터를 활성화
        currentSpeed = normalSpeed;// 초기 이동 속도 설정
        rigid = GetComponent<Rigidbody>();
        characters[1].SetActive(false);
        UI[0].SetActive(false);        
    }

    void Update()
    {
        if (isAlive == false)
        { return; }
        // 캐릭터 이동
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        MoveCharacter(x, z);
        moveVec.x = x;
        moveVec.z = z;

        if (isSwitchingCooldown)
        {
            switchCooldownTimer -= Time.deltaTime;
            // 쿨다운 시간이 끝났는지 확인
            if (switchCooldownTimer <= 0)
            { isSwitchingCooldown = false; }
        }
        // 캐릭터 전환
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isSwitchingCooldown)
            {
                if (isSwitching)
                {
                    // 캐릭터 변경 입력 확인
                    SwitchCharacter();
                    for (int i = 0; i < 2; i++) //캐릭터 변경됬을때 나와있는 스킬 이펙트 끄기
                    { skill[i].SetActive(false); }
                    // 쿨다운 타이머 시작
                    StartSwitchCooldown();
                }
            }
            else if (isSwitchingCooldown)
            { UIManager.Instance.Notice($"쿨타임입니다.(남은 시간 : {(int)switchCooldownTimer})"); }
        }
       
        
        // Shift 키를 누르고 있으면 이동 속도를 높임
        if (Input.GetKey(KeyCode.LeftShift) && isJump == false)
        { currentSpeed = sprintSpeed; }
        else
        { currentSpeed = normalSpeed; }
        if (Input.GetKeyDown(KeyCode.Space)) //스페이스 점프
        {
            if (isJump) //점프는 한번만
            { return; }
            isSwitching = false;
            isJump = true;
            rigid.AddForce(moveVec.normalized + Vector3.up * 10f, ForceMode.Impulse);
        }
        ShowCoolTime();
        //캐릭터 바뀔때마다 각각의 방어력 적용
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
                UIManager.Instance.Notice("사용할 수 있는 HP포션이 부족합니다.");
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
            { UIManager.Instance.Notice($"쿨타임입니다.(남은 시간 : {(int)potionCooldownTimer}초)"); }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {          
            if (MPpotioncount <= 0)
            {
                UIManager.Instance.Notice("사용할 수 있는 MP포션이 부족합니다.");
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
            { UIManager.Instance.Notice($"쿨타임입니다.(남은 시간 : {(int)potionCooldownTimer}초)"); }
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
        // 현재 캐릭터 비활성화
        characters[currentCharacterIndex].SetActive(false);
        characterimg[currentCharacterIndex].SetActive(false);
        Skillcore[currentCharacterIndex].SetActive(false);
        // 다음 캐릭터 인덱스 계산
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Length;

        // 다음 캐릭터 활성화
        ActivateCharacter(currentCharacterIndex);
        //Debug.Log("현재 방어력 : " + def);
    }

    void ActivateCharacter(int index)
    {
        characters[index].SetActive(true);
        characterimg[index].SetActive(true);
        Skillcore[index].SetActive(true);
        // 모든 캐릭터들의 위치를 동일하게 설정
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

    public void Hit(float damage) //맞았을때 데미지
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
    void Die() //사망
    {
        if (currentCharacterIndex == 0)
        { PW.Die(); }
        else if (currentCharacterIndex == 1)
        { PK.Die(); }
        isAlive = false;
        UIManager.Instance.Notice("플레이어가 사망하였습니다.");
        UI[0].SetActive(true);
        Cursor.visible = true;
        if (S_Stat.Level == 10)
        { return; }
        S_Stat.EXP = (int)Mathf.Max(0f, S_Stat.EXP - (int)(MaxExp * 0.1f));       
    }
    public void Revival() //부활
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
    IEnumerator Revivaleffect() //부활 이펙트
    {
        effects[4].SetActive(true);
        yield return new WaitForSeconds(2f);
        effects[4].SetActive(false);
    }
    public void ConsumeMP(float amount) //MP소모
    {       
        S_Stat.MP -= amount;
        UIManager.instance.ShowMP(S_Stat.MP, S_Stat.MaxMP);
    }
    public void IncreaseExperience(int amount)
    {
        if (S_Stat.Level == 10 )
        {return;}
        UIManager.Instance.Notice($"{amount}의 경험치를 흭득하였습니다");        
        S_Stat.EXP += amount; // 경험치 증가
        UIManager.instance.ShoWEXP(S_Stat.EXP,MaxExp,S_Stat.Level);
   
        // 레벨업 체크
        CheckLevelUp();
    }

    void CheckLevelUp() //레벨업 체크
    {  
        if (S_Stat.EXP >= MaxExp)
        { LevelUp(); }
    }

    void LevelUp() //레벨업
    {  
        S_Stat.Level++;
        if (S_Stat.Level == 5)
        {UIManager.Instance.Notice("Level UP \n5레벨 달성으로 보스에게 도전할 수 있습니다.");}
        else if(S_Stat.Level == 3)
        { UIManager.Instance.Notice("Level UP \n3레벨 달성으로 Q스킬이 개방되었습니다."); }
        else if (S_Stat.Level == 6)
        { UIManager.Instance.Notice("Level UP \n6레벨 달성으로 E스킬이 개방되었습니다."); }
        else if (S_Stat.Level == 10)
        {
            UIManager.Instance.Notice("Level UP \n최고 레벨을 달성했습니다.");
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
        // 최대 체력과 최대 마나 증가
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
        // 현재 체력과 현재 마나를 최대값으로 초기화
        S_Stat.HP = S_Stat.MaxHP;
        S_Stat.MP = S_Stat.MaxMP;
       
    }

    IEnumerator LevelUPeffect() //레벨업 이펙트
    {
        effects[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        effects[0].SetActive(false);
    }
    void ShowCoolTime() //쿨타임 표시
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

    void ShowPotionCount() //포션 개수 표시
    {
        potioncount[0].text = $"x{HPpotioncount}";
        potioncount[1].text = $"x{MPpotioncount}";
    }
   

}

