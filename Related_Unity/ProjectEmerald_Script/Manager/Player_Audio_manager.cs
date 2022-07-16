using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Audio_manager : MonoBehaviour
{
    private AudioSource audioSource;
    private static Player_Audio_manager _player_audio_manager;

    public static Player_Audio_manager player_audio_manager
    {
        get
        {
            if (_player_audio_manager == null)
            {
                _player_audio_manager = FindObjectOfType<Player_Audio_manager>();
                if (_player_audio_manager == null)
                {
                    Debug.LogError("There's no active ManagerClass object");
                }
            }
            return _player_audio_manager;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Player_sound_process()
    {
        audioSource.Play();
    }

    public void Sound_clip_set(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void Sound_pitch_set(float pitch)
    {
        audioSource.pitch = pitch;
    }

    public void Sound_volume_set(float volume)
    {
        audioSource.volume = volume;
    }
}
