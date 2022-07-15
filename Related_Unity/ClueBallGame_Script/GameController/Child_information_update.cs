using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Child_information_update : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Information_update parent_update;

    [SerializeField] private int index;
    [SerializeField] private information current_information;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (current_information)
        {
            case information.none:
                current_information = information.not_this;
                break;
            case information.not_this:
                current_information = information.wondering;
                break;
            case information.wondering:
                current_information = information.this_one;
                break;
            case information.this_one:
                current_information = information.none;
                break;
        } // 정보 유추 상태 변경 (사람의 정보 유추만 이를 사용)
        parent_update.set_information_human(index, current_information); // 그 후 이를 부모에게 알려줌
        GameManager.gamemanager.click_play();
    }

    public void init_current_information() //정보 미정으로 초기화
    {
        current_information = information.none;
    }
}
