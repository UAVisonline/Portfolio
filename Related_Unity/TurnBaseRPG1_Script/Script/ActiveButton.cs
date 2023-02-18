using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveButton : MonoBehaviour
{
    public List<GameObject> active_gameobj;
    public List<GameObject> deactive_gameobj;

    public void btn_function()
    {
        for(int i =0;i<active_gameobj.Count;i++)
        {
            active_gameobj[i].SetActive(true);
        }

        for(int i =0;i<deactive_gameobj.Count;i++)
        {
            deactive_gameobj[i].SetActive(false);
        }

        Util_Manager.utilManager.button_click_sound_play();
    }
}
