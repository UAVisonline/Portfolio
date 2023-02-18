using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Amulet_Gacha_System : MonoBehaviour
{
    [SerializeField] private int numbers;
    [SerializeField] private int selected;

    [SerializeField] private Image image_left; // 1
    [SerializeField] private Image image_mid; // 2
    [SerializeField] private Image image_right; // 3

    [SerializeField] private Amulet_information left_information;
    [SerializeField] private Amulet_information mid_information;
    [SerializeField] private Amulet_information right_information;

    [SerializeField] private Sprite null_image;

    [SerializeField] private Image frame;

    private int left_code;
    private int mid_code;
    private int right_code;

    [SerializeField] private Button get_btn;
    [SerializeField] private Button exit_btn;

    [SerializeField] private TextMeshProUGUI amulet_name;
    [SerializeField] private TextMeshProUGUI amulet_information;
    [SerializeField] private AudioClip gacha_sound;

    private void OnEnable()
    {
        selected = 0;
        get_btn.interactable = false;
        frame.gameObject.SetActive(false);
        amulet_name.text = "";
        amulet_information.text = "";

        Util_Manager.utilManager.play_clip(gacha_sound);
    }

    public void selected_set(int value)
    {

        switch(numbers)
        {
            case 1:
                if(value==2)
                {
                    frame.gameObject.SetActive(true);
                    frame.rectTransform.position = image_mid.GetComponent<RectTransform>().position;
                    selected = 2;
                    get_btn.interactable = true;
                    visulize_information();
                }
                break;
            case 2:
                if(value==1)
                {
                    frame.gameObject.SetActive(true);
                    frame.rectTransform.position = image_left.GetComponent<RectTransform>().position;
                    selected = 1;
                    get_btn.interactable = true;
                    visulize_information();
                }
                if(value==3)
                {
                    frame.gameObject.SetActive(true);
                    frame.rectTransform.position = image_right.GetComponent<RectTransform>().position;
                    selected = 3;
                    get_btn.interactable = true;
                    visulize_information();
                }
                break;
            case 3:
                frame.gameObject.SetActive(true);
                selected = value;
                get_btn.interactable = true;
                visulize_information();
                if (value==1)
                {
                    frame.rectTransform.position = image_left.GetComponent<RectTransform>().position;
                }
                if(value==2)
                {
                    frame.rectTransform.position = image_mid.GetComponent<RectTransform>().position;
                }
                if(value==3)
                {
                    frame.rectTransform.position = image_right.GetComponent<RectTransform>().position;
                }
                break;
        }
    }

    public void get_alchemy()
    {
        switch(selected)
        {
            case 1:
                AmuletManager.amuletmanager.insert_uncarried_amulet(left_code);
                break;
            case 2:
                AmuletManager.amuletmanager.insert_uncarried_amulet(mid_code);
                break;
            case 3:
                AmuletManager.amuletmanager.insert_uncarried_amulet(right_code);
                break;
        }
    }

    public void numbers_set(int value)
    {
        left_code = -1;
        right_code = -1;
        mid_code = -1;
        left_information = null;
        mid_information = null;
        right_information = null;

        selected = 0;
        get_btn.interactable = false;
        frame.gameObject.SetActive(false);
        amulet_name.text = "";
        amulet_information.text = "";

        numbers = value;

        if(value==1)
        {
            mid_code = code_set();
            mid_information = AmuletManager.amuletmanager.amulet_dictionary[mid_code].GetComponent<BaseAmuletScript>().information;
            while(mid_information.duplicate_limit>0)
            {
                if(PlayerManager.playerManager.ret_number_same_amulet(mid_code)>=mid_information.duplicate_limit)
                {
                    mid_code = code_set();
                    mid_information = AmuletManager.amuletmanager.amulet_dictionary[mid_code].GetComponent<BaseAmuletScript>().information;
                }
                else
                {
                    break;
                }
            }
            image_mid.sprite = mid_information.amulet_jacket;

            image_left.sprite = null_image;
            image_right.sprite = null_image;
        }
        if(value==2)
        {
            left_code = code_set();
            left_information = AmuletManager.amuletmanager.amulet_dictionary[left_code].GetComponent<BaseAmuletScript>().information;
            while (left_information.duplicate_limit>0)
            {
                if (PlayerManager.playerManager.ret_number_same_amulet(left_code)>=left_information.duplicate_limit)
                {
                    left_code = code_set();
                    left_information = AmuletManager.amuletmanager.amulet_dictionary[left_code].GetComponent<BaseAmuletScript>().information;
                }
                else
                {
                    break;
                }
            }
            image_left.sprite = left_information.amulet_jacket;

            right_code = code_set();
            right_information = AmuletManager.amuletmanager.amulet_dictionary[right_code].GetComponent<BaseAmuletScript>().information;
            while (right_information.duplicate_limit>0)
            {
                if (PlayerManager.playerManager.ret_number_same_amulet(right_code)>=right_information.duplicate_limit)
                {
                    right_code = code_set();
                    right_information = AmuletManager.amuletmanager.amulet_dictionary[right_code].GetComponent<BaseAmuletScript>().information;
                }
                else
                {
                    break;
                }
            }
            image_right.sprite = right_information.amulet_jacket;

            image_mid.sprite = null_image;
        }
        if(value==3)
        {
            left_code = code_set();
            left_information = AmuletManager.amuletmanager.amulet_dictionary[left_code].GetComponent<BaseAmuletScript>().information;
            while (left_information.duplicate_limit>0)
            {
                if (PlayerManager.playerManager.ret_number_same_amulet(left_code)>=left_information.duplicate_limit)
                {
                    left_code = code_set();
                    left_information = AmuletManager.amuletmanager.amulet_dictionary[left_code].GetComponent<BaseAmuletScript>().information;
                }
                else
                {
                    break;
                }
            }
            image_left.sprite = left_information.amulet_jacket;

            mid_code = code_set();
            mid_information = AmuletManager.amuletmanager.amulet_dictionary[mid_code].GetComponent<BaseAmuletScript>().information;
            while (mid_information.duplicate_limit>0)
            {
                if (PlayerManager.playerManager.ret_number_same_amulet(mid_code)>=mid_information.duplicate_limit)
                {
                    mid_code = code_set();
                    mid_information = AmuletManager.amuletmanager.amulet_dictionary[mid_code].GetComponent<BaseAmuletScript>().information;
                }
                else
                {
                    break;
                }
            }
            image_mid.sprite = mid_information.amulet_jacket;

            right_code = code_set();
            right_information = AmuletManager.amuletmanager.amulet_dictionary[right_code].GetComponent<BaseAmuletScript>().information;
            while (right_information.duplicate_limit>0)
            {
                if (PlayerManager.playerManager.ret_number_same_amulet(right_code)>=right_information.duplicate_limit)
                {
                    right_code = code_set();
                    right_information = AmuletManager.amuletmanager.amulet_dictionary[right_code].GetComponent<BaseAmuletScript>().information;
                }
                else
                {
                    break;
                }
            }
            image_right.sprite = right_information.amulet_jacket;
        }
    }

    private int code_set()
    {
        int size = AmuletManager.amuletmanager.ret_amulet_code_list_size();

        while(true)
        {
            int pos = Random.Range(0, size);
            int code_value = AmuletManager.amuletmanager.ret_amulet_code_list_pos(pos);

            if(left_code == code_value || mid_code == code_value || right_code == code_value)
            {
                continue;
            }

            return code_value;
        }
    }

    private void visulize_information()
    {
        Amulet_information tmp = null;
        switch(selected)
        {
            case 1:
                tmp = left_information;
                break;
            case 2:
                tmp = mid_information;
                break;
            case 3:
                tmp = right_information;
                break;
        }

        if(tmp!=null)
        {
            amulet_name.text = tmp.name;
            amulet_information.text = tmp.information;
        }
    }
}
