using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro_Event : MonoBehaviour
{
    [SerializeField] private Animator aniamtor;
    public Dialogue[] dialogues; // 실행할 Dialogue

    // Start is called before the first frame update
    void Start()
    {
        aniamtor = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        aniamtor.SetBool("NEXT", DialogueSystem.dialogue_System.Dialogue_status()); // Dialogue 시스템이 가동되었는가 아닌가를 Animator가 읽음
    }

    void Animation_Dialogue(int i) //Animation Event 전용
    {
        if(!DialogueSystem.dialogue_System.Dialogue_status())
        {
            DialogueSystem.dialogue_System.Dialogue_input(dialogues[i]);
        }
    }

    void Stage_Start()
    {
        Dramatic_UI.dramatic_manager.Warp_scene("barrack_1",1.2f); // 스테이지 이동
    }
}
