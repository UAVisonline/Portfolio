using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentInsert : MonoBehaviour
{
    [SerializeField] private Active_Button active_btn;

    public void function()
    {
        if(LoadManager.loadmanager.ret_content_list()<250)
        {
            LoadManager.loadmanager.clear_current_content_detail();
            LoadManager.loadmanager.set_current_content_index(LoadManager.loadmanager.ret_content_list());
            active_btn.function();
        }
    }
}
