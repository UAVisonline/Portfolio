using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private InterfaceManager interface_manager;
    [SerializeField] private CommandObject command_object;
    [SerializeField] private int index;
    [SerializeField] private Text button_text;
    [SerializeField] private KeyCode key;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            click_event();
        }
    }

    public void click_event()
    {
        //Debug.Log(this.gameObject.name);

        if (command_object!=null && GameManager.gamemanager.get_pause()==false) // 일시정지 시 입력 불가능하게 수정
        {
            GameManager.gamemanager.click_play();
            interface_manager.set_active_false();
            command_object.execute();
        }
    }

    public void set_command_object(CommandObject value, string str, bool number_marking = true)
    {
        command_object = value; // command obejct 할당
        set_text(str, number_marking); // 버튼 text 설정 (숫자도 보이게 할 것인가?)
        this.gameObject.SetActive(true); // 버튼 Setactive True
    }

    private void set_text(string value, bool number_marking) // 버튼 Text 설정
    {
        if(number_marking==true)
        {
            button_text.text = index.ToString() + ". " + value;
        }
        else
        {
            button_text.text = value;
        }
    }
}
