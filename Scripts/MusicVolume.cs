using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolume : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        _audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
    }
}
