using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionApplyButton : MonoBehaviour
{
    [SerializeField] private GameObject OptionCanvas;

    public void apply_option() // 옵션 반영
    {
        OptionCanvas.SetActive(false); // 옵션 Canvas를 Active False
        GameManager.gamemanager.apply_resolution(); // 해상도를 실제로 변경
    }
}
