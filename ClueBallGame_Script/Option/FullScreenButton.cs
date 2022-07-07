using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenButton : MonoBehaviour
{
    [SerializeField] private Image check_image;

    [SerializeField] private bool isfull;

    private void Awake()
    {
        isfull = Screen.fullScreen; // 현재 game에 FullScreen 참조
        check_image.gameObject.SetActive(isfull); // 이를 체크박스로 반영
    }

    public void Click_event() // 체크박스 클릭 시
    {
        isfull = !isfull; // Fullscreen status를 반전
        GameManager.gamemanager.set_isfull(isfull); // 이를 game에 반영
        check_image.gameObject.SetActive(isfull);
    }
}
