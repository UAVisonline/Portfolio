using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AddTitlePage : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputfield;
    [SerializeField] private TextMeshProUGUI input_text;
    [SerializeField] [ReadOnly] private int code;

    private void OnEnable()
    {
        inputfield.text = "";
        input_text.text = "";
        code = 0;
    }

    public void set_code(int i)
    {
        code = i;
    }

    public void insert_func()
    {
        LoadManager.loadmanager.insert_element(input_text.text, code);
    }
}
