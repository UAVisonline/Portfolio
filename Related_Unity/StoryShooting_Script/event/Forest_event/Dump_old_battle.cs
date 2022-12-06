using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Dump_old_battle : MonoBehaviour {

    public GameObject Parent;
    public Animator anim;
    public PlayerController player;
    public Text_manager t_manager;
    public TextAsset txt_0, txt_1, txt_2, death_txt_0, death_txt_1, death_txt_2;
    public bool first_text;
    public Bgm_manager bg_manager;
    public AudioClip normal_BGM, scary_BGM;
    // Use this for initialization
    void Start()
    {
        Parent = GameObject.Find("dump_old_standard");
        if (PlayerPrefs.HasKey("dump_old_battle"))
        {
            Parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (first_text && t_manager.Player_moving)
        {
            StartCoroutine("dump_old_battle", 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!PlayerPrefs.HasKey("dump_old_battle"))
        {
            if (col.tag == "Player")
            {
                //PlayerPrefs.SetInt("current_death", 0);
                anim = GetComponentInParent<Animator>();
                anim.SetBool("surprise", true);
                player = col.GetComponent<PlayerController>();
                t_manager = FindObjectOfType<Text_manager>();
                player.player_cannot_move = true;
                StartCoroutine("Event_0", 1.0f);
            }
        }
    }

    IEnumerator Event_0(float time)
    {
        int death = PlayerPrefs.GetInt("current_death");
        Debug.Log(death);
        //int young_result = PlayerPrefs.GetInt("dump_young_battle");
        yield return new WaitForSeconds(time);
        bg_manager = FindObjectOfType<Bgm_manager>();
        if(PlayerPrefs.GetInt("kill")==3)
        {
            bg_manager.music_change(scary_BGM);
            if(death==0)
            {
                t_manager.text_enable(death_txt_0);
            }
            else if(death ==1)
            {
                t_manager.text_enable(death_txt_1);
            }
            else if(death >=2)
            {
                t_manager.text_enable(death_txt_2);
            }
        }
        else
        {
            bg_manager.music_change(normal_BGM);
            if (death == 0)
            {
                t_manager.text_enable(txt_0);
            }
            else if (death == 1)
            {
                t_manager.text_enable(txt_1);
            }
            else if (death >= 2)
            {
                t_manager.text_enable(txt_2);
            }
        }
        first_text = true;
    }

    IEnumerator dump_old_battle(float time)
    {
        yield return new WaitForSeconds(time);
        t_manager.fade_white_out_on();
        first_text = false;
        yield return new WaitForSeconds(1.0f);
        int where_to_go = PlayerPrefs.GetInt("dump_young_battle");
        t_manager.sub_text.text = "vs 멍청이 괴물 목수";
        yield return new WaitForSeconds(1.5f);
        player.extra_save_position();
        player.Destroy();
        if(where_to_go==0)
        {
            SceneManager.LoadScene("dump_old_battle");
        }
        else
        {
            SceneManager.LoadScene("dump_old_battle_serious");
        }
        yield return null;
    }
}
