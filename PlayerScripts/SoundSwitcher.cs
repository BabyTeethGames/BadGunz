using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSwitcher : MonoBehaviour {

    private AudioSource audioSource;
    public VirtualAudioSource audioPlayer;
    [SerializeField] public List<AudioClip> Sounds;
    public bool playOnAwake;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Sounds[Random.Range(0, Sounds.Count)];
        audioPlayer = GetComponentInParent<VirtualAudioSource>();
        if (playOnAwake)
        {
            playSound();
        }
    }

    public void playSound()
    {
        audioPlayer.Play();
       
    }

    public void switchSound()
    {
        audioSource.clip = Sounds[Random.Range(0, Sounds.Count)];
    }

}
