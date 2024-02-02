using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{       
    public GameObject Status;
    bool S_active = false;   
    [SerializeField] Image Expbar;
    [SerializeField] Image[] HPandMP;
    [SerializeField] Text Level;
    [SerializeField] GameObject optionButton;
    [SerializeField] GameObject Menu;
    [SerializeField] Material[] SkyBox;
    [SerializeField] GameObject Light;
    [SerializeField] GameObject[] ReturnButtonobj;
    public GameObject[] BossPotal;
    public GameObject monsterHP;
    public Image monsterHPbar;
    public Text monstername;    
    DamageText damagetext;    
    public GameObject textobj;
    NoticeUI notice;
    
    bool isactive = false;
    Coroutine cor = null;

    void Start()
    {        
        Status.SetActive(false);
        monsterHP.SetActive(false);       
        optionButton.SetActive(false);
        RenderSettings.skybox = SkyBox[0];
        Expbar.fillAmount = 0;
        Level.text = "1";
        HPandMP[0].fillAmount = 1;
        HPandMP[1].fillAmount = 1;        
    }
    void Update()
    {
        ShowStatusUI();        
        //SkyBoxChange();        
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            Cursor.visible = !isactive;
            if (isactive == false)
            {
                isactive = true;                
                Time.timeScale = 0;               
            }
            else if (isactive == true)
            {
                isactive = false;                
                Time.timeScale = 1;                
            }
            optionButton.SetActive(isactive);
            Cursor.visible = isactive;
            Menu.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {Time.timeScale = 1;}
        else if (Input.GetKeyDown(KeyCode.N))
        { Time.timeScale = 0; }
    }
    void ShowStatusUI()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (S_active == false)
            { S_active = true; }
            else
            { S_active = false; }
            Status.SetActive(S_active);
            Cursor.visible = S_active;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {Status.SetActive(false);}
    }
    public void StatusButton(bool on)
    {
        Status.SetActive(on);
        Cursor.visible = on;
    }    
    public void ShoWEXP(int Exp , int MaxExp, int level)
    {        
        Expbar.fillAmount =(float)Exp / (float)MaxExp;             
        Level.text = $"{level}";       
    }   
    public void ShowHP(float HP,float MaxHP)
    {
        HPandMP[0].fillAmount = HP /MaxHP;       
    }
    public void ShowMP(float MP, float MaxMP)
    { 
        HPandMP[1].fillAmount = MP / MaxMP; 
    }
    public void optionbuttonClick(bool on)
    {
        optionButton.SetActive(on);
        if (on)
        { Time.timeScale = 0; }
        else
        { Time.timeScale = 1; }
        Cursor.visible = on;
    }   
    public void OptionClick(bool on)
    { Menu.SetActive(on); }
    
    public void GameExit()
    {   
        Application.Quit();       
    }

    public void SkyBoxChange()
    {
        
        if (PlayerManager.Instance.transform.position.x > -150)
        {
            RenderSettings.skybox = SkyBox[0];
            Light.SetActive(true);
        }
        else if(PlayerManager.Instance.transform.position.x <= -150 && PlayerManager.Instance.transform.position.x > -250)
        {
            RenderSettings.skybox = SkyBox[1];
            Light.SetActive(false);
        }
        else if (PlayerManager.Instance.transform.position.x <= -250 && PlayerManager.Instance.transform.position.x > -350)
        {RenderSettings.skybox = SkyBox[2]; }
        else if (PlayerManager.Instance.transform.position.x <= -350)
        {RenderSettings.skybox = SkyBox[3];}
    }
    public void MonsterInfo(bool isActive)
    {
        if (cor == null)
        {
            if (isActive)
                cor = StartCoroutine(MonsterHPInfo());
        }
        else
        {
            if (isActive == false)
            {
                StopCoroutine(cor);
                cor = null;
            }
        }
    }
    IEnumerator MonsterHPInfo()
    {
        monsterHP.SetActive(true);                  
        yield return new WaitForSeconds(5f);
        monsterHP.SetActive(false);
        cor = null;
    }
    public void Damage(float damage, Vector3 position)
    {
        if (damagetext == null)
        {damagetext = Instantiate(textobj).GetComponent<DamageText>();}
        damagetext.transform.position = position;
        damagetext.Return(damage);
        damagetext.gameObject.SetActive(true);
    }
    
    public void ReturnButton()
    {
        PlayerManager.instance.transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < ReturnButtonobj.Length; i++)
        {ReturnButtonobj[i].SetActive(false);}
       
    }
    public void NoReturnButton()
    {
        for (int i = 0; i < ReturnButtonobj.Length; i++)
        {ReturnButtonobj[i].SetActive(false);}
        PlayerManager.Instance.transform.position = new Vector3(-340, 0, 0);
        BossPotal[1].SetActive(true);
    }
    public void Bosschallenge()
    {
        if (PlayerManager.Instance.S_Stat.Level >= 5)
        {
            if (PlayerManager.instance.transform.position.x > -350)
            { BossPotal[0].SetActive(false); }
            else if (PlayerManager.instance.transform.position.x <= -360)
            { BossPotal[0].SetActive(true); }
        }
    }
    public void Notice(string massage)
    {
        notice = GameObject.Find("Notice UI").GetComponent<NoticeUI>();
        notice.Sub(massage);
    }
    public void monsterInfo(int Level,string Name, float HP, float MaxHP)
    {
        monstername.text = $"(Lv.{Level}){Name}({HP}/{MaxHP})";
    }
}
