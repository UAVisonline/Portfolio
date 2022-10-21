using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;


public class ChapterPageInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI detail;
    [SerializeField] private Button enter_btn;
    [SerializeField] private Button delete_btn;

    public void OnEnable()
    {
        title.gameObject.SetActive(false);
        detail.gameObject.SetActive(false);
        enter_btn.gameObject.SetActive(false);
        delete_btn.gameObject.SetActive(false);

        LoadManager.select_chapter_event += set_information;
    }

    public void OnDisable()
    {
        LoadManager.select_chapter_event -= set_information;
    }

    public void set_information()
    {
        title.gameObject.SetActive(true);
        detail.gameObject.SetActive(true);
        enter_btn.gameObject.SetActive(true);
        delete_btn.gameObject.SetActive(true);

        title.text = LoadManager.loadmanager.ret_current_chapter();
        detail.text = "";

        List<string> tmp_detail = LoadManager.loadmanager.ret_detail_of_chapter();
        for(int i =0;i<tmp_detail.Count;i++)
        {
            detail.text += tmp_detail[i];
            detail.text += "\n";
        }
    }

    public void enter_chapter()
    {
        // title.gameObject.SetActive(false);
        // enter_btn.gameObject.SetActive(false);
        // delete_btn.gameObject.SetActive(false);
    }

    public void delete_chapter()
    {
        LoadManager.loadmanager.delete_current_chapter();

        title.gameObject.SetActive(false);
        detail.gameObject.SetActive(false);
        enter_btn.gameObject.SetActive(false);
        delete_btn.gameObject.SetActive(false);
    }
}
