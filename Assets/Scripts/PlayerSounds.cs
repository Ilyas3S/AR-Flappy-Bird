using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip [] audioClips;
    AudioSource audioSource;

    private static PlayerSounds _instance;
    public static PlayerSounds Instance
    {
        get => _instance;
        set
        {
            if (_instance == null)
                _instance = value;
            else
                Debug.LogError("This instance already exist: PlayerSounds");
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Debug.Log(audioClips[0]);
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(Audio audio )
    {
        audioSource.PlayOneShot(audioClips[(int)audio]);
    }
}
public enum Audio
{
    ScoreAdd = 0,
    Die = 1
}
