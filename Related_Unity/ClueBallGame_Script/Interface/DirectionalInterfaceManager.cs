using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalInterfaceManager : InterfaceManager
{
    private static DirectionalInterfaceManager _directionalInterfaceManager;

    public static DirectionalInterfaceManager directionalInterfaceManager
    {
        get
        {
            if (_directionalInterfaceManager == null)
            {
                _directionalInterfaceManager = FindObjectOfType<DirectionalInterfaceManager>();
                if (_directionalInterfaceManager == null)
                {
                    Debug.LogError("There is no DirectionalInterfaceManager Class or Can't load DirectionalInterfaceManager Class");
                }
            }

            return _directionalInterfaceManager;
        }
    }

    [SerializeField] protected ButtonUI back_button;
    [SerializeField] protected GameObject Circle; // (가시성을 위해서 존재)

    public override void set_active_false()
    {
        base.set_active_false();
        back_button.gameObject.SetActive(false); // Back Button False
        Circle.SetActive(false); // Circle Obejct False
    }

    public override void set_button_event(int index, CommandObject value, string str = "")
    {
        if(index!=0) 
        {
            if(Circle.activeSelf==false)
            {
                Circle.SetActive(true);// 가시성 Object를 True 상태로 변경
            }
        }

        switch (index)
        {
            case 0:
                back_button.set_command_object(value, str, false); // BackButton 할당
                break;
            case 1:
                first_button.set_command_object(value, str, false);
                break;
            case 2:
                second_button.set_command_object(value, str, false);
                break;
            case 3:
                third_button.set_command_object(value, str, false);
                break;
            case 4:
                fourth_button.set_command_object(value, str, false);
                break;
            case 5:
                //fifth_button.set_command_object(value, str, true); // 사용하지 않음 (가시성을 위하여)
                break;
            case 6:
                sixth_button.set_command_object(value, str, false);
                break;
            case 7:
                seventh_button.set_command_object(value, str, false);
                break;
            case 8:
                eighth_button.set_command_object(value, str, false);
                break;
            case 9:
                ninth_button.set_command_object(value, str, false);
                break;
        }
    }
}
