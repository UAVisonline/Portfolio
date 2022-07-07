using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveButton : CommandObject
{
    [SerializeField] private List<GameObject> gameObjects;// 활성화할지, 안할지 설정할 GameObject
    [SerializeField] private bool status;

    public override void execute()
    {
        for(int i =0;i<gameObjects.Count;i++) 
        {
            gameObjects[i].SetActive(status); //배열 내 게임오브젝트를 전부 status 상태로 활성화
        }
        GameManager.gamemanager.click_play();
    }
}
