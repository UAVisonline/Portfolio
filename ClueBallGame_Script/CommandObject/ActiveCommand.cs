using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveCommand : ButtonCommandObject // 버튼에 ActiveTrue, False가 바뀔 수 있음
{
    [SerializeField] private bool check_life;

    [SerializeField] private List<GameObject> on_gameobjects;
    [SerializeField] private List<GameObject> off_gameobjects;

    private void OnEnable()
    {
        if (check_life)
        {
            if (GameManager.gamemanager.get_life() <= 0) // 목숨이 0 이하인 경우
            {
                if(GameManager.gamemanager.get_mode()==Interface_mode.standard)
                {
                    StandardInterfaceManager.standardmanager.set_interactable_false_first_button(); // Standard Interface에서는 추리 불가능
                }
                else if(GameManager.gamemanager.get_mode() == Interface_mode.direction)
                {
                    DirectionalInterfaceManager.directionalInterfaceManager.set_interactable_false_eighth_button(); // Directional Interface에서는 추리 불가능
                    //StandardInterfaceManager.standardmanager.set_interactable_false_first_button();
                }
                
                return;
            }
        }
    }

    public override void execute()
    {
        for(int i =0;i< off_gameobjects.Count;i++)
        {
            off_gameobjects[i].SetActive(false); // 해당 Object는 꺼버림
        }

        for (int i = 0; i < on_gameobjects.Count; i++)
        {
            on_gameobjects[i].SetActive(true); // 해당 Object는 켜버림 (이를 통한 기능 이동 구현)
        }
    }
}
