using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class greed_battle : MonoBehaviour {

    public GameObject Parent;
    public Animator anim;
    public PlayerController player;
    public Text_manager t_manager;
    public bool first_text;
    public TextAsset txt1, txt2, txt3, txt4;
    public Bgm_manager bg_manager;
    public AudioClip BGM;
    // Use this for initialization
    void Start () {
        Parent = GameObject.Find("greed_monster-battle");
        //PlayerPrefs.DeleteKey("greed_battle");
        if (PlayerPrefs.HasKey("greed_battle"))
        {
            Parent.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	if(first_text&&t_manager.Player_moving)
        {
            StartCoroutine("battle_start", 0.5f);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!PlayerPrefs.HasKey("greed_battle"))
        {
            if (other.tag == "Player")
            {
                anim = GetComponentInParent<Animator>();
                player = other.GetComponent<PlayerController>();
                t_manager = FindObjectOfType<Text_manager>();
                bg_manager = FindObjectOfType<Bgm_manager>();
                bg_manager.music_stop();
                anim.SetBool("Surprise", true);
                player.player_cannot_move = true;
                StartCoroutine("Event_0", 1f);
            }
        }
    }

    IEnumerator Event_0(float time)
    {
        yield return new WaitForSeconds(time);
        int death = PlayerPrefs.GetInt("current_death");
        bg_manager.music_change(BGM);
        if(death==0)
        {
            t_manager.text_enable(txt1);
        }
        else if(death==1)
        {
            t_manager.text_enable(txt2);
        }
        else if(death==2)
        {
            t_manager.text_enable(txt3);
        }
        else
        {
            t_manager.text_enable(txt4);
        }
        first_text = true;
    }

    IEnumerator battle_start(float time)
    {
        yield return new WaitForSeconds(time);
        t_manager.fade_white_out_on();
        first_text = false;
        yield return new WaitForSeconds(1.0f);
        t_manager.sub_text.text = "vs 욕심쟁이 몬스터";
        yield return new WaitForSeconds(1.5f);
        player.extra_save_position();
        player.Destroy();
        SceneManager.LoadScene("greed_battle");
    }
}
