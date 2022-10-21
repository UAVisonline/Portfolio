using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class StoryButton : MonoBehaviour
{
    [SerializeField] private int story_index;
    [SerializeField] private TextMeshProUGUI button_title;

    public void set_information(int index)
    {
        story_index = index;
        button_title.text = LoadManager.loadmanager.ret_index_of_title(index);
    }

    public void function()
    {
        LoadManager.loadmanager.set_current_information(story_index);
    }
}
