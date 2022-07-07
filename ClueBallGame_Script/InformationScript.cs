using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationScript : MonoBehaviour
{
    [SerializeField] private Text try_text;
    [SerializeField] private Text standard_text;
    [SerializeField] private Text directional_text;
    [SerializeField] private Text correct_text;
    [SerializeField] private Text correct_standard_text;
    [SerializeField] private Text correct_directional_text;

    private void OnEnable() // 플레이어 정보 시각화 (PlayerPrefs를 통한 간단한 저장)
    {
        int try_number = PlayerPrefs.GetInt("Try");
        int standard = PlayerPrefs.GetInt("Standard");
        int directional = PlayerPrefs.GetInt("Directional");
        int correct = PlayerPrefs.GetInt("Correct");
        int correct_standard = PlayerPrefs.GetInt("Correct_standard");
        int correct_directional = PlayerPrefs.GetInt("Correct_directional");

        try_text.text = "- 게임 완료 횟수 : " + try_number.ToString();
        standard_text.text = "- 나열식 인터페이스로 게임 완료 횟수  : " + standard.ToString();
        directional_text.text = "- 방향형 인터페이스로 게임 완료 횟수 : " + directional.ToString();
        correct_text.text = "- 게임 승리 횟수 : " + correct.ToString();
        correct_standard_text.text = "- 나열식 인터페이스로 게임 승리 횟수 : " + correct_standard.ToString();
        correct_directional_text.text = "- 방향형 인터페이스로 게임 승리 횟수 : " + correct_directional.ToString();
    }
}
