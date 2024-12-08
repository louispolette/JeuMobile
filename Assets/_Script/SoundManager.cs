using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    [Space]

    public AudioClip jumpSound;
    public AudioClip platformBreakSound;
    public AudioClip springSound;

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

    private AudioClip GetAudioClip(string name)
    {
        switch (name)
        {
            case "jump":
                return jumpSound;
            case "platformBreak":
                return platformBreakSound;
            case "spring":
                return springSound;
            default:
                return null;
        }
    }
}
