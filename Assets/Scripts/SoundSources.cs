using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSources : MonoBehaviour {
    public AudioSource audios;
    public AudioClip hit_sound;     //17
    public AudioClip landing_sound; //22
    public AudioClip jump_sound;    //7
    public AudioClip damage_sound;    //48
    public AudioClip recoverdamage_sound;//33

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void HitSound()
    {
        audios.clip = hit_sound;
        audios.Play();
    }
    public void LandingSound()
    {
        audios.clip = landing_sound;
        audios.Play();
    }
    public void JumpSound()
    {
        audios.clip = jump_sound;
        audios.Play();
    }
    public void DamageSound()
    {
        audios.clip = damage_sound;
        audios.Play();
    }
    public void EnemyDamageSound()
    {
        audios.clip = hit_sound;
        audios.Play();
    }

}
