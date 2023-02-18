using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clear_Script : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clear_text_1;
    [SerializeField] private TextMeshProUGUI clear_text_2;
    [SerializeField] private TextMeshProUGUI clear_text_3;

    private void OnEnable()
    {
        clear_text_1.text = (PlayerManager.playerManager.spec.current_turn).ToString() + "턴";
        clear_text_2.text = (PlayerManager.playerManager.spec.current_chance).ToString() + "인생";
        clear_text_3.text = "만에 저주를 풀었습니다.";
    }

    public void btn_function()
    {
        PlayerManager.playerManager.spec.gameover_status = true;
        PlayerManager.playerManager.save_spec();

        SceneManagerCode.sceneManagerCode.Scene_move("Main");
    }
}
