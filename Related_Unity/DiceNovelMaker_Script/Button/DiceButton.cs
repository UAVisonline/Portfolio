using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField loop_field;
    [SerializeField] private TextMeshProUGUI loop_text;

    [SerializeField] private TMP_InputField number_field;
    [SerializeField] private TextMeshProUGUI number_text;

    [SerializeField] private EditContent edit;

    private void OnEnable()
    {
        loop_field.text = "";
        loop_text.text = "";

        number_field.text = "";
        number_text.text = "";
    }

    public void func()
    {
        string loop_txt = "";
        string number_txt = "";

        for(int i =0;i<loop_text.text.Length;i++)
        {
            if(loop_text.text[i] - '0' >= 0 && loop_text.text[i] - '9' <= 0)
            {
                loop_txt += loop_text.text[i];
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < number_text.text.Length; i++)
        {
            if (number_text.text[i] - '0' >= 0 && number_text.text[i] - '9' <= 0)
            {
                number_txt += number_text.text[i];
            }
            else
            {
                break;
            }
        }

        int loop = int.Parse(loop_txt);
        int number = int.Parse(number_txt);

        edit.insert_dice_input(loop, number);

    }
}
