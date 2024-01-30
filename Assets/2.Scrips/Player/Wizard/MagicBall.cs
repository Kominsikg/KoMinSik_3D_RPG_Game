using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    Rigidbody rigid;
    float damage = 0;
    public float MagicBallSpeed = 10f;
    Coroutine cor = null;   
    public void Init(Transform parentTr, float damage)
    {
        gameObject.SetActive(true);

        transform.position = parentTr.position;
        transform.rotation = parentTr.rotation;
       
        if (rigid == null)
        {rigid = GetComponent<Rigidbody>();}

        this.damage = damage;


        if (cor != null)
        { StopCoroutine(cor); }
        cor = StartCoroutine(TimeOverDie());
    }
    void FixedUpdate()
    {
         rigid.velocity = transform.forward * MagicBallSpeed; 
    }
    IEnumerator TimeOverDie()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.RetunMagicBall(this);
        cor = null;
    }   
    void OnTriggerEnter(Collider other)
    {       
        if (cor != null)
        {
            StopCoroutine(cor);
            cor = null;
        }

        if (other.gameObject.CompareTag("Enemy"))
        { 
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            {hit.Hit(damage);}
            GameManager.Instance.RetunMagicBall(this);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.RetunMagicBall(this);
        }
       
    }
}
