using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Skill3 : MonoBehaviour
{    
    Collider coll;
    bool isVitalization = false;
    Coroutine cor = null;
    float damage = 0;

    public void Init(Transform parentTr, float damage)
    {
        gameObject.SetActive(true);

        transform.position = parentTr.position;
        transform.rotation = parentTr.rotation;

        if (coll == null)
        {coll = GetComponent<Collider>();}

        this.damage = damage * 4;

        isVitalization = false;
        if (cor != null)
        {StopCoroutine(cor);}
        cor = StartCoroutine(Break());
    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(1.5f);
        if (isVitalization == false)
        {W_SkillManager.Instance.ReturnSkill3(this);}
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
