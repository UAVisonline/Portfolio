using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopColorPalate : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ColorPalate ref_palate;
    [SerializeField] private int index;

    [SerializeField] private Button interaction_btn;
    [SerializeField] private Text interaction_text;

    [SerializeField] private Image cape_image;
    [SerializeField] private Image body_image;

    public void OnPointerClick(PointerEventData eventData) // Icon 클릭 시 Shop Material에 이를 먼저 적용 (Skin 미리보기 구현)
    {
        ShopManager.shopmanager.shop_shader(index);
    }

    private void Start()
    {
        index = ShopManager.shopmanager.get_color_index(ref_palate.color_name); // ref Palate index 불러오기

        update_palate(); 
        cape_image.color = ref_palate.cape_color;
        body_image.color = ref_palate.body_color;

        information_init(); // UI 정보 시각화
    }

    public int get_index()
    {
        return index;
    }

    public void update_palate()
    {
        ref_palate = ShopManager.shopmanager.get_palate_information(index);
    }

    public void information_init()
    {
        if(ref_palate.equipment == true) // 해당 Skin을 장착중인 경우
        {
            interaction_btn.interactable = false;
            interaction_text.text = "장착중";
        }
        else
        {
            interaction_btn.interactable = true;
            if(ref_palate.purchased==true) // 해당 Skin을 구매한 경우
            {
                interaction_text.text = "장착가능";
            }
            else // 그렇지도 않은 경우
            {
                interaction_text.text = ref_palate.money.ToString();
            }
        }
    }

    public void btn_interaction()
    {
        if(ref_palate.purchased==false)
        {
            if (ShopManager.shopmanager.minus_coin(ref_palate.money) == true)
            {
                ShopManager.shopmanager.purchased_color(index);
            }
        }
        else
        {
            ShopManager.shopmanager.equipment_color(index);
        }
    }

    public bool is_equip()
    {
        return ref_palate.equipment;
    }
}
