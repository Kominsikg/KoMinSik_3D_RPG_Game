using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] allBGM;
    [SerializeField]
    AudioClip[] PlayerallSoundEffects;
    AudioSource audioSource;
    int SoundNum = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();       
    }
    private void Update()
    {
        PlayMapMusic();
    }
    void PlayMapMusic()
    {        
        if (PlayerManager.Instance.transform.position.x >= -150f)
        {
            audioSource.clip = allBGM[0];
            if (SoundNum != 1)
            {
                SoundNum = 1;
                audioSource.Play();
            }
        }
        else if (PlayerManager.Instance.transform.position.x < -150f && PlayerManager.Instance.transform.position.x >= -350f)
        {           
            audioSource.clip = allBGM[1];
            if (SoundNum != 2)
            {
                SoundNum = 2;
                audioSource.Play();
            }
        }
        else if (PlayerManager.Instance.transform.position.x < -350f)
        {
            audioSource.clip = allBGM[2];
            if (SoundNum != 3)
            {
                SoundNum = 3;
                audioSource.Play();
            }
        }

    }
}