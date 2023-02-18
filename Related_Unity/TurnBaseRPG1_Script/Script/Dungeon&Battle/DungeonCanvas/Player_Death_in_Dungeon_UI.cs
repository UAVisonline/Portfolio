using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Death_in_Dungeon_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_1;
    [SerializeField] private TextMeshProUGUI text_2;

    private void OnEnable()
    {
        if(PlayerManager.playerManager.spec.max_chance >= PlayerManager.playerManager.spec.current_chance)
        {
            text_1.text = "아직 기회는 남아있습니다...";
            text_2.text = "더 강해져서 다시 도전해봅시다.";
        }
        else
        {
            text_1.text = "당신은 결국 모든 기회를 소모했습니다...";
            text_2.text = "다시 처음부터 해봅시다";
        }
    }

    public void btn_function()
    {
        if (PlayerManager.playerManager.spec.max_chance >= PlayerManager.playerManager.spec.current_chance)
        {
            SceneManagerCode.sceneManagerCode.Scene_move("Camp");
        }
        else
        {
            SceneManagerCode.sceneManagerCode.Scene_move("Main");
        }
    }
}
