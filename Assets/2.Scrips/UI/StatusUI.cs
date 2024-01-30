using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    Player_Wizard PW;
    Player_Knight PK;
    public Text[] Shape_Statustexts;
    public Text[] Wizard_Statustexts;
    public Text[] Knight_Statustexts;
    [SerializeField] GameObject[] SkillBook;
    bool WizardSkillBookActive = false;
    bool knightSkillBookActive = false;
   
    private void Awake()
    {        
        PW = GameObject.Find("Player_Wizard").GetComponent<Player_Wizard>();
        PK = GameObject.Find("Player_knight").GetComponent<Player_Knight>();      
    }    
    void Update()
    {
        ShowShareStatus();
        ShowPW_status();
        ShowPK_status();
    }
    void ShowShareStatus()
    {
        Shape_Statustexts[0].text = $"{(int)PlayerManager.Instance.S_Stat.HP} / {PlayerManager.Instance.S_Stat.MaxHP}";
        Shape_Statustexts[1].text = $"{(int)PlayerManager.Instance.S_Stat.MP} / {PlayerManager.Instance.S_Stat.MaxMP}";
        Shape_Statustexts[2].text = $"{PlayerManager.Instance.S_Stat.EXP} / {PlayerManager.Instance.MaxExp}";
        Shape_Statustexts[3].text = $"{PlayerManager.Instance.S_Stat.Level}";
    }
    void ShowPW_status()
    {
        Wizard_Statustexts[0].text = $"{(int)PW.Wizard_Stat.Att}";
        Wizard_Statustexts[1].text = $"{(int)PW.Wizard_Stat.Def}";
        Wizard_Statustexts[2].text = $"{PW.StatPoint}";
    }
    void ShowPK_status()
    {
        Knight_Statustexts[0].text = $"{(int)PK.Knight_Stat.Att}";
        Knight_Statustexts[1].text = $"{(int)PK.Knight_Stat.Def}";
        Knight_Statustexts[2].text = $"{PK.StatPoint}";
    }
    
    public void Wizard_Att_UP()
    {
        if (PW.StatPoint <= 0)
        {
            UIManager.Instance.Notice("스탯 포인트가 부족합니다.");
            return;
        }
        PW.Wizard_Stat.Att += 10;
        PW.StatPoint -= 1;        
    }
    public void Wizard_Att_UP2()
    {
        if (PW.StatPoint <= 4)
        {
            UIManager.Instance.Notice("스탯 포인트가 부족합니다.");
            return;
        }
        PW.Wizard_Stat.Att += 50;
        PW.StatPoint -= 5;
    }
    public void Wizard_Def_UP()
    {
        if (PW.StatPoint <= 0)
        {
            UIManager.Instance.Notice("스탯 포인트가 부족합니다.");
            return; 
        }
        PW.Wizard_Stat.Def += 10;
        PW.StatPoint -= 1;
    }
    public void Wizard_Def_UP2()
    {
        if (PW.StatPoint <= 4)
        {
            UIManager.Instance.Notice("스탯 포인트가 부족합니다.");
            return;
        }
        PW.Wizard_Stat.Def += 50;
        PW.StatPoint -= 5;
    }
    public void Knight_Att_UP()
    {
        if (PK.StatPoint <= 0)
        {
            UIManager.Instance.Notice("스탯 포인트가 부족합니다.");
            return; 
        }
        PK.Knight_Stat.Att += 10;
        PK.StatPoint -= 1;
    }
    public void Knight_Att_UP2()
    {
        if (PK.StatPoint <= 4)
        {
            UIManager.Instance.Notice("스탯 포인트가 부족합니다.");
            return;
        }
        PK.Knight_Stat.Att += 50;
        PK.StatPoint -= 5;
    }
    public void Knight_Def_UP()
    {
        if (PK.StatPoint <= 0)
        {
            UIManager.Instance.Notice("스탯 포인트가 부족합니다.");
            return;
        }
        PK.Knight_Stat.Def += 10;
        PK.StatPoint -= 1;
    }
    public void Knight_Def_UP2()
    {
        if (PK.StatPoint <= 4)
        {
            UIManager.Instance.Notice("스탯 포인트가 부족합니다.");
            return;
        }
        PK.Knight_Stat.Def += 50;
        PK.StatPoint -= 5;
    }
    public void WizardSkillBookButton()
    {
        if (WizardSkillBookActive == false)
        {WizardSkillBookActive = true;}
        else if (WizardSkillBookActive == true)
        {WizardSkillBookActive = false;}
        SkillBook[0].SetActive(WizardSkillBookActive);
    }
    public void KnightSkillBookButton()
    {
        if (knightSkillBookActive == false)
        { knightSkillBookActive = true; }
        else if (knightSkillBookActive == true)
        { knightSkillBookActive = false; }
        SkillBook[1].SetActive(knightSkillBookActive);
    }
}
