using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Anim : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Move(bool ismove)
    {
        anim.SetBool("Move", ismove);
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public void SkillAttack()
    {
        anim.SetTrigger("SkillAttack");
    }
    public void Hit()
    {
        anim.SetTrigger("Hit");
    }
    public void Die()
    {
        anim.SetTrigger("Die");
    }
    public void Scream()
    {
        anim.SetTrigger("Scream");
    }    

}
