using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Voca_Delete : MonoBehaviour
{
    [SerializeField] private VocaControlTower control;

    [SerializeField] private TextMeshProUGUI voca_content;

    public void init_delete()
    {
        voca_content.text = VocaMaster.vocaMaster.get_list_content(VocaMaster.vocaMaster.get_detail_index()); // 삭제할 단어를 text로 표시하도록 함
    }

    public void animation_update(bool value) // 단어 삭제 Animator 내 변수를 설정 (간단한 Slide 효과를 주어서 삭제할 단어 표시)
    {
        this.GetComponent<Animator>().SetBool("Anim", value);
    }

    public void yes_btn()
    {
        VocaMaster.vocaMaster.delete_voca(voca_content.text); // VocaMaster 내 삭제할 단어를 탐색->그 후 삭제
        //control.none_btn();
        //control.init_voca();
    }

    public void no_btn()
    {
        control.none_btn(); // VocaControlTower Script 상태 변화
    }
}
