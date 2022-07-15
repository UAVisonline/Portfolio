using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{

    [SerializeField] private GameObject OptionCanvas;

    public void Click_event()
    {
        if(OptionCanvas.activeSelf == false)
        {
            OptionCanvas.SetActive(true); // 옵션 Canvas를 True해서 화면에 보이게 함
        }
    }
}
