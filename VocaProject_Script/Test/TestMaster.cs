using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public delegate void normal_event_delegate(); // Event Delegate

public delegate void text_event_delegate(string value); 

public class TestMaster : MonoBehaviour
{

    private static TestMaster _testmaster;

    public static TestMaster testmaster // Singleton 생성
    {
        get
        {
            if (_testmaster == null)
            {
                _testmaster = FindObjectOfType<TestMaster>();
            }
            return _testmaster;
        }
    }


    [SerializeField] private List<string> answer = new List<string>(); // 실제 맞춰야할 단어
    [SerializeField] private List<string> explanation = new List<string>(); // 위 단어의 뜻

    [SerializeField] private List<bool> answer_result = new List<bool>(); // 단어를 맞췄는지, 틀렸는지 여부
    private List<string> player_answer_history = new List<string>(); // 플레이어가 입력한 단어

    private int total_correct_problem; // 전체 맞춘 문제들
    private int current_problem_number; // 현재 문제 번호

    string player_answer;

    [SerializeField] private UnityEvent test_start; // 테스트 시작을 위해 필요한 함수 기능을 묶어 놓음

    [SerializeField] List<TextMeshProUGUI> history_text; // 테스트 결과 출력을 위한 Text List

    public static event normal_event_delegate correct_problem_event; // 문제 맞춤
    public static event normal_event_delegate wrong_problem_event; // 문제 틀림
    public static event normal_event_delegate correct_end_problem_event; // 마지막 문제 맞춤
    public static event normal_event_delegate wrong_end_problem_event; // 마지막 문제 틀림
    public static event normal_event_delegate perfect_end_problem_event; // 18문제 이상 맞춤
    public static event text_event_delegate reload_problem_event; // 문제가 바뀌는 경우 (다음 문제로)

    public void test_btn()
    {
        if (VocaMaster.vocaMaster.get_count() >= 20)
        {
            init_test();
        }
    }

