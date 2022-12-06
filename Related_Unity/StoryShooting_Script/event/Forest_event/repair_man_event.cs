using UnityEngine;
using System.Collections;

public class repair_man_event : MonoBehaviour {

    public GameObject Parent;
    public Animator anim;
    public Text_manager t_manager;
    public TextAsset one_talk, no_talk;
    public bool event_end, fade_end;
	// Use this for initialization
	void Start () {
        //PlayerPrefs.SetInt("bridge", 2);
        t_manager = FindObjectOfType<Text_manager>();
        Parent = GameObject.Find("Npc_repair_man_left");
        anim = Parent.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	if(event_end && t_manager.Player_moving)
        {
            if(!fade_end)
            {
                PlayerPrefs.SetInt("bridge", 3);
                fade_end = true;
                StartCoroutine("fade");
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(PlayerPrefs.GetInt("bridge")<=3)
            {
                player.player_cannot_move = true;
                t_manager = FindObjectOfType<Text_manager>();
                anim.SetBool("surprise", true);
                StartCoroutine("text_event");
            }
        }
    }

    IEnumerator text_event()
    {
        yield return new WaitForSeconds(1.0f);
        if (PlayerPrefs.GetInt("repair_man_meet") == 0)
        {
            t_manager.text_enable(no_talk);
            event_end = true;
        }
        else if (PlayerPrefs.GetInt("repair_man_meet") == 1)
        {
            t_manager.text_enable(one_talk);
            event_end = true;
        }
    }

    IEnumerator fade()
    {
        t_manager.fade_black_out_on();
        yield return new WaitForSeconds(2.5f);
        PlayerController player = FindObjectOfType<PlayerController>();
        player.player_cannot_move = false;
        t_manager.fade_black_out_off();
        Destroy(Parent);
    }
}
