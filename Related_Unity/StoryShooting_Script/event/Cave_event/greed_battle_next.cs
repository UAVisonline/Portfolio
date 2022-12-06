using UnityEngine;
using System.Collections;

public class greed_battle_next : MonoBehaviour {

    public TextAsset kill_monster,save_monster;
    public Text_manager t_manager;
    public PlayerController player;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        t_manager = FindObjectOfType<Text_manager>();
        if (!PlayerPrefs.HasKey("greed_battle") || PlayerPrefs.GetInt("cave_puzzle_2")>=1)
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if (t_manager == null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
        if (!PlayerPrefs.HasKey("cave_puzzle_2"))
        {
            
            StartCoroutine("Time_to_text", 1.5f);
            //PlayerPrefs.SetInt("cave_puzzle_2", 0);
        }
    }

    IEnumerator Time_to_text(float time)
    {
        yield return new WaitForSeconds(0.8f);
        player.player_cannot_move = true;
        PlayerPrefs.SetInt("cave_puzzle_2", 0);
        yield return new WaitForSeconds(time);
        player.player_cannot_move = false;
        t_manager = FindObjectOfType<Text_manager>();
        if (PlayerPrefs.GetInt("greed_battle") == 0)
        {
            t_manager.text_enable(save_monster);
            player.player_cannot_move = false;
        }
        else
        {
            t_manager.text_enable(kill_monster);
            player.player_cannot_move = false;
        }
        yield return null;
    }
}
