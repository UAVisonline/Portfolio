using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private bool turn_on;

    [SerializeField] private float max_volume;
    [SerializeField] private float volume_constant;

    private void Awake()
    {
        turn_on = false;
        if(audioSource==null)
        {
            audioSource = this.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if(turn_on) // bgm turn on
        {
            if(audioSource.volume < max_volume) // 음량이 목표 수치에 도달하지 못 했을때
            {
                audioSource.volume += Time.deltaTime * volume_constant;
            }
        }
        else // bgm turn off
        {
            if(audioSource.volume > 0) // 음량이 존재하면
            {
                audioSource.volume -= Time.deltaTime * volume_constant;
            }
        }
    }

    public void set_turn(bool value)
    {
        turn_on = value;
    }

    public void music_play() // 음악 재생
    {
        audioSource.Play();
    }

    public void music_stop() // 음악 정지
    {
        audioSource.Stop();
    }
}
