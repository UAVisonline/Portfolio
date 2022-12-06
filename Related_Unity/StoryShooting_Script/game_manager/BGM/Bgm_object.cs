using UnityEngine;
using System.Collections;

public class Bgm_object : MonoBehaviour {

    public bool bgm_change;
    public Bgm_manager bg_manager;
    public AudioClip bgm;

	// Use this for initialization
	void Start () {
        bg_manager = FindObjectOfType<Bgm_manager>();
	}

    // Update is called once per frame
    void Update()
    {
        if (bg_manager == null)
        {
            bgm_change = false;
            bg_manager = FindObjectOfType<Bgm_manager>();
        }
        else
        {
            if (!bgm_change)
            {
                bgm_change = true;
                bg_manager.music_change(bgm);
            }
        }
    }
}
