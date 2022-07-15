using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceApply : CommandObject // Interface 설정 버튼
{
    private WaitForSeconds wait_time = new WaitForSeconds(0.5f);

    [SerializeField] private GameObject Interface_Canvas;
    [SerializeField] private GameObject RealGame_Canvas;
    [SerializeField] private Interface_mode mode;

    [SerializeField] private string interface_name;

    public override void execute()
    {
        GameManager.gamemanager.set_interface_mode(mode); // 게임매니저에게 해당 Interface를 실행하라고 명령
        GameManager.gamemanager.click_play();

        DramaticManager.dramaticmanager.set_scene_time(1.2f); 
        StartCoroutine("Scene_move");
        // 화면 이동 후 게임 시작

        //Invoke("go_realgame", 1.0f); , Test용 함수
    }

    void OnEnable()
    {
        if (PlayerPrefs.HasKey("Interface") == true) 
        {
            if (PlayerPrefs.GetString("Interface") == interface_name) // PlayerPref 값이 interface name이면
            {
                //Debug.Log("false");
                this.GetComponent<Button>().interactable = false; // 이 interface는 선택 불가
            }
            else
            {
                //Debug.Log("true");
                this.GetComponent<Button>().interactable = true; // 선택 가능
            }
        }
    }

    IEnumerator Scene_move() // Interface 설정 후 실제 Game으로 이동
    {
        yield return wait_time;

        GameManager.gamemanager.background_music_play();
        RealGame_Canvas.SetActive(true);
        Interface_Canvas.SetActive(false);
    }

    public void go_realgame() // Test용 함수
    {
        RealGame_Canvas.SetActive(true);
    }
}
