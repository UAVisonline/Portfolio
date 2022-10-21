using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class ChapterButton : MonoBehaviour
{
    [SerializeField] [ReadOnly] private int chapter_index;
    [SerializeField] private TextMeshProUGUI button_title;

    public void set_information(int index)
    {
        chapter_index = index;
        button_title.text = LoadManager.loadmanager.ret_name_of_chapter(chapter_index);
    }

    public void function()
    {
        LoadManager.loadmanager.set_current_chapter(button_title.text);
    }
}
