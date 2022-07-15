using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_Reasoning : MonoBehaviour // 최종 추리
{
    [SerializeField] private Dialogue loading;

    [SerializeField] private float time;

    [SerializeField] private GameObject off_gameobject;
    [SerializeField] private GameObject correct_gameobject;
    [SerializeField] private GameObject un_correct_gameobject;

    private WaitForSeconds wait_time = null;
    private int result;


    private void OnEnable()
    {
        if (wait_time == null)
        {
            wait_time = new WaitForSeconds(time);
        }
        result = GameManager.gamemanager.Reasoning_result();

        PlayerPrefs.SetInt("Try", PlayerPrefs.GetInt("Try") + 1);
        if(GameManager.gamemanager.get_mode()==Interface_mode.standard)
        {
            PlayerPrefs.SetInt("Standard", PlayerPrefs.GetInt("Standard") + 1);
            PlayerPrefs.SetString("Interface", "standard");
        }
        else if(GameManager.gamemanager.get_mode()==Interface_mode.direction)
        {
            PlayerPrefs.SetInt("Directional", PlayerPrefs.GetInt("Directional") + 1);
            PlayerPrefs.SetString("Interface", "direction");
        }

        if(result==3)
        {
            DramaticManager.dramaticmanager.set_correct(true);

            PlayerPrefs.SetInt("Correct", PlayerPrefs.GetInt("Correct") + 1);
            if (GameManager.gamemanager.get_mode() == Interface_mode.standard)
            {
                PlayerPrefs.SetInt("Correct_standard", PlayerPrefs.GetInt("Correct_standard") + 1);
            }
            else if (GameManager.gamemanager.get_mode() == Interface_mode.direction)
            {
                PlayerPrefs.SetInt("Correct_directional", PlayerPrefs.GetInt("Correct_directional") + 1);
            }
        }
        else
        {
            DramaticManager.dramaticmanager.set_correct(false);
        }
        // PlayerPref 정보 갱신

        DramaticManager.dramaticmanager.animation_start("Answering"); // 정답 제출 시 정보 공개 animation 실행
        DramaticManager.dramaticmanager.set_bool(true);

        GameManager.gamemanager.get_supporter().set_dialogue(loading);
        GameManager.gamemanager.background_music_instant_stop();

        StartCoroutine("Result_function");
    }

    IEnumerator Result_function() // 실제 정답 공개
    {
        GameManager.gamemanager.information_animation_start("Off");

        yield return wait_time;

        if(result!=3) // 정답이 아니면
        {
            un_correct_gameobject.SetActive(true);
            off_gameobject.SetActive(false);
        }
        else // 정답이면
        {
            correct_gameobject.SetActive(true);
            off_gameobject.SetActive(false);
        }
    }
}
