using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestRoom : MonoBehaviour // Test 돌입 전 조건 확인
{
    [SerializeField] private Button test_btn;

    private void OnEnable()
    {
        if (VocaMaster.vocaMaster.get_count() < 20) // 단어장 내 단어가 20개 미만인 경우 (Test 진행 불가능)
        {
            test_btn.interactable = false;
        }
        else
        {
            test_btn.interactable = true;
        }
    }
}
