using UnityEngine;
using System.Collections;

public class Test_sound : MonoBehaviour {

    public Sound_effect sound;
    public Sound_effect_loop sound_loop;
    public AudioClip fx,fx_loop;
	// Use this for initialization
	void Start () {
        sound = FindObjectOfType<Sound_effect>();
        sound_loop = FindObjectOfType<Sound_effect_loop>();
        sound.Play_audio(fx);
        sound_loop.Play_audio_loop(fx_loop);
        //Debug.Log("aaa");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