    private void init_test()
    {
        VocaMaster.vocaMaster.uncheck_all(); // 단어 상세항목에 들어가서 현재 시험에서 참조하는 단어 여부를 해제
        answer.Clear();
        explanation.Clear();
        answer_result.Clear();
        player_answer_history.Clear();

        total_correct_problem = 0;
        current_problem_number = 0;
        int tmp = 20;
        int remain = 0;
        // 시험에 앞서서 정보 초기화

        List<int> ret = VocaMaster.vocaMaster.get_level_index_list(7.0f, 8.9f); // 가장 어려운 문제 4개
        ret = shuffle<int>(ret);
        remain = 4;
        for(int i =0;i<ret.Count;i++)
        {
            string hd = VocaMaster.vocaMaster.get_list_content(ret[i]);
            if(VocaMaster.vocaMaster.get_dictionary_bd_check(hd)==false)
            {
                string bd = VocaMaster.vocaMaster.get_dicitionary_body(hd);
                VocaMaster.vocaMaster.set_dictionary_check(hd);
                answer.Add(hd);
                explanation.Add(bd);
                remain -= 1;
                tmp -= 1;
            }

            if(remain <= 0 || tmp <= 0)
            {
                break;
            }
        }

        ret = VocaMaster.vocaMaster.get_level_index_list(6.0f, 6.9f); // 그 다음 어려운 문제 4개
        ret = shuffle<int>(ret);
        remain = 4;
        for (int i = 0; i < ret.Count; i++)
        {
            string hd = VocaMaster.vocaMaster.get_list_content(ret[i]);
            if (VocaMaster.vocaMaster.get_dictionary_bd_check(hd) == false)
            {
                string bd = VocaMaster.vocaMaster.get_dicitionary_body(hd);
                VocaMaster.vocaMaster.set_dictionary_check(hd);
                answer.Add(hd);
                explanation.Add(bd);
                remain -= 1;
                tmp -= 1;
            }

            if (remain <= 0 || tmp <=0)
            {
                break;
            }
        }

        ret = VocaMaster.vocaMaster.get_level_index_list(4.0f, 5.9f); // 약간 어려운 문제 4개
        ret = shuffle<int>(ret);
        remain = 4;
        for (int i = 0; i < ret.Count; i++)
        {
            string hd = VocaMaster.vocaMaster.get_list_content(ret[i]);
            if (VocaMaster.vocaMaster.get_dictionary_bd_check(hd) == false)
            {
                string bd = VocaMaster.vocaMaster.get_dicitionary_body(hd);
                VocaMaster.vocaMaster.set_dictionary_check(hd);
                answer.Add(hd);
                explanation.Add(bd);
                remain -= 1;
                tmp -= 1;
            }

            if (remain <= 0 || tmp <= 0)
            {
                break;
            }
        }

        ret = VocaMaster.vocaMaster.get_level_index_list(3.0f, 3.9f); // 보통 난이도 문제 4개
        ret = shuffle<int>(ret);
        remain = 4;
        for (int i = 0; i < ret.Count; i++)
        {
            string hd = VocaMaster.vocaMaster.get_list_content(ret[i]);
            if (VocaMaster.vocaMaster.get_dictionary_bd_check(hd) == false)
            {
                string bd = VocaMaster.vocaMaster.get_dicitionary_body(hd);
                VocaMaster.vocaMaster.set_dictionary_check(hd);
                answer.Add(hd);
                explanation.Add(bd);
                remain -= 1;
                tmp -= 1;
            }

            if (remain <= 0 || tmp <= 0)
            {
                break;
            }
        }

        ret = VocaMaster.vocaMaster.get_level_index_list(1.0f, 2.9f); // 쉬운 난이도 문제 4개
        ret = shuffle<int>(ret);
        remain = 4;
        for (int i = 0; i < ret.Count; i++)
        {
            string hd = VocaMaster.vocaMaster.get_list_content(ret[i]);
            if (VocaMaster.vocaMaster.get_dictionary_bd_check(hd) == false)
            {
                string bd = VocaMaster.vocaMaster.get_dicitionary_body(hd);
                VocaMaster.vocaMaster.set_dictionary_check(hd);
                answer.Add(hd);
                explanation.Add(bd);
                remain -= 1;
                tmp -= 1;
            }

            if (remain <= 0 || tmp <= 0)
            {
                break;
            }
        }

        if(tmp>0) // 아직 할당되지 않은 문제에 대하여
        {
            ret = VocaMaster.vocaMaster.get_level_index_list(4.0f, 8.9f); // 어느정도 어려운 문제들에 대해서 해당 값을 할당 
            ret = shuffle<int>(ret);
            remain = tmp;
            for (int i = 0; i < ret.Count; i++)
            {
                string hd = VocaMaster.vocaMaster.get_list_content(ret[i]);
                if (VocaMaster.vocaMaster.get_dictionary_bd_check(hd) == false)
                {
                    string bd = VocaMaster.vocaMaster.get_dicitionary_body(hd);
                    VocaMaster.vocaMaster.set_dictionary_check(hd);
                    answer.Add(hd);
                    explanation.Add(bd);
                    remain -= 1;
                    tmp -= 1;
                }

                if (remain <= 0 || tmp <= 0)
                {
                    break;
                }
            }
        }

        if (tmp > 0) // 그래도 아직 할당되지 않은 문제에 대해서는 
        {
            ret = VocaMaster.vocaMaster.get_level_index_list(1.0f, 3.9f); // 그보다 난이도가 낮은 문제에 대해서 해당 값을 할당
            ret = shuffle<int>(ret);
            remain = tmp;
            for (int i = 0; i < ret.Count; i++)
            {
                string hd = VocaMaster.vocaMaster.get_list_content(ret[i]); // 단어를 가지고 옴
                if (VocaMaster.vocaMaster.get_dictionary_bd_check(hd) == false)
                {
                    string bd = VocaMaster.vocaMaster.get_dicitionary_body(hd); // 그 뜻도 가지고 옴
                    VocaMaster.vocaMaster.set_dictionary_check(hd);
                    answer.Add(hd);
                    explanation.Add(bd);
                    remain -= 1;
                    tmp -= 1;
                    // 이를 시험에 추가
                }

                if (remain <= 0 || tmp <= 0)
                {
                    break;
                }
            }
        }

        test_start.Invoke(); // (문제 준비가 전부 끝남 -> 테스트 실행)
    }

