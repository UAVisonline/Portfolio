using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonSelectRoom : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI selected_dungeon_text;

    [SerializeField] private Button start_button;

    private void OnEnable()
    {
        DungeonManager.dungeonManager.null_dungeon_select();
        selected_dungeon_text.text = "";
        start_button.interactable = false;
    }

    public void btn_function()
    {
        DungeonManager.dungeonManager.making_dungeon_struct_and_Move_Scene();
    }

    public void start_button_true()
    {
        start_button.interactable = true;
    }

    public void selected_dungeon_text_set(string name)
    {
        selected_dungeon_text.text = "Selected : " + name;
    }
}
