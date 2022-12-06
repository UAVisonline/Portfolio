using UnityEngine;
using System.Collections;

public class Cave_event_1_2 : MonoBehaviour {

    public float down_time;
    public bool go_down, first_text;
    public Animator anim;
    public GameObject Parent;
    public TextAsset encounter, worry_about_player;
    public PlayerController player;
    public Text_manager t_manager;
    public Transform tf;
	// Use this for initialization
	void Start () {
        //PlayerPrefs.SetInt("cave_event_1", 1);
	if(!PlayerPrefs.HasKey("cave_event_1"))
        {
            Parent.SetActive(false);
            gameObject.SetActive(false);
        }
    if(PlayerPrefs.GetInt("cave_event_1")!=1)
        {
            Parent.SetActive(false);
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	if(first_text && t_manager.Player_moving)
        {
            StartCoroutine("Event_1", 2f);
        }
    if(go_down && t_manager.Player_moving)
        {
            if(down_time >= 0.0f)
            {
                down_time -= Time.deltaTime;
            }
            if(down_time < 0.0f)
            {
                anim.SetBool("Go_down", go_down);
                StartCoroutine("Event_2");
            }
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            anim = GetComponentInParent<Animator>();
            player = col.GetComponent<PlayerController>();
            t_manager = FindObjectOfType<Text_manager>();
            tf = Parent.GetComponent<Transform>();
            player.player_cannot_move = true;
            anim.SetBool("Surprise", true);
            StartCoroutine("Event_0", 2f);
        }
    }

    IEnumerator Event_0(float time)
    {
        yield return new WaitForSeconds(time);
        t_manager.text_enable(encounter);
        first_text = true;
        yield return null;
    }

    IEnumerator Event_1(float time)
    {
        yield return new WaitForSeconds(time);
        first_text = false;
        t_manager.text_enable(worry_about_player);
        go_down = true;
        yield return null;
    }

    IEnumerator Event_2()
    {
        yield return new WaitForSeconds(1.2f);
        player.player_cannot_move = false;
        PlayerPrefs.SetInt("cave_event_1", 2);
        Parent.SetActive(false);
        yield return null;
        //PlayerPrefs.SetInt("greed_battle", 0);
    }
}
