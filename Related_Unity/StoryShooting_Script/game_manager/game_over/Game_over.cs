using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game_over : MonoBehaviour {
    public PositionManager p_manager;
    public Text_manager t_manager;
    public Text game_over_text, Load_Exit;
    public float game_over_time, load_time,game_over_time_original;
    public int select;
    public AudioClip select_sound;
    public Bgm_manager bg_manager;
	// Use this for initialization
	void Start () {
        p_manager = FindObjectOfType<PositionManager>();
        t_manager = FindObjectOfType<Text_manager>();
        bg_manager = FindObjectOfType<Bgm_manager>();
        bg_manager.music_stop();
        t_manager.sub_text.text = "";
        game_over_time_original = game_over_time;
        game_over_time = 0.0f;
        select = 0;
	}
	
	// Update is called once per frame
	void Update () {
        game_over_text.color = new Color(255.0f, 255.0f, 255.0f, game_over_time / game_over_time_original);
        if(bg_manager == null)
        {
            bg_manager = FindObjectOfType<Bgm_manager>();
            bg_manager.music_stop();
        }
	    if(game_over_time<game_over_time_original)
        {
            game_over_time += Time.deltaTime;
        }
        if (game_over_time>=game_over_time_original)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                select = 1;
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                select = 0;
            }
            if(select == 0)
            {
                Load_Exit.text = ">>>Load Game             Main Menu";
            }
            else
            {
                Load_Exit.text = "Load Game             >>>Main Menu";
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(scene_move(1f));
                AudioSource ad = GetComponent<AudioSource>();
                ad.PlayOneShot(select_sound);
                //scene_move(select);
            }
        }
	}

    IEnumerator scene_move(float time)
    {
        p_manager = FindObjectOfType<PositionManager>();
        p_manager.load_position_data();
        t_manager = FindObjectOfType<Text_manager>();
        t_manager.fade_black_out_on();
        yield return new WaitForSeconds(time*2);
        if(select == 0)
        {
            SceneManager.LoadScene(p_manager.scene_name);
        }
        if(select == 1)
        {
            SceneManager.LoadScene("Main_Screen");
        }
    }
}
