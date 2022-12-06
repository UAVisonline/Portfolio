using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Enemy : MonoBehaviour {

    public string battle_name;//PlayerPref
    public CameraController camera;
    public PlayerBattleController Player;
    public Text_manager t_manager;
    public TextAsset kill_dialouge, time_over_dialouge, you_kill_boss, you_forgive_boss ;
    public int first_health, second_health;
    private int original_first_health, original_second_health;//
    public float rage_time,courtine_time,next_scene_time;
    private float original_rage_time;
    public bool no_hit,choose_time, choose_situation, choose_die, health_die;
    private bool live, first_situation_enemy, second_situation_enemy, fight_end, prefsbool;
    public bool anim_health_die, anim_choose_die, anim_choose_live, end_anim;
    public AudioClip hit_clip;

    // Use this for initialization
    void Start () {
        Player = FindObjectOfType<PlayerBattleController>();
        t_manager = FindObjectOfType<Text_manager>();
        //t_manager.fade_black_out_off();
        original_first_health = first_health;
        original_second_health = second_health;
        original_rage_time = rage_time;
        live = true;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(PlayerPrefs.GetInt("kill"));
        if(camera == null)
        {
            camera = FindObjectOfType<CameraController>();
        }
        if(t_manager == null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
	    if(Player == null)
        {
            Player = FindObjectOfType<PlayerBattleController>();
        }
        if(first_health <= 0)
        {
            if(rage_time >= 0.0f && second_health> 0)
            {
                rage_time -= Time.deltaTime;
            } 
        }
        if(rage_time < 0.0f || second_health<= 0)
        {
            no_hit = true;
            if(!first_situation_enemy)
            {
                Player.ultimate_stop = true;
                first_situation_enemy = true;
                camera.not_player = true;
                Bgm_manager bg_manager = FindObjectOfType<Bgm_manager>();
                bg_manager.music_stop();
                if (second_health <= 0)
                {
                    live = false;
                    StartCoroutine(start_dialogue(courtine_time));
                }
                if (rage_time < 0.0f)
                {
                    live = true;
                    StartCoroutine(start_dialogue(courtine_time));
                }
            }  
        }
        if(choose_time && t_manager.Player_moving)
        {
            choose_situation = true;
            if(Input.GetKeyDown(KeyCode.Q))
            {
                live = true;
                choose_time = false;
                choose_situation = false;
                anim_choose_live = true;
                StartCoroutine(choose_dialogue(courtine_time));
            }
            if(Input.GetKeyDown(KeyCode.E))
            {
                live = false;
                choose_time = false;
                choose_situation = false;
                anim_choose_die = true;
                StartCoroutine(choose_dialogue(courtine_time));
            }
        }
        if(fight_end && t_manager.Player_moving)
        {    
            StartCoroutine(Scene_next(next_scene_time));
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player_Bullet" )
        {
            if(no_hit == false)
            {
                AudioSource fx = GetComponent<AudioSource>();
                fx.PlayOneShot(hit_clip);
                if (first_health > 0)
                {
                    first_health--;
                }
                if (first_health <= 0)
                {
                    if (second_health > 0)
                    {
                        second_health--;
                    }
                }
            }  
        }
    }

    IEnumerator start_dialogue(float time)
    {
        yield return new WaitForSeconds(time);
        t_manager = FindObjectOfType<Text_manager>();
        if(live)
        {
            t_manager.text_enable(time_over_dialouge);
            choose_time = true;
        }
        else
        {
            t_manager.text_enable(kill_dialouge);
            anim_health_die = true;
            
            fight_end = true;
        }
    }

    IEnumerator choose_dialogue(float time)
    {
        yield return new WaitForSeconds(time);
        t_manager = FindObjectOfType<Text_manager>();
        if(live)
        {
            t_manager.text_enable(you_forgive_boss);
            //anim_choose_live = true;
        }
        else
        {
            t_manager.text_enable(you_kill_boss);
            //anim_choose_die = true;
        }
        fight_end = true;
    }

    IEnumerator Scene_next(float time)
    {
        t_manager = FindObjectOfType<Text_manager>();
        PositionManager p_manager = FindObjectOfType<PositionManager>();
        end_anim = true;
        PlayerPrefs.SetInt("current_death", 0);
        if(!prefsbool)
        {
            if (live)
            {
                PlayerPrefs.SetInt(battle_name, 0);
            }
            else
            {
                PlayerPrefs.SetInt(battle_name, 1);
                PlayerPrefs.SetInt("kill", PlayerPrefs.GetInt("kill") + 1);
            }
            prefsbool = true;
        }
        yield return new WaitForSeconds(time);
        t_manager.fade_black_out_on();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(p_manager.scene_name);
    }

    public float first_health_return()
    {
        return ((float)first_health / (float)original_first_health);
    } 

    public float second_health_return()
    {
        return ((float)second_health / (float)original_second_health);
    }
}
