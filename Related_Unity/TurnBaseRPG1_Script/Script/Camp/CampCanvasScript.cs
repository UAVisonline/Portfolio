using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampCanvasScript : MonoBehaviour
{
    [SerializeField] private DayLifescript daylifescript;
    [SerializeField] private Turn_chance_Window_Script turn_chance_script;


    private void Start()
    {
        visualize_current_turn_chance_window();
    }

    public void visualize_current_turn_chance_window()
    {
        if(PlayerManager.playerManager.same_ref_chance()==false) // 플레이어가 던전에서 죽었다 (이 경우가 아니면 발동되지 않음) -> 이거 수정해야 할 듯
        {
            if (PlayerManager.playerManager.spec.gameover_status == false)
            {
                daylifescript.gameObject.SetActive(true);
                daylifescript.Set_sentence_mode_2();
            }
            else
            {
                daylifescript.gameObject.SetActive(true);
                daylifescript.Set_sentence_mode_3();
            }
        }
        else // 플레이어가 던전에서 돌아왔다
        {
            daylifescript.gameObject.SetActive(true);
            daylifescript.Set_sentence_mode_1();
        }

        turn_chance_script.visualize();
    }

    public void visualize_current_chance_window()
    {
        if(PlayerManager.playerManager.same_ref_chance() == false) // 아무튼 플레이어가 죽었다 -> 이거 수정해야 할 듯
        {
            if(PlayerManager.playerManager.spec.gameover_status==false)
            {
                daylifescript.gameObject.SetActive(true);
                daylifescript.Set_sentence_mode_2();
            }
            else
            {
                daylifescript.gameObject.SetActive(true);
                daylifescript.Set_sentence_mode_3();
            }
        }

        turn_chance_script.visualize();
    }

    public void visualize_turn_chance()
    {
        turn_chance_script.visualize();
    }
}
