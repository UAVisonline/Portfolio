using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InformationFrame : MonoBehaviour // 시험 내 정보 창 (맞추어야할 단어 뜻을 표시하는 부분)
{
    [SerializeField] private Image frame_image;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable() // TestMaster 내 Event Handler에 Event 추가
    {
        TestMaster.reload_problem_event += information_set; 
        TestMaster.correct_problem_event += correct;
        TestMaster.wrong_problem_event += incorrect;
        //Debug.Log("On");
    }

    private void OnDisable() // TestMaster 내 Event Handler에 Event 삭제 (OnEnable할때마다 Event가 추가되면 오류가 발생하므로 다시 삭제해주어야 함)
    {
        TestMaster.reload_problem_event -= information_set;
        TestMaster.correct_problem_event -= correct;
        TestMaster.wrong_problem_event -= incorrect;
        //Debug.Log("Off");
    }

    private void information_set(string value) // 단어 뜻을 UI로 시각화 (문제가 바뀔때마다 자동으로 실행해야 함 -> Event Handler 이용)
    {
        text.text = value;
        if(value=="")
        {
            frame_image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        else
        {
            frame_image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    private void correct()
    {
        // Debug.Log("Correct");
    }

    private void incorrect()
    {
        // Debug.Log("InCorrect");
    }
    // Debug용 함수 2개
}
