using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle_Command_Ok_Button_Script : MonoBehaviour, Command
{
    [SerializeField] private Button button;

    private bool condition_value = false;

    [SerializeField] private TextMeshProUGUI head_text;
    [SerializeField] private TextMeshProUGUI body_text;
    [SerializeField] private TextMeshProUGUI condition_text;

    [SerializeField] private GameObject information_frame;
    [SerializeField] private ScrollRect choose_command_scroll_rect;
    [SerializeField] private ScrollRect skill_command_scroll_rect;
    [SerializeField] private ScrollRect item_command_scroll_rect;

    public void set_condition_value(bool value)
    {
        condition_value = value;

        if(condition_value==false)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void set_head_text(string value)
    {
        head_text.text = value;
    }

    public void set_body_text(string value)
    {
        body_text.text = value;
    }

    public void set_condition_text_energy(int require_energy_value)
    {
        condition_text.text = "필요 에너지 : " + require_energy_value.ToString();
        DungeonManager.dungeonManager.set_require_energy(require_energy_value);

        if(DungeonManager.dungeonManager.ret_energy() >= require_energy_value)
        {
            condition_text.color = Color.white;
        }
        else
        {
            condition_text.color = Color.red;
        }
    }

    public void set_condition_text_skill(int require_energy, int turn_condtion, int number_possible, SkillScriptableObject information)
    {
        condition_text.text = "필요 에너지 : " + require_energy.ToString();
        DungeonManager.dungeonManager.set_require_energy(require_energy);

        if (DungeonManager.dungeonManager.ret_energy() >= require_energy)
        {
            condition_text.color = Color.white;
        }
        else
        {
            condition_text.color = Color.red;
        }

        if(information.ret_use_turn_condtion()==true)
        {
            int value = DungeonManager.dungeonManager.return_dungeon_turn() - turn_condtion;
            if (value < 0)
            {
                condition_text.text += " (" + (value * -1).ToString() + "턴 후 사용가능)";
                condition_text.color = Color.red;
            }
        }

        if(information.ret_use_possible_number()==true)
        {
            if(number_possible>0)
            {
                condition_text.text += " (" + number_possible.ToString() + "번 사용가능)";
            }
            else
            {
                condition_text.text += " (사용가능횟수 전부 소진)";
                condition_text.color = Color.red;
            }
        }
    }

    public void active_information_frame()
    {
        information_frame.SetActive(true);
    }

    public void Execute()
    {
        if(condition_value == true)
        {
            information_frame.SetActive(false);

            if(skill_command_scroll_rect.transform.parent.gameObject.activeSelf == true)
            {
                skill_command_scroll_rect.transform.parent.gameObject.SetActive(false);
                choose_command_scroll_rect.transform.parent.gameObject.SetActive(true);
            }

            if(item_command_scroll_rect.transform.parent.gameObject.activeSelf == true)
            {
                item_command_scroll_rect.transform.parent.gameObject.SetActive(false);
                choose_command_scroll_rect.transform.parent.gameObject.SetActive(true);
            }

            item_command_scroll_rect.verticalScrollbar.value = 1;
            skill_command_scroll_rect.verticalScrollbar.value = 1;
            choose_command_scroll_rect.verticalScrollbar.value = 1;
            
            DungeonManager.dungeonManager.Player_act();
        }
    }

    public void Dexecute()
    {

    }
}