    private bool check_answer() // 입력한 단어가 실제 정답인 경우인지, 아닌지 반환
    {
        if(answer[current_problem_number].ToLower()==player_answer) // 모두 소문자로 변환해서 확인 (Player_answer는 해당 함수를 실행하는 부분에서 소문자화)
        {
            return true;
        }
        return false;
    }

    public void start_problem() // 문제 시작 (처음 단어의 뜻을 시각화)
    {
        reload_problem_event(explanation[0]);
    }

    public void animator_start() // 결과 시각화를 위한 준비
    {
        for(int i =0;i<history_text.Count;i++)
        {
            if(answer_result[i]==false)
            {
                history_text[i].color = Color.red; //해당 번호 문제를 틀렸을 경우
            }
            else
            {
                history_text[i].color = Color.blue; // 해당 번호 문제를 맞힌 경우
            }

            history_text[i].text = (i + 1).ToString() + " : " + player_answer_history[i] + " / " + answer[i] + "[" + explanation[i] + "]";
        }

        this.GetComponent<Animator>().SetBool("end", true); // 결과 시각화
    }

    public void animator_end() // 결과 시각화 종료
    {
        this.GetComponent<Animator>().SetBool("end", false);
    }

    public bool return_animator_status()
    {
        return this.GetComponent<Animator>().GetBool("end");
    }

    public void solving_function(string value) // 문제 채점 실제 함수
    {
        player_answer = value.ToLower(); // 
        bool result = check_answer(); // 정답인가 아닌가
        answer_result.Add(result); // 이를 결과에 등록
        player_answer_history.Add(player_answer); // 플레이어의 입력도 등록

        current_problem_number++; // 현재 문제 번호 증가
        if(current_problem_number==20) // 마지막 문제까지 전부 푼 경우
        {
            if(result==true) // 정답이다
            {
                total_correct_problem += 1;
                if(total_correct_problem >= 18)
                {
                    perfect_end_problem_event();
                }
                else
                {
                    correct_end_problem_event();
                }
            }
            else
            {
                wrong_end_problem_event();
            }
            reload_problem_event(""); // 문제 보기 창 시각화 ""로 초기화
            reflect_test();
        }
        else // 마지막 문제가 아닌 경우
        {
            if(result==true)
            {
                total_correct_problem += 1;
                correct_problem_event();

            }
            else
            {
                wrong_problem_event();
            }
            reload_problem_event(explanation[current_problem_number]); // 문제 보기 창을 바꾼 문제로 시각화
        }
    }

    public void reflect_test() // 테스트 결과를 Data에 대하여 반영 (단어 Level 값 변동 및 Coin 증가)
    {
        int coin_plus = 0;
        for(int i =0;i< answer_result.Count;i++)
        {
            if(answer_result[i]==true)
            {
                VocaMaster.vocaMaster.set_dictionary_level(answer[i], -1.0f); // 문제를 맞춘 경우 -> level - 1
                coin_plus += 50;
            }
            else
            {
                VocaMaster.vocaMaster.set_dictionary_level(answer[i], +1.0f); // 문제를 틀린 경우 -> level + 1
            }
        }

        if(coin_plus==1000) // 모든 문제를 전부 맞춘 경우 
        {
            coin_plus += 200; // 200원 보너스
        }

        VocaMaster.vocaMaster.Save();
        ShopManager.shopmanager.plus_coin(coin_plus);
    }

    private List<T> shuffle<T>(List<T> value)
    {
        T tmp;

        for(int i =0;i<value.Count;i++)
        {
            tmp = value[i];
            int rand = Random.Range(0, value.Count);
            value[i] = value[rand];
            value[rand] = tmp;
        }

        return value;
    }
}
