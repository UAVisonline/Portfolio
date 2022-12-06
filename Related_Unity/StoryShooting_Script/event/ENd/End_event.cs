using UnityEngine;
using System.Collections;

public class End_event : MonoBehaviour {

    public GameObject Child_1, Child_2;
    public int kill_event_sound_timing;
    public Text_manager t_manager;
    public PlayerController player;
    private bool event_condition, kill_event_condition, pass_event_condition;
    public TextAsset player_kill_man, player_forgive_man, normal,not_again, end_event;
    public AudioClip kill_audio;
	// Use this for initialization
	void Start () {
        //PlayerPrefs.DeleteKey("end_event");
        //Debug.Log(PlayerPrefs.GetInt("kid_boss_battle"));
        //PlayerPrefs.SetInt("kill",5);
        //Debug.Log(PlayerPrefs.GetInt("kill"));
	    if(PlayerPrefs.GetInt("end_event")==2)
        {
            Destroy(Child_1);
            Destroy(Child_2);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(kill_event_condition)
        {
            if (t_manager.currentLine == kill_event_sound_timing)
            {
                kill_event_sound_timing = -1;
                this.GetComponent<AudioSource>().PlayOneShot(kill_audio);
                t_manager.dialogue_time = 2.00f;
            } 
            if(t_manager.Player_moving)
            {
                PlayerPrefs.SetInt("end_event", 2);
                kill_event_condition = false;
                t_manager.fade_black_out_off();
                player.player_cannot_move = false;
            }
        }
        if(pass_event_condition)
        {
            if(t_manager.Player_moving)
            {
                event_condition = false;
                player.player_cannot_move = false;
                PlayerPrefs.SetInt("end_event", 3);
            }
        }
        if(event_condition && t_manager.Player_moving)
        {
            if (PlayerPrefs.GetInt("end_event") == 1)
            {
                event_condition = false;
                player.player_cannot_move = false;
                PlayerPrefs.SetInt("end_event", 3);
            }
            else
            {
                event_condition = false;
                player.player_cannot_move = false;
                player.player_move_world(player.transform.position.x, 5.8f, "forest_cave_to_end");
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)    
    {
        Debug.Log("xxx");
        if(other.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
            t_manager = FindObjectOfType<Text_manager>();
            if (!PlayerPrefs.HasKey("end_event"))
            {
                if (PlayerPrefs.GetInt("kill") == 6)
                {
                    StartCoroutine("kill_event");
                }
                else if (PlayerPrefs.GetInt("kill") == 0)
                {
                    StartCoroutine("pass_event");
                }
                else if(PlayerPrefs.GetInt("kill") != 0 && PlayerPrefs.GetInt("kill") != 6)
                {
                    StartCoroutine("end_event_text");
                }
            }
            else if (PlayerPrefs.GetInt("end_event") == 0 || PlayerPrefs.GetInt("end_event") == 1)
            {
                if (PlayerPrefs.GetInt("kill") == 6)
                {
                    StartCoroutine("kill_event");
                }
                else
                {
                    StartCoroutine("end_event_text");
                }
            }
        }
        
    }

    IEnumerator kill_event()
    {
        Debug.Log("aaa");
        t_manager.Player_moving = false;
        player.player_cannot_move = true;
        kill_event_condition = true;
        t_manager.fade_white_out_on();
        yield return new WaitForSeconds(2.0f);
        t_manager.text_enable(player_kill_man);
        Destroy(Child_1);
        Destroy(Child_2);
    }

    IEnumerator pass_event()
    {
        player.player_cannot_move = true;
        //event_condition = true;
        yield return new WaitForSeconds(0.5f);
        t_manager.text_enable(player_forgive_man);
        pass_event_condition = true;
    }
        

    IEnumerator end_event_text()
    {
        t_manager.Player_moving = false;
        player.player_cannot_move = true;
        event_condition = true;
        yield return new WaitForSeconds(0.5f);
        if(!PlayerPrefs.HasKey("end_event"))
        {
            PlayerPrefs.SetInt("end_event", 0);
            t_manager.text_enable(normal);
            
        }
        else if (PlayerPrefs.GetInt("end_event") == 1)
        {
            t_manager.text_enable(end_event);
        }
        else
        {
            t_manager.text_enable(not_again);
        }
        
    }
}
