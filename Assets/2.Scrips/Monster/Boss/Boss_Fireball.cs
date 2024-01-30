using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Fireball : MonoBehaviour
{
    Rigidbody rigid;
    float damage = 0;
    public float Speed = 20f;
    Coroutine cor = null;
    public void Init(Transform parentTr, float damage)
    {
        gameObject.SetActive(true);

        transform.position = parentTr.position;
        transform.rotation = parentTr.rotation;

        if (rigid == null)
        { rigid = GetComponent<Rigidbody>(); }

        this.damage = damage * 2f;


        if (cor != null)
        { StopCoroutine(cor); }
        cor = StartCoroutine(TimeOverDie());
    }
    void FixedUpdate()
    {
        rigid.velocity = transform.forward * Speed;
    }
    IEnumerator TimeOverDie()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.RetunBossFireBall(this);
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
        else if (other.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.RetunBossFireBall(this);
        }

    }
}

