using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ContentButton : MonoBehaviour
{
    [SerializeField] [ReadOnly] private int content_index;
    [SerializeField] private TextMeshProUGUI button_title;

    public void set_information(int index)
    {
        content_index = index;
        button_title.text = LoadManager.loadmanager.ret_name_of_content(content_index);
    }

    public void function()
    {
        LoadManager.loadmanager.set_current_content(button_title.text, content_index);
    }
}
