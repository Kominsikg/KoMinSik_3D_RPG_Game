using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour
{
    public GameObject MessageBox;
    public Text MessageText;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        MessageBox.SetActive(false);
    }

    public void Sub(string message)
    {
        MessageText.text = message;
        MessageBox.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(SubDelay());
    }
    IEnumerator SubDelay()
    {
        MessageBox.SetActive(true);
        anim.SetBool("isOn", true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isOn", false);
        yield return new WaitForSeconds(0.5f);
        MessageBox.SetActive(false);
    }
}
