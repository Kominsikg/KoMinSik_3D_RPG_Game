using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPpotion : MonoBehaviour
{
    Coroutine cor = null;
    Rigidbody rigid;
   
    public void Init(Transform Tr)
    {
        gameObject.SetActive(true);
        transform.position = Tr.position;
        transform.rotation = Tr.rotation;

        if (rigid == null)
        { rigid = GetComponent<Rigidbody>(); }
        if (cor != null)
        { StopCoroutine(cor); }
        cor = StartCoroutine(TimeOverDie());
    }
    IEnumerator TimeOverDie()
    {
        yield return new WaitForSeconds(10f);
        GameManager.Instance.ReturnHPpotion(this);
        cor = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.HPpotioncount++;
            UIManager.Instance.Notice("HPÆ÷¼ÇÀ» Å‰µæÇÏ¿´½À´Ï´Ù");
            GameManager.Instance.ReturnHPpotion(this);
        }

    }
}
