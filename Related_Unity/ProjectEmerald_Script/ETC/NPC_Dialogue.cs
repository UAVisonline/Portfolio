using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC_Dialogue : MonoBehaviour
{
    [SerializeField] private bool Save_zone;
    [SerializeField] private Dialogue dialogue;
    private Animator animator;
    private bool Can_talk, talk_ready;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        Can_talk = false;
        talk_ready = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Can_talk)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && Player_Controller.player_controller.Is_State_Can_Talking() && talk_ready == true)
            {
                if (!DialogueSystem.dialogue_System.Dialogue_status() && Player_Controller.player_controller.Return_Can_Move())
                {
                    if(Save_zone)
                    {
                        Save();
                    }
                    DialogueSystem.dialogue_System.Dialogue_input(dialogue);
                }
            }
        }
        if (Can_talk && !DialogueSystem.dialogue_System.Dialogue_status())
        {
            animator.SetBool("Talk_On", true);
        }
        else
        {
            animator.SetBool("Talk_On", false);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "interaction")
        {
            Can_talk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "interaction")
        {
            Can_talk = false;
        }
    }

    private void Ready_Animation()
    {
        talk_ready = true;
    }

    private void Unready_Animation()
    {
        talk_ready = false;
    }

    private void Save()
    {
        if(PlayerPrefs.GetString("Save")=="false" || !PlayerPrefs.HasKey("Save"))
        {
            PlayerPrefs.SetString("Save", "true");
        }
        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
        Debug.Log(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("x_pos", Player_Controller.player_controller.return_x_pos());
        PlayerPrefs.SetFloat("y_pos", Player_Controller.player_controller.return_y_pos());
        Player_Manager.player_manager.Refill();
    }
}
