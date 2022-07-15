using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_Menu_Button : MonoBehaviour
{
    [SerializeField] private bool check = false;

    public void Practice_Click()
    {
        if(!check)
        {
            Problem_Base.problem.set_traing_mode(true);
            StartCoroutine("Go_To_Game");
            check = true;
        }
    }

    public void Arcade_Click()
    {
        if (!check)
        {
            Problem_Base.problem.set_arcade_mode(true);
            StartCoroutine("Go_To_Game");
            check = true;
        }
    }

    public void Exit_Click()
    {
        if(!check)
        {
            StartCoroutine("Go_To_Exit");
            check = true;
        }
    }

    IEnumerator Go_To_Game()
    {
        Directer_machine.directer.set_panel_slide(true);

        yield return new WaitForSeconds(1.4f);
        Problem_Base.problem.Start_Game();
        SceneManager.LoadScene("Game");
    }

    IEnumerator Go_To_Exit()
    {
        Directer_machine.directer.set_panel_slide(true);
        yield return new WaitForSeconds(1.4f);
        check = false;
        Application.Quit();
    }
}
