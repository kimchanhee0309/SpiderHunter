using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자동으로 AudioSource GetComponent 부착
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    //어디서나 접근할 수 있는 정적 변수 만들기
    public static SoundManager instance;

    AudioSource myAudio;

    public AudioClip sndHitEnefmy;
    public AudioClip sndEnemyAttack;
    public AudioClip sndPickUp;
    public AudioClip sndEnemyDie;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayHitSound()
    {
        myAudio.PlayOneShot(sndHitEnefmy);
    }

    public void PlayEnemyAttack()
    {
        myAudio.PlayOneShot(sndEnemyAttack);
    }

    public void PlayEnemyDie()
    {
        myAudio.PlayOneShot(sndEnemyDie);
    }

    public void PlayPickUpSound()
    {
        myAudio.PlayOneShot(sndPickUp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
