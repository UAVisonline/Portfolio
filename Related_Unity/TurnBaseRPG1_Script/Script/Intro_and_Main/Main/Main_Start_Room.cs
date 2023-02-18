using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main_Start_Room : MonoBehaviour
{
    [SerializeField] private Image new_background;
    [SerializeField] private Image continue_background;

    [SerializeField] private Image new_game_image;

    [SerializeField] private Image continue_image;
    [SerializeField] private Button continue_button;
    [SerializeField] private TextMeshProUGUI continue_text;

    private void OnEnable()
    {
        if(PlayerManager.playerManager.spec.gameover_status==false && PlayerManager.playerManager.spec.origin != Character_Origin.None)
        {
            continue_button.interactable = true;
            continue_text.text = "이전 게임을 계속해서 \n 진행합니다.";
            continue_text.color = Color.white;
        }
        else
        {
            continue_button.interactable = false;
            continue_text.text = "이어할 수 있는 게임이 \n 없습니다.";
            continue_text.color = Color.red;
        }

        new_background.color = new Color(0.33f, 0.33f, 0.33f, 0.3f);
        continue_background.color = new Color(0.33f, 0.33f, 0.33f, 0.3f);
        new_game_image.color = new Color(0.33f, 0.33f, 0.33f, 1.0f);
        continue_image.color = new Color(0.33f, 0.33f, 0.33f, 1.0f);
    }

    public void continue_btn()
    {
        Util_Manager.utilManager.button_click_sound_play();

        if(DungeonManager.dungeonManager.ret_in_dungeon()==false)
        {
            SceneManagerCode.sceneManagerCode.Scene_move("Camp");
        }
        else
        {
            SceneManagerCode.sceneManagerCode.Scene_move("Dungeon");
        }
    }

    public void continue_on()
    {
        new_background.color = new Color(0.33f, 0.33f, 0.33f, 0.3f);
        continue_background.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
        new_game_image.color = new Color(0.33f, 0.33f, 0.33f, 1.0f);
        continue_image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void new_game_on()
    {
        new_background.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
        continue_background.color = new Color(0.33f, 0.33f, 0.33f, 0.3f);
        new_game_image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        continue_image.color = new Color(0.33f, 0.33f, 0.33f, 1.0f);
    }

    public void both_off()
    {
        new_background.color = new Color(0.33f, 0.33f, 0.33f, 0.3f);
        continue_background.color = new Color(0.33f, 0.33f, 0.33f, 0.3f);
        new_game_image.color = new Color(0.33f, 0.33f, 0.33f, 1.0f);
        continue_image.color = new Color(0.33f, 0.33f, 0.33f, 1.0f);
    }
}
