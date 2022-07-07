using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour
{
    public void click_event() // 게임 종료 버튼
    {
        Application.Quit();
    }
}
