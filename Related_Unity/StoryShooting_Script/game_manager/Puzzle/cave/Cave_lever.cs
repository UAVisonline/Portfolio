using UnityEngine;
using System.Collections;

public class Cave_lever : MonoBehaviour {

    public bool lever_down;
    public int lever_num;
    public bool can_do_it;
    public Cave_puzzle1_manager cave_manager;
    public PlayerController p_controller;
    public Text_manager t_manager;
    public int x_dir, y_dir;
    private Animator anim;
    public AudioClip lever_sound;
    // Use this for initialization

    void Start () {
        anim = GetComponentInParent<Animator>();
        cave_manager = FindObjectOfType<Cave_puzzle1_manager>();
        p_controller = FindObjectOfType<PlayerController>();
        t_manager = FindObjectOfType<Text_manager>();
        can_do_it = false;
        if(cave_manager.puzzle)
        {
            lever_down = false;
        }
        else
        {
            lever_down = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Lever_Down",lever_down);
        if(p_controller == null)
        {
            p_controller = FindObjectOfType<PlayerController>();
        }
        if(cave_manager.where_put == 0)
        {
            lever_down = false;
        }
        if(t_manager.Player_moving)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (can_do_it && p_controller.lastmove.x == x_dir && p_controller.lastmove.y == y_dir)
                {
                    if (!lever_down)
                    {
                        AudioSource ad = GetComponent<AudioSource>();
                        ad.PlayOneShot(lever_sound);
                        lever_down = true;
                        cave_manager.lever[cave_manager.where_put] = lever_num;
                        cave_manager.where_put++;
                    }
                }
            }
        }
        if(cave_manager.where_put == 5)
        {
            lever_down = true;
        }
	}

    void OnTriggerEnter2D()
    {
        can_do_it = true;
    }

    void OnTriggerExit2D()
    {
        can_do_it = false;
    }
}
