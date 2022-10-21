using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ContentList : MonoBehaviour
{
    [SerializeField] private GameObject content_button;

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

        int size = LoadManager.loadmanager.ret_content_list();

        for (int i = 0; i < size; i++)
        {
            GameObject tmp = Instantiate(content_button, this.transform);
            tmp.GetComponent<ContentButton>().set_information(i);
            buttons.Add(tmp);
        }
    }
}
