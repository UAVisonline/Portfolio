using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VocaDetail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hd; // 단어
    [SerializeField] private TextMeshProUGUI bd; // 실제 뜻
    [SerializeField] private TextMeshProUGUI level; // 해당 단어의 Level

    public void init_detail()
    {
        hd.text = VocaMaster.vocaMaster.get_list_content(VocaMaster.vocaMaster.get_detail_index()); // VocaMaster의 detail index를 통해 영단어를 읽음
        bd.text = VocaMaster.vocaMaster.get_dicitionary_body(hd.text); // VocaMaster에서 읽은 영단어의 뜻을 가지고 옴
        level.text = "Level : " + VocaMaster.vocaMaster.get_dictionary_level(hd.text).ToString("F1"); // 또한 읽은 영단어의 Level을 가지고 옴
    }

    public void animation_update(bool value)
    {
        this.GetComponent<Animator>().SetBool("Anim", value);
    }
}
