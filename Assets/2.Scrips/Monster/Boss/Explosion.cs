using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Rigidbody rigid;
    float damage = 0;   
    Coroutine cor = null;
    private void Update()
    {
        if (cor == null)
        { cor = StartCoroutine(TimeOverDie()); }
    }
    public void Init(Transform parentTr, float damage)
    {
        gameObject.SetActive(true);

        transform.position = parentTr.position;
        transform.rotation = parentTr.rotation;

        if (rigid == null)
        { rigid = GetComponent<Rigidbody>(); }

        this.damage = damage;
        
    }
    IEnumerator TimeOverDie()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.RetunExplosion(this);        
        cor = null;
    }
    void OnTriggerEnter(Collider other)
    {

        if (cor != null)
        {
            StopCoroutine(cor);
            cor = null;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(damage); }            
        }

    }
}
