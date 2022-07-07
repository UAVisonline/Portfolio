using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reasoning : MonoBehaviour // 일반 추리
{
    [SerializeField] private Dialogue loading;
    [SerializeField] private Dialogue zero_match;
    [SerializeField] private Dialogue one_match;
    [SerializeField] private Dialogue two_more_match;

    [SerializeField] private float time;

    [SerializeField] private GameObject off_gameobject;
    [SerializeField] private GameObject on_gameobject;

    private int result;
    private WaitForSeconds wait_time = null;


    private void OnEnable()
    {
        if(wait_time==null)
        {
            wait_time = new WaitForSeconds(time);
        }
        result = GameManager.gamemanager.Reasoning_result();

        GameManager.gamemanager.set_life(GameManager.gamemanager.get_life() - 1); // 추리 기회 감소
        GameManager.gamemanager.get_supporter().set_dialogue(loading); // Loading 대사 출력

        StartCoroutine("Result_function");
    }

    IEnumerator Result_function()
    {
        yield return wait_time;
        if(result==0)
        {
            GameManager.gamemanager.get_supporter().set_dialogue(zero_match);
        }
        else if(result==1)
        {
            GameManager.gamemanager.get_supporter().set_dialogue(one_match);
        }
        else if(result>=2)
        {
            GameManager.gamemanager.get_supporter().set_dialogue(two_more_match);
        }
        // 맞춘 선택지 갯수에 따른 대사 설정

        Life_text text = FindObjectOfType<Life_text>();
        text.set_life_text();

        off_gameobject.SetActive(false);
        on_gameobject.SetActive(true);
    }
}
