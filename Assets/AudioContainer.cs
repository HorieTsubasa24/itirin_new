using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainer : MonoBehaviour {
    public AudioClip[] audios;
    public GameObject StageUI;
    int SoundIdx { get; set; }
    public AudioSource audiosource;

	// Use this for initialization
	void Start () {
        SoundIdx = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(audiosource.isPlaying == false)
        {
            SoundIdx = (SoundIdx < audios.Length - 1) ? SoundIdx + 1 : 0;
            audiosource.clip = audios[SoundIdx];
            audiosource.Play();
            StageUI.GetComponent<Animator>().Play("Sound_Appear", 0);
        }
	}
}
