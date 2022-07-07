using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCategory : MonoBehaviour // Skill Category UI
{
    [SerializeField] private SkillScriptableObject ref_skill; // 해당 UI가 참조하는 Skill Object
    [SerializeField] private int index; // 해당 SkillCategory가 참조하는 Skill에 index

    [SerializeField] private Button interaction_btn;
    [SerializeField] private Text interaction_text;

    [SerializeField] private Image skill_image;

    private void Start()
    {
        index = ShopManager.shopmanager.get_skill_index(ref_skill.skill_name); // ShopManager로부터 Skill Index를 참조받아 설정함

        skill_image.sprite = ref_skill.skill_icon;
        information_init(); // 정보 시각화
    }

    public int get_index()
    {
        return index;
    }

    public void information_init()
    {
        if (ref_skill.equipment == true) // 현재 Skill 장착중이면
        {
            interaction_btn.interactable = false;
            interaction_text.text = "장착중";
        }
        else // 그렇지 않은 경우
        {
            interaction_btn.interactable = true;
            if (ref_skill.purchased == true) // 해당 Skill은 구매한 경우
            {
                interaction_text.text = "장착가능";
            }
            else // 구매하지 않은 경우
            {
                interaction_text.text = ref_skill.money.ToString();
            }
        }
    }

    public void btn_interaction()
    {
        if (ref_skill.purchased == false) // 구매하지 않은 경우
        {
            if (ShopManager.shopmanager.minus_coin(ref_skill.money) == true) // Skill을 살 수 있는 돈이 충분하다면
            {
                ShopManager.shopmanager.purchased_skill(index); // Skill 구매
            }
        }
        else // 구매한 경우 (Skill을 장착)
        {
            ShopManager.shopmanager.equipment_skill(index);
        }
    }
}
