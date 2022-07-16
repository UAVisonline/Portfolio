using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button_Key : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Start_New_game()
    {
        PlayerPrefs.DeleteAll();
        Player_Manager.player_manager.set_position(-9.0f, -6.4f, false);
        Player_Manager.player_manager.Refill();
        Canvas canvas = GameObject.Find("Title_Screen").GetComponent<Canvas>();
        canvas.sortingOrder = 5;
        PlayerPrefs.SetString("Save", "true");
        PlayerPrefs.SetString("Scene", "barrack_1");
        PlayerPrefs.SetFloat("x_pos", -9.0f);
        PlayerPrefs.SetFloat("y_pos", -6.4f);
        Dramatic_UI.dramatic_manager.Warp_scene("Intro");
    }

    public void Load_Game()
    {
        Player_Manager.player_manager.set_position(PlayerPrefs.GetFloat("x_pos"), PlayerPrefs.GetFloat("y_pos"), false);
        Canvas canvas = GameObject.Find("Title_Screen").GetComponent<Canvas>();
        if(canvas!=null)
        {
            canvas.sortingOrder = 5;
        }
        Dramatic_UI.dramatic_manager.Warp_scene(PlayerPrefs.GetString("Scene"));
        Player_Manager.player_manager.Refill();
    }

    public void Exit_game()
    {
        PlayerPrefs.Save();
        Dramatic_UI.dramatic_manager.fade_in(0.5f);
        Canvas canvas = GameObject.Find("Title_Screen").GetComponent<Canvas>();
        canvas.sortingOrder = 5;
        StartCoroutine(Quit());
    }

    public void Pause_button()
    {
        Dramatic_UI.dramatic_manager.Set_Pause();
    }

    public void Go_To_Mainmenu()
    {
        //Debug.Log("asdasd");
        Time.timeScale = 1.0f;
        Dramatic_UI.dramatic_manager.Set_Pause();
        Dramatic_UI.dramatic_manager.Warp_scene("Main");
    }

    public void Game_Over_Mainmenu()
    {
        Canvas canvas = GameObject.Find("Title_Screen").GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 5;
        }
        Dramatic_UI.dramatic_manager.Warp_scene("Main");
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(1.0f);
        Application.Quit();
    }

    IEnumerator Main_Menu()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
