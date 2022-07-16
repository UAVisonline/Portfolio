using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC_Dialogue_Advanced : MonoBehaviour
{
    [SerializeField] private Dialogue[] dialogue;
    private Animator animator;
    private bool Can_talk, talk_ready;
    [SerializeField] private string important_event_name;
    [SerializeField] private int[] evevt_value;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        Can_talk = false;
        talk_ready = false;
        if(!PlayerPrefs.HasKey(important_event_name) && important_event_name!="")
        {
            PlayerPrefs.SetInt(important_event_name, 0);
        }
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
                    DialogueSystem.dialogue_System.Dialogue_input(dialogue[PlayerPrefs.GetInt(important_event_name)]);
                    for(int i = 0;i<evevt_value.Length;i++)
                    {
                        if(i== PlayerPrefs.GetInt(important_event_name))
                        {
                            PlayerPrefs.SetInt(important_event_name, evevt_value[PlayerPrefs.GetInt(important_event_name)]);
                            break;
                        }
                    } 
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
}