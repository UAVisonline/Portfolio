using UnityEngine;
using System.Collections;

public class Npc_text_testing : MonoBehaviour {

    public TextAsset txt;
    public Text_manager t_manager;
    public testing_player_controller player_manager;
    public int dir_x, dir_y;
    public bool can_talking;
	// Use this for initialization
	void Start () {
        t_manager = FindObjectOfType<Text_manager>();
        player_manager = FindObjectOfType<testing_player_controller>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(can_talking)
        {
            if(t_manager.Player_moving && player_manager.lastmove.x == dir_x && player_manager.lastmove.y == dir_y)
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    t_manager.Player_moving = false;
                    t_manager.text_enable(txt);
                    t_manager.textbox_enable();
                    Debug.Log("aaa");
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
