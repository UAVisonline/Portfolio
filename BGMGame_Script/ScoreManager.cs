using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _score_manager;

    [SerializeField] private Text tmp_text;

    [SerializeField] private int total_question;
    [SerializeField] private int present_question;
    [SerializeField] private int correct;

    public static ScoreManager score_manager // singleton 설정
    {
        get
        {
            if (_score_manager == null)
            {
                _score_manager = FindObjectOfType<ScoreManager>();
                if (_score_manager == null)
                {
                    Debug.LogError("Can't Load Problem");
                }
            }
            return _score_manager;
        }
    }

    private void Awake() // Singleton 할당
    {
        if (_score_manager == null)
        {
            DontDestroyOnLoad(this.gameObject);
            _score_manager = FindObjectOfType<ScoreManager>();
            if (_score_manager == null)
            {
                Debug.LogError("Can't Load Problem");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void arcade_text()
    {
        string tmp = (present_question + 1).ToString() + "/" + total_question.ToString();

        if(tmp_text==null) // 문제 번호 오브젝트 할당
        {
            tmp_text = GameObject.Find("Problem_Number").GetComponent<Text>();
        }
        if(tmp_text!=null) // 문제 번호 시각화
        {
            tmp_text.text = tmp;
        }
    }

    public string ret_arcade_score() // 몇 문제 맞혔는지 반환
    {
        string score = "전체 " + total_question.ToString() + "개의 문제 중" +  correct.ToString() + "개의 문제를 맞추었습니다.";
        return score;
    }

    public void score_up() // 문제를 맞히면
    {
        correct++;
    }

    public void present_up() // 현재 문제가 넘어가면
    {
        present_question++;
    }

    public bool last_question() // 마지막 문제 상태 반환
    {
        if (present_question >= total_question)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void set_initial() // 처음 상태로 설정 (맞춘문제 0, 현재 문제는 처음 푸는 문제)
    {
        present_question = 0;
        correct = 0;
    }

    public void arcade_result_func()
    {
        tmp_text.text = "";
        tmp_text = null;
        StartCoroutine("Arcade_Result");
    }

    IEnumerator Arcade_Result() // 아케이드 모드 결과 출력
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "Arcade_result") break;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Text result = GameObject.Find("result").GetComponent<Text>();
        result.text = ret_arcade_score();

        yield return new WaitForSeconds(1.0f);
        Directer_machine.directer.set_panel_slide(false);
    }
}
