using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Skill2 : MonoBehaviour
{
    Collider skillCollider;
    float damage = 0;
    bool isVitalization = false;
    Coroutine skillCoroutine = null;
    Coroutine Damangecor = null;

    public void Init(Transform parentTr, float damage)
    {
        gameObject.SetActive(true);

        transform.position = parentTr.position;
        transform.rotation = parentTr.rotation;

        if (skillCollider == null)
        {
            skillCollider = GetComponent<Collider>();
        }

        this.damage = damage;

        isVitalization = false;
        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
        }
        skillCoroutine = StartCoroutine(SkillSequence());
    }

    IEnumerator SkillSequence()
    {
        yield return new WaitForSeconds(2f);

        if (!isVitalization)
        {
            if (Damangecor != null)
            {
                StopCoroutine(Damangecor);
                Damangecor = null;
            }
            W_SkillManager.Instance.ReturnSkill2(this);
        }

        skillCoroutine = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (Damangecor == null)
            { Damangecor = StartCoroutine(DamageDel(other.gameObject)); }            
        }
    }

    IEnumerator DamageDel(GameObject enemy)
    {
        IHit hit = enemy.GetComponent<IHit>();
        if (hit != null)
        { hit.Hit(damage);}
        //Debug.Log("2스킬 적 명중, " + damage);
        yield return new WaitForSeconds(0.5f);
        Damangecor = null;
    }
}
