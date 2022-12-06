using UnityEngine;
using System.Collections;

public class Sound_effect_loop : MonoBehaviour {

    private AudioSource ad_source;
    private bool audio_start;

    // Use this for initialization
    void Start()
    {
        ad_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play_audio_loop(AudioClip clip)
    {
        ad_source.clip = clip;
        //ad_source.loop = true;
        ad_source.Play();
        audio_start = true;
    }

    public void Stop()
    {
        ad_source.Stop();
    }
}
