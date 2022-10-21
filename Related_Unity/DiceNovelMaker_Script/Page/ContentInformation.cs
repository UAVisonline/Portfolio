using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class ContentInformation : MonoBehaviour
{
    [SerializeField] private Image question_image;

    [SerializeField] private TextMeshProUGUI list_size_text;
    [SerializeField] private Button modify_btn;
    [SerializeField] private Button delete_btn;
    [SerializeField] private GameObject text_content;

    [SerializeField] [ReadOnly] private List<GameObject> text_content_list = new List<GameObject>();

    private void OnEnable()
    {
        init_information();

        LoadManager.select_content_event += set_information;
    }

    private void OnDisable()
    {
        LoadManager.select_content_event -= set_information;
    }

    public void set_information()
    {
        if (text_content_list.Count > 0)
        {
            for (int i = 0; i < text_content_list.Count; i++)
            {
                Destroy(text_content_list[i]);
            }
            text_content_list.Clear();
        }

        question_image.gameObject.SetActive(false);

        modify_btn.interactable = true;
        delete_btn.interactable = true;

        List<string> tmp = LoadManager.loadmanager.ret_detail_of_content();
        for(int i =0;i<tmp.Count;i++)
        {
            GameObject tmp_text = Instantiate(text_content, this.transform);
            tmp_text.GetComponent<TextMeshProUGUI>().text = tmp[i];
            text_content_list.Add(tmp_text);
        }
    }

    public void init_information()
    {
        question_image.gameObject.SetActive(true);

        list_size_text.text = LoadManager.loadmanager.ret_content_list().ToString() + "/250";
        modify_btn.interactable = false;
        delete_btn.interactable = false;

        if (text_content_list.Count > 0)
        {
            for (int i = 0; i < text_content_list.Count; i++)
            {
                Destroy(text_content_list[i]);
            }
            text_content_list.Clear();
        }
    }

    public void delete_func()
    {
        LoadManager.loadmanager.delete_current_content();

        init_information();
    }
}
