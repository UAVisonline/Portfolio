using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Left_right_move_script : MonoBehaviour // 게임방법, Credit과 같은 좌우이동 버튼을 통한 정보 공개에 대하여
{
    [SerializeField] private List<GameObject> gameobjects; // 보여줄지 말지 설정하는 GameObject

    [SerializeField] private int index; // 현재 보여주는 gameobject index

    [SerializeField] private Button left_button;
    [SerializeField] private Button right_button;

    private void OnEnable()
    {
        set_interactable();
    }

    public void left_click() // index - 1
    {
        gameobjects[index].SetActive(false);
        index -= 1;
        gameobjects[index].SetActive(true);
        GameManager.gamemanager.click_play();
        set_interactable();
    }

    public void right_click() // index + 1
    {
        gameobjects[index].SetActive(false);
        index += 1;
        gameobjects[index].SetActive(true);
        GameManager.gamemanager.click_play();
        set_interactable();
    }

    private void set_interactable() // 현재 index에 따른 Left, Right button interactable 설정
    {
        if (index == 0)
        {
            left_button.interactable = false;
            right_button.interactable = true;
        }
        else if (index == gameobjects.Count - 1)
        {
            left_button.interactable = true;
            right_button.interactable = false;
        }
        else
        {
            left_button.interactable = true;
            right_button.interactable = true;
        }
    }
}
