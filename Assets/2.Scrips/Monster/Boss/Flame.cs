using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{    
    Two_Phase Boss;
    private void Start()
    {
        Boss = GetComponentInParent<Two_Phase>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            { hit.Hit(700); }
        }
    }
}
