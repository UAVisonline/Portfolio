using UnityEngine;
using System.Collections;

public class Npc_Text_advanced : MonoBehaviour {

    public TextAsset txt;
    public Text_manager t_manager;
    public Animator anim;
    public PlayerController player;
    public int dir_x, dir_y, anim_dir_x, anim_dir_y;
    public bool can_talking;
    // Use this for initialization
    void Start()
    {
        t_manager = FindObjectOfType<Text_manager>();
        player = FindObjectOfType<PlayerController>();
        anim = GetComponentInParent<Animator>();
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
        if(anim == null)
        {
            anim = GetComponentInParent<Animator>();
        }
        if (can_talking)
        {
            if (t_manager.Player_moving && player.lastmove.x == dir_x && player.lastmove.y == dir_y)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    anim.SetFloat("x_dir", anim_dir_x);
                    anim.SetFloat("y_dir", anim_dir_y);
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

