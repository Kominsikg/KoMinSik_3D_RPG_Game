using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monstar_anim : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();          
    }   
    
    public void Move(bool isMove)
    {
        anim.SetBool("isMove",isMove); 
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public void Dead()
    {
        anim.SetTrigger("Die");
    }
    public void Damage()
    {
        anim.SetTrigger("Hit");
    }
    public void SkillAttack()
    {
        anim.SetTrigger("SkillAttack");
    }
    public void Revival()
    {
        anim.SetTrigger("Revival");
    }
}
