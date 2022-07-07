using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerField : MonoBehaviour
{
    [SerializeField] private InputField answer_input;
    [SerializeField] private Text answer;

    [SerializeField] private TestMaster testMaster;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            submit();
        }
    }

    public void submit() // 실제 단어 입력
    {
        testMaster.solving_function(answer.text); // InputField에 입력한 단어를 보냄

        answer_input.Select();
        answer_input.text = "";
        answer.text = "";
        // InputField 내용 초기화
    }
}
