using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class Chapter_Story_Title : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;

    private void OnEnable()
    {
        title.text = LoadManager.loadmanager.ret_current_title();
    }
}
