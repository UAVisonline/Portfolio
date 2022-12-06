using UnityEngine;
using System.Collections;

public class Bgm_manager : MonoBehaviour {

    public AudioClip bgm, change_bgm;
    public AudioSource audio;
    public string bgm_name;
    public bool volume_down, volume_up, stop;
    public static bool destory;
    private GameObject camera;
	// Use this for initialization
	void Start () {
        if(!destory)
        {
            DontDestroyOnLoad(gameObject);
            destory = true;
        }
        else
        {
            Destroy(gameObject);
        }
        audio = GetComponent<AudioSource>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        if(stop)
        {
            audio.volume = 0.0f;
            return;
        }
        if(camera==null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        else
        {
            this.transform.position = camera.transform.position;
        }
	    if(volume_up && !volume_down)
        {
            if(audio.volume<1.0f)
            {
                audio.volume += Time.deltaTime / 2.00f;
                if(audio.volume>1.00f)
                {
                    audio.volume = 1.00f;
                }
            }
            
        }
        else if(!volume_up && volume_down)
        {
            audio.volume -= Time.deltaTime / 1.00f;
        }
	}

    public void music_change(AudioClip change)
    {
        stop = false;
        if (bgm == null)
        {
            bgm = change;
            audio.clip = bgm;
            bgm_name = change.ToString();
            volume_down = false;
            volume_up = true;
            audio.Play();
        }
        else
        {
            Debug.Log(change.ToString());
            if(bgm.ToString()!=change.ToString())
            {
                Debug.Log("cgs");
                StartCoroutine("volume_change",change);
            }
        }
    }

    public void music_stop()
    {
        //stop = true;
        audio.Stop();
        audio.clip = null;
        bgm_name = "";
        bgm = null;
        change_bgm = null;
        Debug.Log("stop");
    }
    public void volume_to_up()
    {
        //stop = false;
        volume_down = false;
        volume_up = true;
    }
    public void volume_to_down()
    {
        //stop = false;
        volume_down = true;
        volume_up = false;
    }

    IEnumerator volume_change(AudioClip change)
    {
        volume_to_down();
        Debug.Log("change");
        yield return new WaitForSeconds(1.50f);
        bgm = change;
        audio.clip = bgm;
        audio.Play();
        volume_to_up();
        yield return null;
    }
}
