using UnityEngine;
using System.Collections;
using System;

public class move_world : MonoBehaviour {

    public float x_start, y_start,x_dir,y_dir;
    public float Max_x, Min_x, Max_y, Min_y;
    public string worldname;
    public bool white_fade, bgm_off;
    public AudioClip foot_walk;

	// Use this for initialization

	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            AudioSource ad_source = GetComponent<AudioSource>();
            ad_source.clip = foot_walk;
            //ad_source.loop = true;
            ad_source.Play();
            if(bgm_off)
            {
                Bgm_manager bg_manager = FindObjectOfType<Bgm_manager>();
                bg_manager.music_stop();
            }
        }
    }
}
