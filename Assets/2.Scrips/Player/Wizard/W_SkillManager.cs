using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_SkillManager : Singleton<W_SkillManager>
{
    public GameObject[] skillprefab;

    Queue<Wizard_Skill1> skill1 = new Queue<Wizard_Skill1>();
    GameObject skill_1;
    Queue<Wizard_Skill2> skill2 = new Queue<Wizard_Skill2>();    
    GameObject skill_2;
    Queue<Wizard_Skill3> skill3 = new Queue<Wizard_Skill3>();
    GameObject skill_3;
    void Start()
    {
        skill_1 = Instantiate(skillprefab[2], transform);
        skill1.Enqueue(skill_1.GetComponent<Wizard_Skill1>());
        skill_1.SetActive(false);

        skill_2 = Instantiate(skillprefab[0], transform);
        skill2.Enqueue(skill_2.GetComponent<Wizard_Skill2>());
        skill_2.SetActive(false);

        skill_3 = Instantiate(skillprefab[1], transform);
        skill3.Enqueue(skill_3.GetComponent<Wizard_Skill3>());
        skill_3.SetActive(false);
    }
    public Wizard_Skill1 GetSkill1()
    {
        if (skill1.Count > 0)
        { return skill1.Dequeue(); }
        else
        { return Instantiate(skill_1, transform).GetComponent<Wizard_Skill1>(); }
    }
    public void ReturnSkill1(Wizard_Skill1 skill)
    {
        skill1.Enqueue(skill);
        skill.transform.SetParent(transform);
        skill.gameObject.SetActive(false);
    }

    public Wizard_Skill2 GetSkill2()
    {
        if (skill2.Count > 0)
        { return skill2.Dequeue();}
        else
        { return Instantiate(skill_2, transform).GetComponent<Wizard_Skill2>(); }
    }
    public void ReturnSkill2(Wizard_Skill2 skill)
    {
        skill2.Enqueue(skill);
        skill.transform.SetParent(transform);        
        skill.gameObject.SetActive(false);       
    }

    public Wizard_Skill3 GetSkill3()
    {
        if (skill3.Count > 0)
        { return skill3.Dequeue(); }
        else
        { return Instantiate(skill_3, transform).GetComponent<Wizard_Skill3>(); }
    }

    public void ReturnSkill3(Wizard_Skill3 skill)
    {
        skill3.Enqueue(skill);
        skill.transform.SetParent(transform);
        skill.gameObject.SetActive(false);
    }
}
