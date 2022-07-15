using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeftRight : MonoBehaviour
{
    [SerializeField] private Voca_Mother mother;

    [SerializeField] private Button left_button;
    [SerializeField] private Button right_button;

    private void OnEnable()
    {
        left_right_reload();
    }

    public void left_right_reload()
    {
        if ((VocaMaster.vocaMaster.get_index() + 1) * 5 >= VocaMaster.vocaMaster.get_count()) // 현재 Voca Page에 Index를 참조-> 이 값이 현재 단어 갯수보다 크면
        {
            right_button.interactable = false; // 오른쪽 버튼 비활성화 (다음 Page로 이동)
        }
        else
        {
            right_button.interactable = true; // 오른쪽 버튼 활성화
        }

        if (VocaMaster.vocaMaster.get_index() == 0) // 현재 Voca Page Index가 0이면 -> 이전 Page가 없는 경우
        {
            left_button.interactable = false; // 왼쪽 버튼 비활성화
        }
        else
        {
            left_button.interactable = true; // 왼쪽 버튼 활성화
        }
    }

    public void left_button_click() // 왼쪽 버튼 Click
    {
        VocaMaster.vocaMaster.set_index(VocaMaster.vocaMaster.get_index()-1); // Voca Master의 page index - 1
        mother.reload_page(); // Voca mother에서 Page를 Reload (해당 Page 내 단어를 실제로 화면 상 보이게 해주는 역할 )
        left_right_reload();
    }

    public void right_button_click() // 오른쪽 버튼 Click
    {
        VocaMaster.vocaMaster.set_index(VocaMaster.vocaMaster.get_index() + 1); // Voca Master의 page index + 1
        mother.reload_page(); // Voca mother에서 Page를 Reload (해당 Page 내 단어를 실제로 화면 상 보이게 해주는 역할 )
        left_right_reload();
    }
}
