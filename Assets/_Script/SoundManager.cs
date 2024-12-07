using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    public AudioClip jumpSound;
    public AudioClip platformBreakSound;

    private AudioSource _audioSource; 

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string clipName)
    {
        AudioClip clip = GetAudioClip(clipName);

        _audioSource.PlayOneShot(clip);
    }

    [ContextMenu("Play Jump Sound")]
    private void PlayJumpSound()
    {
        _audioSource.PlayOneShot(jumpSound);
    }

    private AudioClip GetAudioClip(string name)
    {
        switch (name)
        {
            case "jump":
                return jumpSound;
            case "platformBreak":
                return platformBreakSound;
            default:
                return null;
        }
    }
}
