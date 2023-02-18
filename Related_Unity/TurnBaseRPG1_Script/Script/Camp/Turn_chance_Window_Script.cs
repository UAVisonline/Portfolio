using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Turn_chance_Window_Script : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turn_text;
    [SerializeField] private TextMeshProUGUI chance_text;
    [SerializeField] private TextMeshProUGUI stress_text;

    public void visualize()
    {
        if(PlayerManager.playerManager.spec.current_turn==PlayerManager.playerManager.spec.max_turn)
        {
            turn_text.color = Color.red;
        }
        else
        {
            turn_text.color = Color.white;
        }
        turn_text.text = "현재 턴 : " + PlayerManager.playerManager.spec.current_turn.ToString();


        if (PlayerManager.playerManager.spec.current_chance-1 == PlayerManager.playerManager.spec.max_chance-1)
        {
            chance_text.color = Color.red;
        }
        else
        {
            chance_text.color = Color.white;
        }
        chance_text.text = "현재 인생 : " + (PlayerManager.playerManager.spec.current_chance).ToString();


        if (PlayerManager.playerManager.spec.current_stress <= 30)
        {
            stress_text.color = Color.blue;
        }
        else if(PlayerManager.playerManager.spec.current_stress <= 70)
        {
            stress_text.color = Color.white;
        }
        else
        {
            stress_text.color = Color.red;
        }
        stress_text.text = "현재 스트레스 : " + PlayerManager.playerManager.spec.current_stress.ToString();
    }
}
