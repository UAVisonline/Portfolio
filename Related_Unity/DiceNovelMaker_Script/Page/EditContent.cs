using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class EditContent : MonoBehaviour
{
    [SerializeField] private TMP_InputField content_title_input_field;
    [SerializeField] private TextMeshProUGUI content_title;

    [SerializeField] private GameObject input_detail;

    [SerializeField] private List<GameObject> details = new List<GameObject>();

    private void OnEnable()
    {
        if(details.Count>0)
        {
            for(int i =0;i<details.Count;i++)
            {
                Destroy(details[i]);
            }
            details.Clear();
        }

        List<string> ref_content_detail = LoadManager.loadmanager.ret_detail_of_content();
        content_title_input_field.text = "";
        content_title.text = "";

        if(ref_content_detail.Count>0)
        {
            for(int i =0;i<ref_content_detail.Count;i++)
            {
                GameObject tmp = Instantiate(input_detail, this.transform);
                tmp.GetComponent<InputDetail>().set_information(ref_content_detail[i], i);
                tmp.GetComponent<InputDetail>().set_edit_master(this);
                details.Add(tmp);
            }
            content_title_input_field.text = LoadManager.loadmanager.ret_current_content();
        }
        else
        {
            GameObject tmp = Instantiate(input_detail, this.transform);
            tmp.GetComponent<InputDetail>().set_information("새로운 내용을 입력해주세요.", 0);
            tmp.GetComponent<InputDetail>().set_edit_master(this);
            details.Add(tmp);
        }
    }

    public void delete_input_field(int index)
    {
        GameObject tmp = details[index];
        details.RemoveAt(index);
        Destroy(tmp);
        
        for(int i =0;i<details.Count;i++)
        {
            details[i].GetComponent<InputDetail>().set_index(i);
        }
    }

    public void insert_input_field()
    {
        GameObject tmp = Instantiate(input_detail, this.transform);
        tmp.GetComponent<InputDetail>().set_information("새로운 내용을 입력해주세요.", details.Count);
        tmp.GetComponent<InputDetail>().set_edit_master(this);
        details.Add(tmp);
    }

    public void insert_dice_input(int loop, int dice)
    {
        //Debug.Log(loop + "   " + dice);

        string dice_result = loop.ToString() + "D" + dice.ToString() + " = {";

        for(int i =0;i<loop;i++)
        {
            int number = Random.Range(0, dice) + 1;
            dice_result += number.ToString();
            if(i<loop-1)
            {
                dice_result += ",";
            }
        }
        dice_result += "}";

        GameObject tmp = Instantiate(input_detail, this.transform);
        tmp.GetComponent<InputDetail>().set_information(dice_result, details.Count);
        tmp.GetComponent<InputDetail>().set_edit_master(this);
        details.Add(tmp);
    }

    public void insert_edit_content()
    {
        List<string> result = new List<string>();
        for(int i =0;i<details.Count;i++)
        {
            string tmp = details[i].GetComponent<InputDetail>().get_information();
            result.Add(tmp);
        }

        string text_parameter = "";
        for(int i = 0;i<content_title.text.Length;i++)
        {
            if((int)content_title.text[i] != 8203 && (int)content_title.text[i]!=160) // text 값 중 Zero width space나 Non-breaking space가 나오는 경우가 아니라면
            {
                text_parameter += content_title.text[i];
            }
        } // Zero width Space 문제 해결

        LoadManager.loadmanager.insert_content(text_parameter, result);
    }
}
