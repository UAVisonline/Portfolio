using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSet : MonoBehaviour // 어느 Scene에서든 정보를 초기화하고 Main화면으로 이동할 수 있음
{
    [SerializeField] private List<GameObject> off_gameobjects;
    [SerializeField] private List<GameObject> on_gameobjects;

    [SerializeField] private InterfaceManager standard_manager;
    [SerializeField] private InterfaceManager directional_manager;

    [SerializeField] private string animation_name;

    private WaitForSeconds wait_time = new WaitForSeconds(0.5f);

    public void Game_set() // Game 정보 초기화
    {
        GameManager.gamemanager.set_visualize(false);
        GameManager.gamemanager.information_animation_start(animation_name);
        GameManager.gamemanager.set_pause(false);

        DramaticManager.dramaticmanager.set_scene_time(1.2f);
        DramaticManager.dramaticmanager.set_bool(false);

        StartCoroutine("Scene_move");
    }

    IEnumerator Scene_move() // Main화면에서 실제 Game으로 이동
    {
        yield return wait_time;

        for (int i = 0; i < off_gameobjects.Count; i++)
        {
            if (off_gameobjects[i].activeSelf == true)
            {
                off_gameobjects[i].SetActive(false);
            }
        }

        if(GameManager.gamemanager.get_mode()==Interface_mode.standard)
        {
            StandardInterfaceManager.standardmanager.set_active_false();
        }
        else if(GameManager.gamemanager.get_mode() == Interface_mode.direction)
        {
            DirectionalInterfaceManager.directionalInterfaceManager.set_active_false();
        }

        for (int i = 0; i < on_gameobjects.Count; i++)
        {
            if (on_gameobjects[i].activeSelf == false)
            {
                on_gameobjects[i].SetActive(true);
            }
        }
    }
}
