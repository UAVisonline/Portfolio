using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddChapterPage : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputfield_title;
    [SerializeField] private TextMeshProUGUI title_text;

    [SerializeField] private TMP_InputField inputfield_detail;
    [SerializeField] private TextMeshProUGUI detail_text;

    private void OnEnable()
    {
        inputfield_title.text = "";
        title_text.text = "";

        inputfield_detail.text = "";
        detail_text.text = "";
    }

    public void insert_func()
    {
        List<string> detail = new List<string>();
        string[] tmp = detail_text.text.Split('\n');
        for(int i =0;i<tmp.Length;i++)
        {
            detail.Add(tmp[i]);
        }

        LoadManager.loadmanager.insert_chapter(title_text.text, detail);
    }
}
