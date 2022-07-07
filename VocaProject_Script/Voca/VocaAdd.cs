using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VocaAdd : MonoBehaviour
{
    [SerializeField] private InputField hd_input;
    [SerializeField] private TMP_InputField bd_input;

    [SerializeField] private Text hd;
    [SerializeField] private TextMeshProUGUI bd;

    [SerializeField] private GameObject turn_off;
    [SerializeField] private GameObject turn_on;

    public void voca_add()
    {
        bool white_space = true;
        for(int i =0;i<hd.text.Length;i++)
        {
            if(hd.text[i]!= ' ') // 입력하고자 하는 단어의 공백이 한 번이라도 존재하지 않으면
            {
                white_space = false; // 공백이 아니다
                break;
            }
        }

        if(white_space==true) // 만약 입력하고자 하는 단어가 공백이면
        {
            hd_input.Select();
            hd_input.text = "공백단어는 입력할 수 없습니다.";
            hd.text = "공백단어는 입력할 수 없습니다.";
            return;
        }

        if(bd.text == "-----") // 만약 입력하는 단어의 뜻이 -----이면
        {
            bd_input.Select();
            bd_input.text = "----- 은 뜻으로 입력할 수 없습니다.";
            bd.text = "----- 은 뜻으로 입력할 수 없습니다.";
            return;
        }

        if(VocaMaster.vocaMaster.insert_voca(hd.text, bd.text)==true) // 단어 추가가 성공적으로 이루어 진 경우
        {
            move_field();
        }
        else // 이미 단어장에 해당 단어가 추가되있는 경우
        {
            hd_input.Select();
            hd_input.text = "해당 단어는 이미 단어장에 입력되어 있습니다.";
            hd.text = "해당 단어는 이미 단어장에 입력되어 있습니다.";
            return;
        }

    }

    public void move_field() // 단어 추가 후 다시 add button을 통해 단어를 입력했는데 이전 내용이 남아있으면 안되므로
    {
        hd_input.Select();
        hd_input.text = "";

        bd_input.Select();
        bd_input.text = "";
        
        hd.text = "";
        bd.text = "";
        // InputField 및 Text를 초기화

        turn_off.SetActive(false); // 꺼야 할 오브젝트
        turn_on.SetActive(true); // 켜야 할 오브젝트
    }
}
