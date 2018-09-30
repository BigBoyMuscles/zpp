using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour {

    public AudioSource sound;

    public AudioClip jump;
    public AudioClip shoot;
    public AudioClip rope;
    public AudioClip death;
    public AudioClip hurt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void jumpAudio()
    {
        sound.PlayOneShot(jump);
    }

    public void shootAudio()
    {
        sound.PlayOneShot(shoot);
    }

    public void ropeAudio()
    {
        sound.PlayOneShot(rope);
    }

    public void deathAudio()
    {
        sound.PlayOneShot(death);
    }

    public void hurtAudio()
    {
        sound.PlayOneShot(hurt);
    }
}
