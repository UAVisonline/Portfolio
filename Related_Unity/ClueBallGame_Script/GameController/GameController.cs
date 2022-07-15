using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour // Main Game 처음 시작
{
    //[SerializeField] private List<GameObject> scenes;
    [SerializeField] private GameObject first_scene;

    private void OnEnable()
    {
        first_scene.SetActive(true);
    }

    /*
    private void OnDisable()
    {
        for(int i =0;i<scenes.Count;i++)
        {
            if(scenes[i] !=null)
            {
                scenes[i].SetActive(false);
            }
        }
    }
    */
}
