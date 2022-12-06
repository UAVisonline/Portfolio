using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Talk_battle_shamen : MonoBehaviour {

    public Text_manager t_manager;
    public PlayerController player;
    private Animator anim;
    public GameObject parent;
    public int dir_x, dir_y;
    public bool can_talking, go_battle;
    public TextAsset txt_first, txt_second, txt_third, txt_fourth, txt_fifth;
    //public string prefs;
    public AudioClip battle_bgm;
    // Use this for initialization
    void Start()
    {
        t_manager = FindObjectOfType<Text_manager>();
        player = FindObjectOfType<PlayerController>();
        if (parent != null)
        {
            anim = parent.GetComponent<Animator>();
        }
        if (anim == null)
        {
            anim = GetComponentInParent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (t_manager == null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if (can_talking)
        {
            if (t_manager.Player_moving && player.lastmove.x == dir_x && player.lastmove.y == dir_y && go_battle == false)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    t_manager.Player_moving = false;
                    player.player_cannot_move = true;
                    go_battle = true;
                    if(PlayerPrefs.GetInt("shamen_rage")==0)
                    {
                        PlayerPrefs.SetInt("shamen_rage", 1);
                        t_manager.text_enable(txt_first);
                    }
                    else
                    {
                        switch (PlayerPrefs.GetInt("current_death"))
                        {
                            case 0:
                                t_manager.text_enable(txt_second);
                                break;
                            case 1:
                                t_manager.text_enable(txt_second);
                                break;
                            case 2:
                                t_manager.text_enable(txt_third);
                                break;
                            case 3:
                                t_manager.text_enable(txt_fourth);
                                break;
                            default:
                                t_manager.text_enable(txt_fifth);
                                break;
                        }
                    }
                    Bgm_manager bg = FindObjectOfType<Bgm_manager>();
                   t_manager.textbox_enable();
                    bg.music_stop();
                }
            }
        }
        if(go_battle && t_manager.Player_moving)
        {    
            StartCoroutine("Event_0");
        }
    }

    void OnTriggerEnter2D()
    {
        can_talking = true;
        Debug.Log(can_talking);
    }

    void OnTriggerExit2D()
    {
        can_talking = false;
    }

    IEnumerator Event_0()
    {
        anim.SetBool("change", true);
        yield return new WaitForSeconds(3.0f);
        t_manager.fade_black_out_on();
        yield return new WaitForSeconds(1.0f);
        player.extra_save_position();
        player.Destroy();
        t_manager.sub_text.text = "자비따윈 없다. 지옥으로 떨어져 버려라.";
        yield return new WaitForSeconds(1.0f);
        t_manager.sub_text.text = "";
        yield return new WaitForSeconds(0.5f);
        t_manager.sub_text.text = "vs 주술사, 커즈";
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("shamen_battle");
    }
}
