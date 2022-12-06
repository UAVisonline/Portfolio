using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class kid_boss_event : MonoBehaviour {

    public GameObject parent;
    public PlayerController player;
    public Animator anim;
    public bool first_text;
    public Text_manager t_manager;
    public TextAsset txt_0, txt_1, txt_2, kill_txt_0, kill_txt_1,kill_txt_2;
    public AudioClip battle_bgm;
	// Use this for initialization
	void Start () {
        //PlayerPrefs.DeleteKey("meet_kid");
        parent = GameObject.Find("kid_boss_standard");
        if (PlayerPrefs.HasKey("meet_kid"))
        {
            anim = parent.GetComponent<Animator>();
            anim.SetBool("meet", true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(t_manager==null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
        if (t_manager.Player_moving && first_text)
        {
            StartCoroutine("kid_boss_battle");
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
            if(!PlayerPrefs.HasKey("kid_boss_battle"))
            {
                player.player_cannot_move = true;
                Bgm_manager bg_manager = FindObjectOfType<Bgm_manager>();
                bg_manager.music_stop();
                t_manager = FindObjectOfType<Text_manager>();
                if (!PlayerPrefs.HasKey("meet_kid"))
                {
                    PlayerPrefs.SetInt("meet_kid", 0);
                    anim = parent.GetComponent<Animator>();
                    anim.SetBool("surprise", true);
                }
                StartCoroutine("Event_0");
            }
        }
    }

    IEnumerator Event_0()
    {
        yield return new WaitForSeconds(1.50f);
        int death = PlayerPrefs.GetInt("current_death");
        Debug.Log(death);
        if(PlayerPrefs.GetInt("kill")>=4)
        {
            switch (death)
            {
                case 0:
                    t_manager.text_enable(kill_txt_0);
                    break;
                case 1:
                    t_manager.text_enable(kill_txt_1);
                    break;
                default:
                    t_manager.text_enable(kill_txt_2);
                    break;
            }
        }
        else
        {
            switch (death)
            {
                case 0:
                    t_manager.text_enable(txt_0);
                    break;
                case 1:
                    t_manager.text_enable(txt_1);
                    break;
                default:
                    t_manager.text_enable(txt_2);
                    break;
            }
        }
        first_text = true;
    }

    IEnumerator kid_boss_battle()
    {
        yield return new WaitForSeconds(0.75f);
        t_manager.fade_white_out_on();
        first_text = false;
        Bgm_manager bg_manager = FindObjectOfType<Bgm_manager>();
        bg_manager.music_change(battle_bgm);
        yield return new WaitForSeconds(1.0f);
        t_manager.sub_text.text = "vs 사람들을 지키는 꼬마 대장, 리차드";
        yield return new WaitForSeconds(1.5f);
        player.extra_save_position();
        player.Destroy();
        SceneManager.LoadScene("kid_boss_battle");
        yield return null;
    }
}
