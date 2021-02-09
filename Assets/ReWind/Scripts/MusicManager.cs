using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance
    {
        get
        {
            if (_musicManager) return _musicManager;
                
            _musicManager = FindObjectOfType<MusicManager>();

            if (!_musicManager)
            {
                Debug.LogError("There needs to be a PlayerManager in the scene!");
            }
            else
            {
                _musicManager.Initialize();
            }

            return _musicManager;
        }
    }
    private static MusicManager _musicManager;
    
    [SerializeField] private AudioClip levelCompleteSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Initialize()
    {
        _musicManager = this;
    }

    public void PlayLevelCompleteSound()
    {
        _audioSource.PlayOneShot(levelCompleteSound);
    }
}
