using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Information_update : MonoBehaviour
{
    [SerializeField] private List<Image> computer_information_image;
    [SerializeField] private List<Image> human_information_image;
    [SerializeField] private List<Child_information_update> childs; //실제 사람 정보를 변경하는데 사용될 Class Script

    [SerializeField] private List<information> computer_informations;
    [SerializeField] private List<information> human_informations;

    [SerializeField] private Sprite none; //정보 미정
    [SerializeField] private Sprite wondering; // 정보 헷갈림
    [SerializeField] private Sprite not_this_one; // 정보 배제
    [SerializeField] private Sprite this_one; // 정보 확정

    public void set_information_computer(int index, information value) // Computer가 설정한 정보값을 반영 (Sprite 변경)
    {
        computer_informations[index] = value;
        switch (value)
        {
            case information.none:
                computer_information_image[index].sprite = none;
                break;
            case information.wondering:
                computer_information_image[index].sprite = wondering;
                break;
            case information.not_this:
                computer_information_image[index].sprite = not_this_one;
                break;
            case information.this_one:
                computer_information_image[index].sprite = this_one;
                break;
        }
    }

    public void set_information_human(int index, information value) // 사람이 설정, 유추한 정보값을 반영 (Sprite 변경)
    {
        human_informations[index] = value;
        switch (value)
        {
            case information.none:
                human_information_image[index].sprite = none;
                break;
            case information.wondering:
                human_information_image[index].sprite = wondering;
                break;
            case information.not_this:
                human_information_image[index].sprite = not_this_one;
                break;
            case information.this_one:
                human_information_image[index].sprite = this_one;
                break;
        }
    }

    public void init_childs()
    {
        for(int i = 0;i<childs.Count;i++)
        {
            childs[i].init_current_information(); // 사람 정보를 기본으로 초기화
        }
    }
}
