using UnityEngine;
using System.Collections;

public class Shamen_pref : MonoBehaviour {

    public bool bgm_change, rage;
    public AudioClip horror_music;
    public Bgm_manager bg_manager;
    // Use this for initialization
    void Start () {
        //PlayerPrefs.DeleteKey("shamen_rage");
	if(PlayerPrefs.GetInt("kill")>=5 && PlayerPrefs.GetInt("dump_old_battle")==1 && PlayerPrefs.GetInt("dump_young_battle")==1)
        {
            if(!PlayerPrefs.HasKey("shamen_rage"))
            {
                PlayerPrefs.SetInt("shamen_rage", 0);
            } 
        }
    if(PlayerPrefs.GetInt("shamen_rage")==0 || PlayerPrefs.GetInt("shamen_rage") == 1)
        {
            bg_manager = FindObjectOfType<Bgm_manager>();
            rage = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(rage)
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
                    bg_manager.music_change(horror_music);
                }
            }
        }
        
	}
}
