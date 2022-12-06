using UnityEngine;
using System.Collections;

public class Npc_Text : MonoBehaviour {

    public TextAsset txt;
    public Text_manager t_manager;
    public PlayerController player;
    public int dir_x, dir_y;
    public bool can_talking;
    // Use this for initialization
    void Start () {
        t_manager = FindObjectOfType<Text_manager>();
        player = FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if(t_manager == null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if (can_talking)
        {
            if (t_manager.Player_moving && player.lastmove.x == dir_x && player.lastmove.y == dir_y)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    t_manager.Player_moving = false;
                    t_manager.text_enable(txt);
                    t_manager.textbox_enable();
                }
            }
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
}
