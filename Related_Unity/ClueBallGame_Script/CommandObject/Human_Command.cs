using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_Command : ButtonCommandObject // 용의자 버튼
{
    [SerializeField] private suspect suspect;

    [SerializeField] private List<GameObject> on_gameobjects;
    [SerializeField] private List<GameObject> off_gameobjects;

    public override void execute()
    {
        GameManager.gamemanager.set_suspect(suspect);

        for (int i = 0; i < off_gameobjects.Count; i++)
        {
            off_gameobjects[i].SetActive(false);
        }

        for (int i = 0; i < on_gameobjects.Count; i++)
        {
            on_gameobjects[i].SetActive(true);
        }
    }
}
