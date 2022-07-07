using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Voca_index : MonoBehaviour
{
    [SerializeField] private int index; // 해당 Script의 index (0~4)
    private string hd;

    [SerializeField] private Button hd_button;
    [SerializeField] private Button trash_button;
    [SerializeField] private TextMeshProUGUI hd_text;

    public void voca_init()
    {
        int ref_index = index + VocaMaster.vocaMaster.get_index()*5; // Voca Master 및 index 지역변수를 이용해 참조할 index를 설정
        if(VocaMaster.vocaMaster.is_list_content(ref_index)==true) // 해당 ref index에 단어가 존재하면
        {
            hd = VocaMaster.vocaMaster.get_list_content(ref_index);
            hd_text.text = hd;
            hd_button.interactable = true;
            trash_button.interactable = true;
            // 단어를 읽을 수 있게 하도록 함
        }
        else
        {
            hd = "-----";
            hd_text.text = hd;
            hd_button.interactable = false;
            trash_button.interactable = false;
            // 읽을 수 없게 하도록 함 (읽을 값이 없으므로)
        }
    }

    public void set_master_detail_index() // hd button을 누르면 발생
    {
        VocaMaster.vocaMaster.set_detail_index(index + VocaMaster.vocaMaster.get_index()*5); // VocaMaster의 상세 index를 설정 (실제 읽고자 하는 단어의 index)
    }
}
