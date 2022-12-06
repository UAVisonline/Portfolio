using UnityEngine;
using System.Collections;

public class BGM_prefs_object : MonoBehaviour {

    public bool bgm_change;
    public Bgm_manager bg_manager;
    public AudioClip bgm_first,bgm_second;
    public string prefs;
    public int prefs_value;

    // Use this for initialization
    void Start()
    {
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
                
                if(PlayerPrefs.GetInt(prefs)!=prefs_value)
                {
                    bgm_change = true;
                    bg_manager.music_change(bgm_first);
                }
                else
                {
                    bgm_change = true;
                    bg_manager.music_change(bgm_second);
                }
            }
        }
    }
}
