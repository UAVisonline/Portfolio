using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputDetail : MonoBehaviour
{
    [SerializeField] private TMP_InputField input_field;
    [SerializeField] private TextMeshProUGUI real_text;

    [SerializeField] private int list_index;
    [SerializeField] private EditContent edit_master;

    public void set_information(string content, int index)
    {
        input_field.text = content;
        list_index = index;
    }

    public void set_index(int index)
    {
        list_index = index;
    }

    public string get_information()
    {
        return real_text.text;
    }

    public void set_edit_master(EditContent value)
    {
        edit_master = value;
    }

    public void delete_this_detail()
    {
        edit_master.delete_input_field(list_index);
    }

}
