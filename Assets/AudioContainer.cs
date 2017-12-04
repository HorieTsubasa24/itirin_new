using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioContainer : MonoBehaviour {
    public AudioClip[] audios;
    public GameObject StageUI;
	int StageNum;
    int SoundIdx { get; set; }
    public AudioSource audiosource;

	// Use this for initialization
	void OnEnable () {
		StageNum = 1;
        SoundIdx = 0;
        audiosource.clip = audios[SoundIdx];
        audiosource.Play();
	}
	
	// Update is called once per frame
	public void AudioChange() {
        SoundIdx = (SoundIdx < audios.Length - 1) ? SoundIdx + 1 : 0;
        audiosource.clip = audios[SoundIdx];
        audiosource.Play();
		StageChangeFlag();
	}

	public void StageChangeFlag()
	{
		StageNum++;
		StageUI.GetComponentInChildren<Text>().text = "Stage " + StageNum.ToString();
        StageUI.GetComponent<Animator>().Play("Sound_Appear", 0);
	}
}
