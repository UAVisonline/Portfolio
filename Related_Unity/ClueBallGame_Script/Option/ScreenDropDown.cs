using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDropDown : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("Screen_index")==false)
        {
            PlayerPrefs.SetInt("Screen_index", 1);
        }
        else
        {
            dropdown.value = PlayerPrefs.GetInt("Screen_index");
        }
    }

    public void screen_change()
    {
        int index = dropdown.value;
        switch(index)
        {
            case 0:
                GameManager.gamemanager.set_resolution_size(1920, 1080);
                break;
            case 1:
                GameManager.gamemanager.set_resolution_size(1600, 900);
                break;
            case 2:
                GameManager.gamemanager.set_resolution_size(1280, 720);
                break;
            case 3:
                GameManager.gamemanager.set_resolution_size(960, 540);
                break;
            case 4:
                GameManager.gamemanager.set_resolution_size(640, 360);
                break;
        }
        PlayerPrefs.SetInt("Screen_index", index);
    }
}
