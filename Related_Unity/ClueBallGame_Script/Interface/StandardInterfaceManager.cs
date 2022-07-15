using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandardInterfaceManager : InterfaceManager
{
    private static StandardInterfaceManager _standardmanager;

    public static StandardInterfaceManager standardmanager
    {
        get
        {
            if (_standardmanager == null)
            {
                _standardmanager = FindObjectOfType<StandardInterfaceManager>();
                if (_standardmanager == null)
                {
                    Debug.LogError("There is no StandardInterface Class or Can't load StandardInterface Class");
                }
            }

            return _standardmanager;
        }
    }

    private int number_of_choice;
    private int current_button;

    [SerializeField] private float bottom_position;
    [SerializeField] private float interval_of_button;

    public void set_number_of_choice(int value) // y_pos 설정을 위한 함수 (전체 보기의 갯수를 불러오며 현재 button이 몇번째인지 확인)
    {
        number_of_choice = value;
        current_button = 1;
    }

    public override void set_button_event(int index, CommandObject value, string str = "")
    {
        int variable = ( number_of_choice ) - index; // 
        float y_pos = bottom_position + interval_of_button * variable; // 버튼 위치 설정

        int tmp = current_button;
        if(index==9) // back button에 대한 경우
        {
            tmp = 9;
        }
        else // 그렇지 않은 경우
        {
            current_button++;
        }

        switch (tmp)
        {
            case 1:
                first_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, y_pos);
                first_button.set_command_object(value, str);
                break;
            case 2:
                second_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, y_pos);
                second_button.set_command_object(value, str);
                break;
            case 3:
                third_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, y_pos);
                third_button.set_command_object(value, str);
                break;
            case 4:
                fourth_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, y_pos);
                fourth_button.set_command_object(value, str);
                break;
            case 5:
                fifth_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, y_pos);
                fifth_button.set_command_object(value, str);
                break;
            case 6:
                sixth_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, y_pos);
                sixth_button.set_command_object(value, str);
                break;
            case 7:
                seventh_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, y_pos);
                seventh_button.set_command_object(value, str);
                break;
            case 8:
                eighth_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, y_pos);
                eighth_button.set_command_object(value, str);
                break;
            case 9:
                ninth_button.set_command_object(value, str, false); // backbutton은 위와 다른 위치 규칙을 사용하므로 위에서 사용하는 위치 방식을 사용하지 않음
                break;
        }
    }
}
