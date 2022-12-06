using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game_end : MonoBehaviour {

    public float credit_time;
    private int kill_total;
    public PlayerController player;
    public Text_manager t_manager;
    public TextAsset all_forgive, all_kill, only_greed_kill, only_reaper_kill, only_kid_kill, only_dump_young_kill, only_dump_old_kill, two_kill, dump_brother_kill, human_kill;
    public TextAsset three_kill, monster_kill, four_kill , five_kill, error_message;
    private bool first_text;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        if(player!=null)
        {
            Destroy(player.gameObject);
        }
        kill_total = 0;
        StartCoroutine("game_end");
	}
	
	// Update is called once per frame
	void Update () {
	    if(t_manager==null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
        else
        {
            if(t_manager.fade_on)
            {
                t_manager.fade_black_out_off();
            }
        }
        if(first_text && t_manager.Player_moving)
        {
            if(credit_time>0.0f)
            {
                credit_time -= Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene("credit");
            } 
        }
	}

    IEnumerator game_end()
    {
        kill_total = PlayerPrefs.GetInt("greed_battle") + PlayerPrefs.GetInt("reaper_battle") + PlayerPrefs.GetInt("dump_young_battle") + PlayerPrefs.GetInt("dump_old_battle") + PlayerPrefs.GetInt("kid_boss_battle") + PlayerPrefs.GetInt("shamen_battle");
        yield return new WaitForSeconds(1.5f);
        if(kill_total != PlayerPrefs.GetInt("kill"))
        {
            t_manager.text_enable(error_message);
        }
        else if(PlayerPrefs.GetInt("kill") == 6)
        {
            t_manager.text_enable(all_kill);
        }
        else if(PlayerPrefs.GetInt("kill") == 0)
        {
            t_manager.text_enable(all_forgive);
        }
        else if(PlayerPrefs.GetInt("kill") == 5)
        {
            t_manager.text_enable(five_kill);
        }
        else if(PlayerPrefs.GetInt("kill") == 4)
        {
            t_manager.text_enable(four_kill);
        }
        else if(PlayerPrefs.GetInt("kill") == 3)
        {
            if(PlayerPrefs.GetInt("greed_battle")==1 && PlayerPrefs.GetInt("dump_young_battle")==1&& PlayerPrefs.GetInt("dump_old_battle")==1)
            {
                t_manager.text_enable(monster_kill) ;
            }
            else
            {
                t_manager.text_enable(three_kill);
            }
        }
        else if (PlayerPrefs.GetInt("kill") == 2)
        {
            if (PlayerPrefs.GetInt("dump_young_battle") == 1 && PlayerPrefs.GetInt("dump_old_battle") == 1)
            {
                t_manager.text_enable(dump_brother_kill);
            }
            else if(PlayerPrefs.GetInt("kid_boss_battle") == 1 && PlayerPrefs.GetInt("reaper_battle") == 1)
            {
                t_manager.text_enable(human_kill);
            }
            else
            {
                t_manager.text_enable(two_kill);
            }
        }
        else if (PlayerPrefs.GetInt("kill") == 1)
        {
            if(PlayerPrefs.GetInt("dump_young_battle") == 1 || PlayerPrefs.GetInt("dump_old_battle") == 1)
            {
                t_manager.text_enable(only_dump_old_kill);
            }
            else if(PlayerPrefs.GetInt("greed_battle") == 1)
            {
                t_manager.text_enable(only_greed_kill);
            }
            else if (PlayerPrefs.GetInt("kid_boss_battle") == 1)
            {
                t_manager.text_enable(only_kid_kill);
            }
            else if (PlayerPrefs.GetInt("reaper_battle") == 1)
            {
                t_manager.text_enable(only_reaper_kill);
            }
        }
        first_text = true;
    }
}
