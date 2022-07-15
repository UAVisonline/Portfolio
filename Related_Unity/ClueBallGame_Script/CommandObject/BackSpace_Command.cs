using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSpace_Command : ButtonCommandObject // 뒤로가기 버튼
{
    [SerializeField] private Dialogue dialogue; // 이 때 Supporter가 실행할 대사 (없는 경우 on gameobject에 할당된 대사가 실행될 것임)

    [SerializeField] private List<GameObject> on_gameobjects;
    [SerializeField] private List<GameObject> off_gameobjects;

    public override void execute()
    {
        if(dialogue!=null)
        {
            GameManager.gamemanager.get_supporter().set_dialogue(dialogue);
        }

        for (int i = 0; i < off_gameobjects.Count; i++)
        {
            off_gameobjects[i].SetActive(false);
        }

        for (int i = 0; i < on_gameobjects.Count; i++)
        {
            on_gameobjects[i].SetActive(true);
        }
    }
}
