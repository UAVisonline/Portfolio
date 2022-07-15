using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execution : MonoBehaviour
{
    [SerializeField] private int choices;

    [SerializeField] private List<ButtonCommandObject> commands;
    [SerializeField] private Dialogue dialogue;

    public virtual void OnEnable()
    {
        if(GameManager.gamemanager.get_mode()==Interface_mode.standard) // Standard Mode에서 선택지 위치 선정을 위한 함수 실행
        {
            StandardInterfaceManager.standardmanager.set_number_of_choice(choices);
        }
        
        for(int i =0;i<commands.Count;i++) // 각 버튼에 대하여 Command 할당
        {
            commands[i].set_button();
        }

        if(dialogue!=null)
        {
            GameManager.gamemanager.get_supporter().set_dialogue(dialogue);
        }
    }
}
