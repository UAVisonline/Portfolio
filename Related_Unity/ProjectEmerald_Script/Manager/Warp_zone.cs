using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp_zone : MonoBehaviour
{
    public float pos_x, pos_y;
    public string warp_scene;
    public bool warpzone;
    private bool ready_to_warp;
    // Start is called before the first frame update
    void Start()
    {
        ready_to_warp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ready_to_warp)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Dramatic_UI.dramatic_manager.Warp_scene(warp_scene);
                Player_Controller.player_controller.Set_Can_hurt(false);
                Player_Manager.player_manager.set_position(pos_x, pos_y, Player_Controller.player_controller.Return_left());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("aaa");
        if(warpzone)
        {
            if (collision.tag == "Player")
            {
                //Dramatic_UI.dramatic_manager.fade_in();
                Warp();
            }
        }
        if(!warpzone)
        {
            if (collision.tag == "Player")
            {
                this.GetComponent<Animator>().SetBool("door", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!warpzone)
        {
            if (collision.tag == "Player")
            {
                this.GetComponent<Animator>().SetBool("door", false);
            }
        }
    }

    private void Warp()
    {
        Dramatic_UI.dramatic_manager.Warp_scene(warp_scene);
        Player_Controller.player_controller.Set_Can_hurt(false);
        Player_Manager.player_manager.set_position(pos_x, pos_y, Player_Controller.player_controller.Return_left());
    }

    private void Animation_warp_ready()
    {
        ready_to_warp = true;
    }

    private void Animation_warp_unready()
    {
        ready_to_warp = false;
    }
}
