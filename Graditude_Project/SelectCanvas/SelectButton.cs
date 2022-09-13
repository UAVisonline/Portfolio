using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SelectButton : MonoBehaviour
{
    [BoxGroup("Reference")] private Album_Select select_obj;
    [BoxGroup("Reference")] private Button button;
    [BoxGroup("Reference")] [SerializeField] private TextMeshProUGUI tmpro;
    [BoxGroup("Reference")] [SerializeField] private GameObject outline;

    [BoxGroup("Variable")] [SerializeField] private string pre_string;
    [BoxGroup("Variable")] [SerializeField] private level_difficulty level;
    [BoxGroup("Variable")] [SerializeField] private director_mode directing_value;
    [BoxGroup("Variable")] [SerializeField] private multiplayer_setting multi_value;


    private void Awake()
    {
        select_obj = FindObjectOfType<Album_Select>();
        button = this.GetComponent<Button>();
    }

    public void Click_button_level()
    {
        if(select_obj!=null)
        {
            select_obj.set_level_difficulty(level);
            this.GetComponent<AudioSource>().Play();
        }
    }

    public void Click_button_direct()
    {
        if (select_obj != null)
        {
            select_obj.set_directing_mode(directing_value);
            this.GetComponent<AudioSource>().Play();
        }
    }

    public void Click_buttom_multiplay()
    {
        if (select_obj != null)
        {
            select_obj.set_multiplayer(multi_value);          
            this.GetComponent<AudioSource>().Play();            
        }        
    }

    public void Interact_change_level(level_difficulty value)
    {
        if(level != value)
        {
            outline.SetActive(false);
            //button.interactable = true;
        }
        else
        {
            outline.SetActive(true);
            //button.interactable = false;
        }
    }

    public void Interact_change_director(director_mode value)
    {
        if(directing_value != value)
        {
            outline.SetActive(false);
        }
        else
        {
            outline.SetActive(true);
        }
    }

    public void Interact_change_multiplay(multiplayer_setting value)
    {
        if (multi_value != value)
        {
            outline.SetActive(false);
        }
        else
        {
            outline.SetActive(true);
        }
    }

    public void change_text(string value)
    {
        tmpro.text = pre_string + "\n" + value;

        if(value != "---")
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
