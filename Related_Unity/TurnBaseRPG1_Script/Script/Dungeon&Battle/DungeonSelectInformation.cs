using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DungeonSelectInformation : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI dungeon_name_text;
    [SerializeField] private TextMeshProUGUI dungeon_level_text;

    public DungeonSelect select_information;
    public Sprite dungeon_sprite;

    [SerializeField] private DungeonSelectRoom select_room;

    private void Start()
    {
        image.sprite = dungeon_sprite;
        dungeon_name_text.text = select_information.ret_name();
        dungeon_level_text.text = "Level : " + select_information.ret_dungeon_level().ToString();
    }

    public void btn_function()
    {
        DungeonManager.dungeonManager.set_dungeon_select(select_information);
        select_room.selected_dungeon_text_set(select_information.ret_name());
        select_room.start_button_true();
    }
}
