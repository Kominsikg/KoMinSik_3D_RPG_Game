using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Skill1 : MonoBehaviour
{
    ParticleSystem ps;
    Collider coll;
    float damage = 0;
    bool isVitalization = false;
    Coroutine cor = null;
    
    public void Init(Transform parentTr, float damage)
    {
        gameObject.SetActive(true);

        transform.position = parentTr.position;
        transform.rotation = parentTr.rotation;

        if (coll == null)
        {coll = GetComponent<Collider>();}

        this.damage = damage * 2;

        isVitalization = false;
        if (cor != null)
        {StopCoroutine(cor);}
        cor = StartCoroutine(Break());
    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(1f);
        if (isVitalization == false)
        {W_SkillManager.Instance.ReturnSkill1(this);}
        cor = null;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(damage); }           
        }
    }


}
