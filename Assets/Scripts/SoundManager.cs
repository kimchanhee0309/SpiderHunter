using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ڵ����� AudioSource GetComponent ����
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    //��𼭳� ������ �� �ִ� ���� ���� �����
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
