using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle_Command_Set_Button : MonoBehaviour, Command
{
    [SerializeField] private TextMeshProUGUI btn_text;
    [SerializeField] private Button button;
    [SerializeField] private player_act_type act_type_value;

    [SerializeField] private int act_pos_value;

    [SerializeField] private Battle_Command_Ok_Button_Script ok_button_script;

    [SerializeField] private Command_information command_information;

    void OnEnable()
    {
        switch (act_type_value)
        {
            case player_act_type.normal_attack:
                // button.interactable = DungeonManager.dungeonManager.ret_energy_status();
                button.interactable = true;
                break;
            case player_act_type.defense:
                // button.interactable = DungeonManager.dungeonManager.ret_energy_status();
                button.interactable = true;
                break;
            case player_act_type.passing:
                button.interactable = true;
                break;
            case player_act_type.skill:
                button.interactable = InventoryManager.inventoryManager.ret_equiped_had_skill(act_pos_value);
                if(button.interactable == false)
                {
                    btn_text.text = "---";
                }
                else
                {
                    Skill temp = InventoryManager.inventoryManager.ret_equiped_skill(act_pos_value);
                    btn_text.text = temp.ret_skill_information().head;
                }
                break;
            case player_act_type.using_item:
                button.interactable = InventoryManager.inventoryManager.ret_consumption_had(act_pos_value);
                if (InventoryManager.inventoryManager.consumption_Items[act_pos_value] == null)
                {
                    btn_text.text = "---";
                }
                else
                {
                    btn_text.text = InventoryManager.inventoryManager.consumption_Items[act_pos_value].ret_item_name_text();
                }
                break;
        }
    }

    public void Execute()
    {
        switch (act_type_value)
        {
            case player_act_type.normal_attack:
                ok_button_script.set_condition_value(DungeonManager.dungeonManager.ret_energy_status());
                ok_button_script.set_head_text(command_information.head);
                ok_button_script.set_body_text(command_information.body);
                ok_button_script.set_condition_text_energy(command_information.require_energy);
                ok_button_script.active_information_frame();
                break;
            case player_act_type.defense:
                ok_button_script.set_condition_value(DungeonManager.dungeonManager.ret_energy_status());
                ok_button_script.set_head_text(command_information.head);
                ok_button_script.set_body_text(command_information.body);
                ok_button_script.set_condition_text_energy(command_information.require_energy);
                ok_button_script.active_information_frame();
                break;
            case player_act_type.skill:
                DungeonManager.dungeonManager.set_act_pos(act_pos_value);
                ok_button_script.set_condition_value(InventoryManager.inventoryManager.ret_equiped_skill_condtion(act_pos_value));
                Skill temp = InventoryManager.inventoryManager.ret_equiped_skill(act_pos_value);
                ok_button_script.set_head_text(temp.ret_skill_information().head);
                ok_button_script.set_body_text(temp.ret_skill_information().body);
                ok_button_script.set_condition_text_skill(temp.ret_need_energy(), temp.ret_turn_condtion(), temp.ret_possible_number(), temp.ret_skill_information());
                ok_button_script.active_information_frame();
                break;
            case player_act_type.using_item:
                DungeonManager.dungeonManager.set_act_pos(act_pos_value);
                //ok_button_script.set_condition_value(InventoryManager.inventoryManager.consumption_Items[act_pos_value].function_condition());
                bool item_condition_1 = InventoryManager.inventoryManager.consumption_Items[act_pos_value].function_condition();
                bool item_condition_2 = (InventoryManager.inventoryManager.consumption_Items[act_pos_value].ret_required_energy() <= DungeonManager.dungeonManager.ret_energy());
                ok_button_script.set_condition_value(item_condition_1 && item_condition_2);
                ok_button_script.set_head_text(InventoryManager.inventoryManager.consumption_Items[act_pos_value].ret_item_name_text());
                ok_button_script.set_body_text(InventoryManager.inventoryManager.consumption_Items[act_pos_value].ret_item_information_text());
                ok_button_script.set_condition_text_energy(InventoryManager.inventoryManager.consumption_Items[act_pos_value].ret_required_energy());
                ok_button_script.active_information_frame();
                break;
            case player_act_type.passing:
                ok_button_script.set_condition_value(true);
                ok_button_script.set_head_text(command_information.head);
                ok_button_script.set_body_text(command_information.body);
                ok_button_script.set_condition_text_energy(command_information.require_energy);
                ok_button_script.active_information_frame();
                break;
        }

        DungeonManager.dungeonManager.set_act_type(act_type_value);
    }

    public void Dexecute()
    {

    }
}
