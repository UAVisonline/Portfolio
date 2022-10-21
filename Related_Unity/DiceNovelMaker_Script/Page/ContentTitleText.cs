using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContentTitleText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chapter_title;

    private void OnEnable()
    {
        chapter_title.text = LoadManager.loadmanager.ret_current_chapter();
    }
}
