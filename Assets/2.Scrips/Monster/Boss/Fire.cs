using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{    
    Two_Phase bosstwo;
    private void Update()
    {
       if( PlayerManager.Instance.S_Stat.HP <= 0)
        { gameObject.SetActive(false); }
        StartCoroutine(FireTime());
    }
    IEnumerator FireTime()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(400); }
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            float HealingPoint = 100;
            bosstwo = GameObject.Find("Red (1)").GetComponent<Two_Phase>();
            if (bosstwo.B_Stat.HP <= 0)
            {HealingPoint = 0;}
            bosstwo.B_Stat.HP = Mathf.Min(bosstwo.B_Stat.MaxHP, bosstwo.B_Stat.HP + HealingPoint);
        }
    }
}
