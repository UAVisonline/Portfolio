using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Cave_event_1 : MonoBehaviour {

    public TextAsset encounter;
    public GameObject parent, red_object , red_particle;
    public Animator anim;
    public PlayerController player;
    public Text_manager t_manager;
    public bool kill;
    public Bgm_manager bg_manager;
    public AudioClip BGM;
    // Use this for initialization
	void Start () {
       if(!PlayerPrefs.HasKey("cave_event_1"))
        {
            PlayerPrefs.SetInt("cave_event_1",0);
        }
       if(PlayerPrefs.GetInt("cave_event_1")>=1)
        {
            parent.SetActive(false);
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (kill == true && t_manager.Player_moving == true)
        {
            StartCoroutine("Event_1", 1f);
            kill = false;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            anim = GetComponentInParent<Animator>();
            player = other.GetComponent<PlayerController>();
            t_manager = FindObjectOfType<Text_manager>();
            bg_manager = FindObjectOfType<Bgm_manager>();
            bg_manager.music_change(BGM);
            player.player_cannot_move = true;
            anim.SetBool("Surprise", true);
            StartCoroutine("Event_0", 2f);
        }
    }

    IEnumerator Event_0(float time)
    {
        yield return new WaitForSeconds(time);
        t_manager.text_enable(encounter);
        kill = true;
    }

    IEnumerator Event_1(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(red_object, player.tf.transform.position, transform.rotation);
        //Instantiate(red_particle, player.tf.transform.position, transform.rotation);
        yield return new WaitForSeconds(2.4f);
        //bg_manager.music_stop();
        PlayerPrefs.SetInt("cave_event_1", 1);
        player.Destroy();
        SceneManager.LoadScene("game_over");
    }
}
