using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class SelectPageInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image jacket;
    [SerializeField] private Button enter_btn;
    [SerializeField] private Button delete_btn;

    public void OnEnable()
    {
        title.gameObject.SetActive(false);
        jacket.gameObject.SetActive(false);
        enter_btn.gameObject.SetActive(false);
        delete_btn.gameObject.SetActive(false);

        LoadManager.select_novel_event += set_information;   
    }

    public void OnDisable()
    {
        LoadManager.select_novel_event -= set_information;
    }

    public void set_information()
    {
        title.gameObject.SetActive(true);
        jacket.gameObject.SetActive(true);
        enter_btn.gameObject.SetActive(true);
        delete_btn.gameObject.SetActive(true);

        title.text = LoadManager.loadmanager.ret_current_title();
        jacket.sprite = LoadManager.loadmanager.ret_current_sprite();
    }

    public void enter_story()
    {
        this.gameObject.SetActive(false);
    }

    public void delete_story()
    {
        LoadManager.loadmanager.delete_current_element();

        title.gameObject.SetActive(false);
        jacket.gameObject.SetActive(false);
        enter_btn.gameObject.SetActive(false);
        delete_btn.gameObject.SetActive(false);
    }
}
