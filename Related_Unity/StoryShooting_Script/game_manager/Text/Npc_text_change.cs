using UnityEngine;
using System.Collections;

public class Npc_text_change : MonoBehaviour {

   
    public Text_manager t_manager;
    public PlayerController player;
    private Animator anim;
    public GameObject parent;
    public int dir_x, dir_y, prefs_value, event_text_correct_value;
    public bool can_talking;
    public TextAsset txt_first,txt_second;
    public string prefs;
    // Use this for initialization
    void Start()
    {
        t_manager = FindObjectOfType<Text_manager>();
        player = FindObjectOfType<PlayerController>();
        if(parent!=null)
        {
            anim = parent.GetComponent<Animator>();
        }
        if(anim == null)
        {
            anim = GetComponentInParent<Animator>();
        }
        //anim = GetComponent<Animator>();
        //PlayerPrefs.DeleteKey(prefs);
        if (PlayerPrefs.GetInt(prefs) == prefs_value)
        {
            if (anim != null)
            {
                anim.SetBool("change", true);
            }     
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
            if (t_manager.Player_moving && player.lastmove.x == dir_x && player.lastmove.y == dir_y)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    t_manager.Player_moving = false;
                    if(!PlayerPrefs.HasKey(prefs) || PlayerPrefs.GetInt(prefs) == event_text_correct_value)
                    {
                        t_manager.text_enable(txt_first);
                        PlayerPrefs.SetInt(prefs, prefs_value);
                    }
                    else
                    {
                        t_manager.text_enable(txt_second);
                    }
                    t_manager.textbox_enable();
                }
            }
            else if(!t_manager.Player_moving && player.lastmove.x == dir_x && player.lastmove.y == dir_y && t_manager.currentLine == t_manager.endLine)
            {
                if (anim != null)
                {
                    anim.SetBool("change", true);
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
