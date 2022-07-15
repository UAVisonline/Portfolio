using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Command : ButtonCommandObject // 범행장소 버튼
{
    [SerializeField] private crime_scene place;

    [SerializeField] private List<GameObject> on_gameobjects;
    [SerializeField] private List<GameObject> off_gameobjects;

    public override void execute()
    {
        GameManager.gamemanager.set_place(place);

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
