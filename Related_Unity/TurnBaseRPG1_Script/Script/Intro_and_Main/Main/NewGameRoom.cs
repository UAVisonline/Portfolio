using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGameRoom : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI origin_name;
    [SerializeField] private TextMeshProUGUI origin_information;

    [SerializeField] private Button start_btn;

    [SerializeField] private List<Origin_information> origin_obj;

    private Dictionary<Character_Origin, Origin_information> dictionary = new Dictionary<Character_Origin, Origin_information>(); 

    private int selected_origin;

    [SerializeField] private AudioClip warrior_sound;
    [SerializeField] private AudioClip assassin_sound;
    [SerializeField] private AudioClip wizard_sound;

    private void OnEnable()
    {
        image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        origin_name.text = "";
        origin_information.text = "";

        selected_origin = -1;
        start_btn.interactable = false;

        if(dictionary.Count==0)
        {
            for(int i = 0; i < origin_obj.Count; i++)
            {
                if(dictionary.ContainsKey(origin_obj[i].origin)==false)
                {
                    dictionary.Add(origin_obj[i].origin, origin_obj[i]);
                }
            }
        }
    }

    public void selected_origin_set(int code)
    {
        selected_origin = code;

        Character_Origin temp = (Character_Origin)code;

        switch(temp)
        {
            case Character_Origin.Warrior:
                Util_Manager.utilManager.play_clip(warrior_sound);
                break;
            case Character_Origin.Assassin:
                Util_Manager.utilManager.play_clip(assassin_sound);
                break;
            case Character_Origin.Wizard:
                Util_Manager.utilManager.play_clip(wizard_sound);
                break;
        }

        start_btn.interactable = true;

        origin_name.text = dictionary[temp].origin_name;
        origin_information.text = dictionary[temp].ret_origin_information();
        image.sprite = dictionary[temp].origin_sprite;
        image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void new_game_start()
    {
        //PlayerManager.playerManager.spec.gameover_status = true;

        Util_Manager.utilManager.button_click_sound_play();

        DungeonManager.dungeonManager.init_dungeon_struct(); // 던전 정보 초기화
        InventoryManager.inventoryManager.init_and_save_inventory(); // Inventory 초기화 및 저장 (아이템 삭제 과정을 해당 코드에 넣지 않음)
        PlayerManager.playerManager.make_spec(selected_origin); // 플레이어 정보 새로 생성

        SceneManagerCode.sceneManagerCode.Scene_move("Camp");
    }
}
