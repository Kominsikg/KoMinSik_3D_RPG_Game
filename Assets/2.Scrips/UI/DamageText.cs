using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    TextMesh text;
    Color alpha;
    float damage;
    float movespeed = 2;
    float alphaspeed = 2;    
    Coroutine cor = null;

    void Start()
    {        
        text = transform.GetChild(0).GetComponent<TextMesh>();
        alpha = text.color;               
    }

    public void Return(float damage)
    {
        alpha.a = 1;
        this.damage = damage; 
    }
    
    void Update()
    {
        text.text = $"{damage}";
        transform.Translate(new Vector3(0, Time.deltaTime * movespeed, 0));
        alpha.a = Mathf.Max(0, alpha.a - (Time.deltaTime * alphaspeed));
        text.color = alpha;
        transform.LookAt(PlayerManager.Instance.transform, Vector3.up);        
        if (cor != null)
        {StopCoroutine(cor);}
        cor = StartCoroutine(destroyTime());
    }

    IEnumerator destroyTime()
    {
        yield return new WaitForSeconds(2);
        cor = null;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

}
