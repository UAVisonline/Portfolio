using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour // Interface에 대한 부모 클래스
{
    [SerializeField] protected ButtonUI first_button;
    [SerializeField] protected ButtonUI second_button;
    [SerializeField] protected ButtonUI third_button;
    [SerializeField] protected ButtonUI fourth_button;
    [SerializeField] protected ButtonUI fifth_button;
    [SerializeField] protected ButtonUI sixth_button;
    [SerializeField] protected ButtonUI seventh_button;
    [SerializeField] protected ButtonUI eighth_button;

    [SerializeField] protected ButtonUI ninth_button; // list interface = back button

    public virtual void set_active_false() // 버튼 Object False
    {
        first_button.gameObject.SetActive(false);
        first_button.GetComponent<Button>().interactable = true; // Standard interface에서 추리기능

        second_button.gameObject.SetActive(false);
        third_button.gameObject.SetActive(false);
        fourth_button.gameObject.SetActive(false);
        fifth_button.gameObject.SetActive(false);
        sixth_button.gameObject.SetActive(false);
        seventh_button.gameObject.SetActive(false);

        eighth_button.gameObject.SetActive(false);
        eighth_button.GetComponent<Button>().interactable = true; // Directional interface에서 추리기능

        ninth_button.gameObject.SetActive(false);
    }

    public void set_interactable_false_first_button()
    {
        first_button.GetComponent<Button>().interactable = false;
    }

    public void set_interactable_false_eighth_button()
    {
        eighth_button.GetComponent<Button>().interactable = false;
    }

    public virtual void set_button_event(int index, CommandObject value, string str="") // 버튼에 대해 CommandObject 설정 
    {
        switch(index)
        {
            case 1:
                first_button.set_command_object(value, str);
                break;
            case 2:
                second_button.set_command_object(value, str);
                break;
            case 3:
                third_button.set_command_object(value, str);
                break;
            case 4:
                fourth_button.set_command_object(value, str);
                break;
            case 5:
                fifth_button.set_command_object(value, str);
                break;
            case 6:
                sixth_button.set_command_object(value, str);
                break;
            case 7:
                seventh_button.set_command_object(value, str);
                break;
            case 8:
                eighth_button.set_command_object(value, str);
                break;
            case 9:
                ninth_button.set_command_object(value, str);
                break;
        }
    }
}
