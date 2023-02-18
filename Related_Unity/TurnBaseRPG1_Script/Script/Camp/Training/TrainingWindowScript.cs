using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainingWindowScript : MonoBehaviour
{
    [SerializeField] private CampCanvasScript campscript;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI sentence;
    [SerializeField] private TextMeshProUGUI value_sentence;

    public void STR_window(int value)
    {
        title.text = "힘 훈련";
        sentence.text = "열심히 쇠질 하는 것으로 당신의 힘을 아래와 같이 올렸습니다.";
        value_sentence.text = value.ToString();
    }

    public void DEX_window(int value)
    {
        title.text = "민첩 훈련";
        sentence.text = "은밀히 움직이는 것으로 당신의 민첩성을 아래와 같이 올렸습니다.";
        value_sentence.text = value.ToString();
    }

    public void INT_window(int value)
    {
        title.text = "지능 훈련";
        sentence.text = "고문서를 읽는 것으로 당신의 지능을 아래와 같이 올렸습니다.";
        value_sentence.text = value.ToString();
    }

    public void ATK_window(int value)
    {
        if(value>0)
        {
            title.text = "공격 훈련";
            sentence.text = "대련을 통해 당신의 공격력을 아래와 같이 올렸습니다.";
            value_sentence.text = value.ToString();
        }
        else
        {
            title.text = "공격 훈련";
            sentence.text = "무리한 몸으로 대련을 진행해 이번에는 효과를 조금도 보지 못했습니다.";
            value_sentence.text = "";
        }
    }

    public void Rest_window(int value)
    {
        title.text = "휴식";
        sentence.text = "말 못할 취미생활을 통해 당신은 스트레스를 아래와 같이 내렸습니다.";
        value_sentence.text = value.ToString();
    }

    public void TrainOkayBtn() // 트레이닝 종료 (다음 턴으로)
    {
        this.gameObject.SetActive(false);
        campscript.visualize_current_chance_window();
    }
}
