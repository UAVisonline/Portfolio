using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SelectPageContent : MonoBehaviour
{
    [SerializeField] private GameObject story_button;

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

        int size = LoadManager.loadmanager.ret_size_list();

        for (int i = 0; i < size; i++)
        {
            GameObject tmp = Instantiate(story_button, this.transform);
            tmp.GetComponent<StoryButton>().set_information(i);
            buttons.Add(tmp);
        }
    }
}
