using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip playerHit;
    public AudioClip enemyHit;
    public AudioClip shadowBolt;
    public AudioClip shadowNova;
    public AudioClip playerMelee;
    public AudioClip Music;
    public AudioClip enemyDie;
    public AudioClip playerDie;
    public AudioClip levelUp;
    public AudioClip enemyCelebrate;

    public void PlaySound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
