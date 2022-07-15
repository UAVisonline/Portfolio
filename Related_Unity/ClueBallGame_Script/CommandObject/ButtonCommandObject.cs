using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonCommandObject : CommandObject
{
    [SerializeField] private int button_index_standard; // Standard mode UI index
    [SerializeField] private int button_index_directional; // Directional mode UI index
    [SerializeField] private string button_name;

    public void set_button()
    {
        if(GameManager.gamemanager.get_mode()==Interface_mode.standard)
        {
            StandardInterfaceManager.standardmanager.set_button_event(button_index_standard, this, button_name); // StandardInterface를 불러와서 Command Object 할당
        }
        else if(GameManager.gamemanager.get_mode() == Interface_mode.direction)
        {
            DirectionalInterfaceManager.directionalInterfaceManager.set_button_event(button_index_directional, this, button_name); // DirectionalInterface를 불러와서 Command Object 할당
        }
    }
}
