using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Problem_file : MonoBehaviour
{
    [SerializeField] private string answer_title;
    [SerializeField] private string[] bogis;
    [SerializeField] private string[] Hints = new string[2];
    [SerializeField] private string game_title;
    [SerializeField] private string bgm_title;
    [SerializeField] private AudioClip bgm;
    [SerializeField] private Sprite jacket;

    public void Problem_set() // 음악 Singleton에 대해 정보를 설정
    {
        Problem_Base.problem.set_hint(Hints);
        Problem_Base.problem.set_information(game_title, bgm_title);
        Problem_Base.problem.set_audiosource(bgm);
        Problem_Base.problem.set_jacket(jacket);
        Problem_Base.problem.set_answer_state(this);
    }
    
    public Sprite ret_Sprite()
    {
        return jacket;
    }

    public string ret_answer()
    {
        return answer_title;
    }

    public int ret_bogi_size()
    {
        return bogis.Length;
    }

    public string ret_bogi(int i)
    {
        return bogis[i];
    }
}
