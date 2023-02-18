using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Util_Manager : MonoBehaviour
{
    private static Util_Manager _utilManager;

    public static Util_Manager utilManager
    {
        get
        {
            if(_utilManager==null)
            {
                _utilManager = FindObjectOfType<Util_Manager>();
                if(_utilManager==null)
                {
                    Debug.LogError("Can't Find Util Manager");
                }
            }
            return _utilManager;
        }
    }

    [SerializeField] private SmallinformationWindow small_window;
    [SerializeField] private WaitForSeconds short_time = new WaitForSeconds(0.02f);
    [SerializeField] private WaitForSeconds middle_time = new WaitForSeconds(0.1f);

    [SerializeField] private List<AudioSource> sfx_audiosource_list;
    private int audio_sfx_pos = 0;

    [SerializeField] private AudioSource bgm_audiosource_first;
    [SerializeField] private AudioSource bgm_audiosource_second;

    [SerializeField] private AudioClip button_click_sfx;
    private Smallinformation ref_information;

    private void Awake()
    {
        if(_utilManager==null)
        {
            _utilManager = this.GetComponent<Util_Manager>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void small_window_init(string name, string information, Smallinformation smallinformation)
    {
        small_window.gameObject.SetActive(true);
        small_window.visualize(name, information);
        ref_information = smallinformation;
    }

    public void small_window_off()
    {
        ref_information = null;
        small_window.gameObject.SetActive(false);
    }

    public bool check_smallinformation(Smallinformation value)
    {
        return ref_information == value;
    }

    public WaitForSeconds return_short_time()
    {
        return short_time;
    }

    public void play_clip(AudioClip value)
    {
        if(value!=null)
        {
            sfx_audiosource_list[audio_sfx_pos].clip = value;
            sfx_audiosource_list[audio_sfx_pos].Play();
            audio_sfx_pos++;
            if (sfx_audiosource_list.Count <= audio_sfx_pos)
            {
                audio_sfx_pos = 0;
            }
        }
    }

    public void button_click_sound_play()
    {
        play_clip(button_click_sfx);
    }

    public void bgm_play(AudioClip value)
    {
        if(bgm_audiosource_first.isPlaying==true)
        {
            StartCoroutine(bgm_naturally_change(value,1));
        }
        else if(bgm_audiosource_second.isPlaying==true)
        {
            StartCoroutine(bgm_naturally_change(value,2));
        }
        else
        {
            if(value!=null)
            {
                StartCoroutine(bgm_naturally_change(value, 1));
            }
        }
    }

    public void bgm_instant_stop()
    {
        if(bgm_audiosource_first.isPlaying==true)
        {
            bgm_audiosource_first.Stop();
        }
        
        if(bgm_audiosource_second.isPlaying==true)
        {
            bgm_audiosource_second.Stop();
        }
    }

    public void bgm_instant_start(AudioClip value)
    {
        if(bgm_audiosource_first.isPlaying==false)
        {
            bgm_audiosource_first.clip = value;
            bgm_audiosource_first.volume = 1.0f;
            bgm_audiosource_first.Play();
        }
        else if(bgm_audiosource_second.isPlaying==false)
        {
            bgm_audiosource_second.clip = value;
            bgm_audiosource_second.volume = 1.0f;
            bgm_audiosource_second.Play();
        }
    }

    IEnumerator bgm_naturally_change(AudioClip value,int pos)
    {
        float pre_sound;
        float divide_sound;

        if(pos==1)
        {
            pre_sound = bgm_audiosource_first.volume;
            divide_sound = pre_sound * 0.1f;

            bgm_audiosource_second.Stop();
            bgm_audiosource_second.volume = 0.0f;
            if (value != null)
            {
                bgm_audiosource_second.clip = value;
            }

            for (int i =0;i<10;i++)
            {
                bgm_audiosource_first.volume -= divide_sound;
                if(bgm_audiosource_first.volume<0.0f)
                {
                    bgm_audiosource_first.volume = 0.0f;
                }

                if(i>=7 && value!=null)
                {
                    if(bgm_audiosource_second.isPlaying==false)
                    {
                        bgm_audiosource_second.Play();
                    }
                    bgm_audiosource_second.volume += 0.1f;
                }
                yield return middle_time;
            }

            if(value!=null)
            {
                while(bgm_audiosource_second.volume<1.0f)
                {
                    bgm_audiosource_second.volume += 0.1f;
                    yield return middle_time;
                }
            }
        }
        else if(pos==2)
        {
            pre_sound = bgm_audiosource_second.volume;
            divide_sound = pre_sound * 0.1f;

            bgm_audiosource_first.Stop();
            bgm_audiosource_first.volume = 0.0f;
            if (value != null)
            {
                bgm_audiosource_first.clip = value;
            }

            for (int i = 0; i < 10; i++)
            {
                bgm_audiosource_second.volume -= divide_sound;
                if (bgm_audiosource_second.volume < 0.0f)
                {
                    bgm_audiosource_second.volume = 0.0f;
                }

                if (i >= 7 && value != null)
                {
                    if (bgm_audiosource_first.isPlaying == false)
                    {
                        bgm_audiosource_first.Play();
                    }
                    bgm_audiosource_first.volume += 0.1f;
                }
                yield return middle_time;
            }

            if (value != null)
            {
                while (bgm_audiosource_first.volume < 1.0f)
                {
                    bgm_audiosource_first.volume += 0.1f;
                    yield return middle_time;
                }
            }
        }
    }
}
