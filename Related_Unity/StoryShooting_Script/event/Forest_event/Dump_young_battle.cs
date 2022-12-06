using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Dump_young_battle : MonoBehaviour {
    public GameObject Parent;
    public Animator anim;
    public PlayerController player;
    public Text_manager t_manager;
    public TextAsset txt_0, txt_1, txt_2;
    public bool first_text;
    public Bgm_manager bg_manager;
    public AudioClip BGM;
    // Use this for initialization
    void Start () {
        Parent = GameObject.Find("dump_young__battle");
        if (PlayerPrefs.HasKey("dump_young_battle"))
        {
            Parent.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (first_text && t_manager.Player_moving)
        {
            StartCoroutine("dump_young_battle", 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!PlayerPrefs.HasKey("dump_young_battle"))
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
        yield return new WaitForSeconds(time);
        bg_manager = FindObjectOfType<Bgm_manager>();
        bg_manager.music_change(BGM);
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
        first_text = true;
    }

    IEnumerator dump_young_battle(float time)
    {
        yield return new WaitForSeconds(time);
        t_manager.fade_white_out_on();
        first_text = false;
        yield return new WaitForSeconds(1.0f);
        t_manager.sub_text.text = "vs 멍청한 괴물 형제, 그 동생";
        yield return new WaitForSeconds(1.5f);
        player.extra_save_position();
        player.Destroy();
        SceneManager.LoadScene("dump_young_battle");
        yield return null;
    }
}
