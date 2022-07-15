using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Life_text : MonoBehaviour // 플레이어 추리 가능 횟수 시각화 Script
{
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        set_life_text();
    }

    public void set_life_text() // 추리를 통해 줄어들기 때문에 Reasoning Script에서만 해당 함수를 사용
    {
        text.text = GameManager.gamemanager.get_life().ToString();
    }
}
