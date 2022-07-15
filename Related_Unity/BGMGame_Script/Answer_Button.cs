using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Answer_Button : MonoBehaviour
{
    Text button_text;
    // Start is called before the first frame update
    void Start()
    {
        button_text = GetComponentInChildren<Text>();
    }

    public void Hint_Button(int num) // 번호에 일치하는 힌트 제공
    {
        Problem_Base.problem.give_a_hint(num);
    }

    // Update is called once per frame
    /*public void Answer_Button_Click()
    {
        if (input == null)
        {
            input = FindObjectOfType<InputField>();
        }
        if (!Problem_Base.problem.return_correct())
        {
            string str = input.text;
            if (Problem_Base.problem.Answer_Problem(str))
            {
                Problem_Base.problem.jacket_answer();
                input.readOnly = true;
                button_text.text = "NEXT!!!";
            }
            else if(!Problem_Base.problem.Answer_Problem(str))
            {
                input.text = "Wrong Answer";
            }
        }
        else if(Problem_Base.problem.return_correct())
        {
            input.readOnly = false;
            Problem_Base.problem.set_correct(false);
            StartCoroutine("next_problem");
        }
        
    }*/

    public void answer_button(int i) // 해당 번호에 맞는 보기 입력
    {
        Problem_Base.problem.answer_function(i);
    }

    public void next_button_click() // 다음 문제로 이동
    {
        if(Problem_Base.problem.ret_solved())
        {
            Problem_Base.problem.set_solved(false);
            StartCoroutine("next_problem");
        }
    }

    public void back_button_click() // 메인메뉴로 이동
    {
        StartCoroutine("go_to_main");
    }


    public void Pass_Button() // 풀지 않은 문제 패스하기
    {
        if (!Problem_Base.problem.ret_solved()) // 문제를 풀지 않았을때만 가능
        {
            Problem_Base.problem.pass_function();
        }
    }

    IEnumerator next_problem()
    {
        Directer_machine.directer.set_panel_slide(true);
        yield return new WaitForSeconds(1.4f);

        Problem_Base.problem.Music_Stop();
        Problem_Base.problem.jacket_black();
        Problem_Base.problem.information_clear();
        Problem_Base.problem.correct_wrong_clear();
        ScoreManager.score_manager.present_up();
        // 음악 정지 후 이전 정보 초기화

        if (Problem_Base.problem.ret_arcade_mode()) // 아케이드 모드에서
        {
            ScoreManager.score_manager.arcade_text();

            if(ScoreManager.score_manager.last_question()) // 마지막 문제였으면
            {
                Problem_Base.problem.Problem_delete();
                ScoreManager.score_manager.arcade_result_func();
                SceneManager.LoadScene("Arcade_result"); // 결과창으로 이동
            }
            else // 마지막 문제가 아니면
            {
                Problem_Base.problem.Change_Problem(); // 다음 문제로 변경
                yield return new WaitForSeconds(0.7f);

                Directer_machine.directer.set_panel_slide(false);
                yield return new WaitForSeconds(0.3f);
                Problem_Base.problem.Music_Play();
            }
        }
        else if(Problem_Base.problem.ret_training_mode()) // 트레이닝 모드에서
        {
            Problem_Base.problem.Change_Problem();
            yield return new WaitForSeconds(0.7f);

            Directer_machine.directer.set_panel_slide(false);
            yield return new WaitForSeconds(0.3f);
            Problem_Base.problem.Music_Play();
        }
        

    }

    IEnumerator go_to_main()
    {
        Directer_machine.directer.set_panel_slide(true);
        yield return new WaitForSeconds(1.4f);
        ScoreManager.score_manager.set_initial();
        Problem_Base.problem.Back_Game();
        SceneManager.LoadScene("Main");
    }
}
