using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ChapterContent : MonoBehaviour
{
    [SerializeField] private GameObject chapter_button;

    [SerializeField] [ReadOnly] List<GameObject> buttons = new List<GameObject>();

    private void OnEnable()
    {
        make_content_button();
    }

    public void make_content_button()
    {
        if (buttons.Count > 0)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i]);
            }
            buttons.Clear();
        }

        int size = LoadManager.loadmanager.ret_chapter_list();

        for (int i = 0; i < size; i++)
        {
            GameObject tmp = Instantiate(chapter_button, this.transform);
            tmp.GetComponent<ChapterButton>().set_information(i);
            buttons.Add(tmp);
        }
    }
}
