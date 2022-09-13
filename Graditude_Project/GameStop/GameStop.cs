using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStop : MonoBehaviour // 그 게임화면 오른쪽 위에 있는 게임 재시작 UI
{
    [SerializeField] private Animator animator;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    private bool text_on = false;
    private bool status = false;

    public void interaction(bool value)
    {
        if(value==false)
        {
            off_frame();
        }
        else
        {
            on_frame();
        }
    }

    private void on_frame()
    {
        if (animator == null)
        {
            animator = this.GetComponent<Animator>();
        }
        animator.SetBool("On", true);

        if(text_on == false)
        {
            text_on = true;
            text.text = GameManager.gamemanager.get_song_title();
        }
        
        if(GameManager.gamemanager.get_multiplay_status()==true)
        {
            button.interactable = false;
        }
    }

    private void off_frame()
    {
        if (animator == null)
        {
            animator = this.GetComponent<Animator>();
        }
        animator.SetBool("On", false);
    }

    public void click_event()
    {
        GameManager.gamemanager.Game_stop();
    }
}
