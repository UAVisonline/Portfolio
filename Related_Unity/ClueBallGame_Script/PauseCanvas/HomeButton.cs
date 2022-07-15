using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HomeButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject pause_canvas;

    public void OnPointerClick(PointerEventData eventData) // 해당 오브젝트를 클릭햇는데
    {
        if(DramaticManager.dramaticmanager.get_bool()==false) // 화면전환 중이 아닌경우
        {
            GameManager.gamemanager.set_pause(true); // pause변수 설정
            GameManager.gamemanager.click_play(); // 
            pause_canvas.SetActive(true); // 정지 화면 object True
        }
    }
}
