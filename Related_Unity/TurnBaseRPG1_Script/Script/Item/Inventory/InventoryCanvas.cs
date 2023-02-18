using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryCanvas : MonoBehaviour // Do it Store Also
{
    [SerializeField] private GameObject inventory_object;
    [SerializeField] private GameObject information_object;
    [SerializeField] private GameObject store_object;

    [SerializeField] private Image selected_frame;
    [SerializeField] private Button buy_sell_button;
    [SerializeField] private Button function_button;
    [SerializeField] private Button trash_button;

    [SerializeField] private Image item_image;
    [SerializeField] private TextMeshProUGUI item_name_text;
    [SerializeField] private TextMeshProUGUI item_information_text;
    [SerializeField] private TextMeshProUGUI gold_text;
    [SerializeField] private TextMeshProUGUI player_gold_text;
    [SerializeField] private TextMeshProUGUI store_text;

    private InventoryFrame ref_frame;
    [SerializeField] private inventory_frame_enum frame_enum;
    [SerializeField] private int item_pos;

    [SerializeField] private Item_information ref_information;
    private RectTransform selected_frame_transform;

    [SerializeField] InventoryFrame weapon_frame;
    [SerializeField] InventoryFrame armor_frame;
    [SerializeField] InventoryFrame accessory_frame;
    [SerializeField] List<InventoryFrame> armed_frame;
    [SerializeField] List<InventoryFrame> consumption_frame;

    [SerializeField] private AudioClip buy_item_sfx;
    [SerializeField] private AudioClip sell_item_sfx;
    [SerializeField] private AudioClip armed_item_sfx;
    [SerializeField] private AudioClip trash_item_sfx;

    private void OnEnable()
    {
        inventory_object.gameObject.SetActive(true);
        information_object.gameObject.SetActive(true);
        store_object.gameObject.SetActive(false);

        frame_enum = inventory_frame_enum.weapon;
        item_pos = -1;
        ref_information = null;

        item_image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        selected_frame.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        item_name_text.text = "";
        item_information_text.text = "";
        gold_text.text = "";
        player_gold_text.text = "Player Gold : " + PlayerManager.playerManager.spec.current_gold.ToString();

        buy_sell_button.gameObject.SetActive(false);
        function_button.gameObject.SetActive(false);
        trash_button.gameObject.SetActive(false);

        if (InventoryManager.inventoryManager.ret_inshop()==true || InventoryManager.inventoryManager.ret_indrop()==true)
        {
            store_object.gameObject.SetActive(true);
            if(InventoryManager.inventoryManager.ret_inshop() == true)
            {
                store_text.text = "Store item";
            }
            else
            {
                store_text.text = "Drop item";
            }
        }

        if(selected_frame_transform==null)
        {
            selected_frame_transform = selected_frame.GetComponent<RectTransform>();
        }

        visualize_all_frame();
    }

    public void buy_sell_btn()
    {
        if(InventoryManager.inventoryManager.ret_inshop()==true)
        {
            if (frame_enum == inventory_frame_enum.store_armed || frame_enum == inventory_frame_enum.store_consumption)
            {
                if (frame_enum == inventory_frame_enum.store_armed)
                {
                    int pos = InventoryManager.inventoryManager.ret_empty_armed_pos();
                    if (PlayerManager.playerManager.gold_cal(-1 * Mathf.RoundToInt(PlayerManager.playerManager.schedule_information.gold_sale_percent * ref_information.gold)) == true)
                    {
                        making_item(pos);
                        Util_Manager.utilManager.play_clip(buy_item_sfx);
                    }
                }
                else
                {
                    int pos = InventoryManager.inventoryManager.ret_empty_consumption_pos();
                    if (PlayerManager.playerManager.gold_cal(-1 * Mathf.RoundToInt(PlayerManager.playerManager.schedule_information.gold_sale_percent * ref_information.gold)) == true)
                    {
                        making_item(pos);
                        Util_Manager.utilManager.play_clip(buy_item_sfx);
                    }
                }
                player_gold_text.text = "Player Gold : " + PlayerManager.playerManager.spec.current_gold.ToString();
            }
            else
            {
                if (PlayerManager.playerManager.gold_cal(1 * Mathf.RoundToInt(PlayerManager.playerManager.schedule_information.gold_sell_percent * ref_information.gold)) == true)
                {
                    delete_item();
                    Util_Manager.utilManager.play_clip(sell_item_sfx);
                    player_gold_text.text = "Player Gold : " + PlayerManager.playerManager.spec.current_gold.ToString();
                }
            }
        }
        else if(InventoryManager.inventoryManager.ret_indrop() == true)
        {
            if (frame_enum == inventory_frame_enum.store_armed)
            {
                int pos = InventoryManager.inventoryManager.ret_empty_armed_pos();
                making_item(pos);
                Util_Manager.utilManager.play_clip(buy_item_sfx);
            }
            else
            {
                int pos = InventoryManager.inventoryManager.ret_empty_consumption_pos();
                making_item(pos);
                Util_Manager.utilManager.play_clip(buy_item_sfx);
            }
            player_gold_text.text = "Player Gold : " + PlayerManager.playerManager.spec.current_gold.ToString();
            ref_frame.acquire_true();
        }
    }

    public void function_btn()
    {
        if(frame_enum==inventory_frame_enum.player_armed)
        {
            equip_item();
            Util_Manager.utilManager.play_clip(armed_item_sfx);
            //InventoryManager.inventoryManager.save_inventory();
            //PlayerManager.playerManager.save_spec();
        }
        else if(frame_enum==inventory_frame_enum.player_consumption)
        {
            // InventoryManager.inventoryManager.save_inventory();
            // 소지품 사용 PlayerManager.playerManager.save_spec();
            // delete_item();

            use_item();
        }
        else if(frame_enum==inventory_frame_enum.weapon || frame_enum==inventory_frame_enum.armor || frame_enum==inventory_frame_enum.accessory)
        {
            int value_pos = InventoryManager.inventoryManager.ret_empty_armed_pos();

            if(value_pos!=-1)
            {
                unequip_item(value_pos);
                Util_Manager.utilManager.play_clip(armed_item_sfx);
                //InventoryManager.inventoryManager.save_inventory();
                //PlayerManager.playerManager.save_spec();
            }
        }
    }

    public void trash_btn()
    {
        if(frame_enum != inventory_frame_enum.store_armed && frame_enum != inventory_frame_enum.store_consumption)
        {
            delete_item();
            Util_Manager.utilManager.play_clip(trash_item_sfx);
        }
    }

    public void set_canvas_information(InventoryFrame value_frame, int value_pos, Item_information value_information, RectTransform rectTransform)
    {
        ref_frame = value_frame;
        frame_enum = ref_frame.ret_frame_enum();
        item_pos = value_pos;
        ref_information = value_information;

        selected_frame.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        selected_frame_transform.sizeDelta = rectTransform.sizeDelta;
        selected_frame_transform.transform.position = rectTransform.transform.position;

        item_image.sprite = ref_information.jacket;
        item_name_text.text = ref_information.name;
        item_information_text.text = ref_information.information;
        item_image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        

        if (frame_enum == inventory_frame_enum.store_armed || frame_enum == inventory_frame_enum.store_consumption) // 상점 내 아이템을 선택
        {
            if (InventoryManager.inventoryManager.ret_inshop() == true) // 현재 상점 내에 있는 경우
            {
                buy_sell_button.gameObject.SetActive(true);
                buy_sell_button.GetComponentInChildren<TextMeshProUGUI>().text = "구매";

                if( Mathf.RoundToInt(PlayerManager.playerManager.schedule_information.gold_sale_percent * ref_information.gold) <= PlayerManager.playerManager.spec.current_gold)
                {
                    if (frame_enum == inventory_frame_enum.store_armed)
                    {
                        if (InventoryManager.inventoryManager.ret_empty_armed_pos() == -1)
                        {
                            buy_sell_button.interactable = false;
                        }
                        else
                        {
                            buy_sell_button.interactable = true;
                        }
                    }
                    else if (frame_enum == inventory_frame_enum.store_consumption)
                    {
                        if(InventoryManager.inventoryManager.ret_empty_consumption_pos()==-1)
                        {
                            buy_sell_button.interactable = false;
                        }
                        else
                        {
                            buy_sell_button.interactable = true;
                        }
                    }
                }
                else
                {
                    buy_sell_button.interactable = false;
                }

            }
            else if(InventoryManager.inventoryManager.ret_indrop()==true) // 몬스터나 이벤트를 통해 아이템이 드랍된 경우
            {
                buy_sell_button.gameObject.SetActive(true);
                buy_sell_button.GetComponentInChildren<TextMeshProUGUI>().text = "획득";

                if (frame_enum == inventory_frame_enum.store_armed)
                {
                    if (InventoryManager.inventoryManager.ret_empty_armed_pos() == -1)
                    {
                        buy_sell_button.interactable = false;
                    }
                    else
                    {
                        buy_sell_button.interactable = true;
                    }
                }
                else if (frame_enum == inventory_frame_enum.store_consumption)
                {
                    if (InventoryManager.inventoryManager.ret_empty_consumption_pos() == -1)
                    {
                        buy_sell_button.interactable = false;
                    }
                    else
                    {
                        buy_sell_button.interactable = true;
                    }
                }
            }
            else // 위 두 경우 전부 아닌경우 (이론 상 불가능하지만 만일의 경우를 대비해 제작)
            {
                buy_sell_button.gameObject.SetActive(false);
            }
            function_button.gameObject.SetActive(false);
            trash_button.gameObject.SetActive(false);

            gold_text.text = "Gold : " + (Mathf.RoundToInt(PlayerManager.playerManager.schedule_information.gold_sale_percent * ref_information.gold)).ToString();
        }
        else // 플레이어가 가진 아이템을 선택
        {
            if (InventoryManager.inventoryManager.ret_inshop() == true) // 현재 상점 내에 있으면 해당 아이템을 판매할수도 있다
            {
                buy_sell_button.gameObject.SetActive(true);
                buy_sell_button.interactable = true;
                buy_sell_button.GetComponentInChildren<TextMeshProUGUI>().text = "판매";
            }
            else // 상점 내에 없으므로 해당 아이템을 판매하는 것은 불가능하다
            {
                buy_sell_button.gameObject.SetActive(false);
            }
            function_button.gameObject.SetActive(true);
            trash_button.gameObject.SetActive(true);

            if (frame_enum==inventory_frame_enum.weapon || frame_enum==inventory_frame_enum.armor || frame_enum==inventory_frame_enum.accessory) // 현재 장착한 장비 칸인 경우
            {
                function_button.GetComponentInChildren<TextMeshProUGUI>().text = "장착해제";
                if(InventoryManager.inventoryManager.ret_empty_armed_pos()==-1)
                {
                    function_button.interactable = false;
                }
                else
                {
                    function_button.interactable = true;
                }
            }
            else if(frame_enum==inventory_frame_enum.player_armed) // 그저 소지하고 장착은 하지 않은 장비인 경우
            {
                function_button.GetComponentInChildren<TextMeshProUGUI>().text = "장착";
                function_button.interactable = true;
            }
            else if(frame_enum==inventory_frame_enum.player_consumption) // 현재 소지한 소지품인 경우
            {
                function_button.GetComponentInChildren<TextMeshProUGUI>().text = "사용";
                if (InventoryManager.inventoryManager.consumption_Items[item_pos].function_condition()==true)
                {
                    function_button.interactable = true;
                }
                else
                {
                    function_button.interactable = false;
                }
            }

            gold_text.text = "Gold : " + (Mathf.RoundToInt(PlayerManager.playerManager.schedule_information.gold_sell_percent * value_information.gold)).ToString();
        }
    }

    public void visualize_all_frame()
    {
        visualize_weapon_frame();
        visualize_armor_frame();
        visualize_accessory_frame();
        for(int i = 0;i<armed_frame.Count;i++)
        {
            visualize_armed_frame(i);
        }
        for(int i = 0;i<consumption_frame.Count;i++)
        {
            visualize_consumption_frame(i);
        }
    }

    public void visualize_weapon_frame()
    {
        if(InventoryManager.inventoryManager.player_weapon==null)
        {
            weapon_frame.set_information(null);
        }
        else
        {
            weapon_frame.set_information(InventoryManager.inventoryManager.player_weapon.information);
        }
    }

    public void visualize_armor_frame()
    {
        if (InventoryManager.inventoryManager.player_armor == null)
        {
            armor_frame.set_information(null);
        }
        else
        {
            armor_frame.set_information(InventoryManager.inventoryManager.player_armor.information);
        }
    }

    public void visualize_accessory_frame()
    {
        if (InventoryManager.inventoryManager.player_accessory == null)
        {
            accessory_frame.set_information(null);
        }
        else
        {
            accessory_frame.set_information(InventoryManager.inventoryManager.player_accessory.information);
        }
    }

    public void visualize_armed_frame(int pos)
    {
        if(InventoryManager.inventoryManager.armed_Items[pos]==null)
        {
            armed_frame[pos].set_information(null);
        }
        else
        {
            armed_frame[pos].set_information(InventoryManager.inventoryManager.armed_Items[pos].information);
        }
    }

    public void visualize_consumption_frame(int pos)
    {
        if (InventoryManager.inventoryManager.consumption_Items[pos] == null)
        {
            consumption_frame[pos].set_information(null);
        }
        else
        {
            consumption_frame[pos].set_information(InventoryManager.inventoryManager.consumption_Items[pos].information);
        }
    }

    public void false_all_btn()
    {
        buy_sell_button.gameObject.SetActive(false);
        function_button.gameObject.SetActive(false);
        trash_button.gameObject.SetActive(false);

        ref_information = null;
        item_pos = -1;

        item_image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        selected_frame.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        item_name_text.text = "";
        item_information_text.text = "";
        gold_text.text = "";
    }

    private void making_item(int value_pos)
    {
        InventoryManager.inventoryManager.making_item(ref_information.kind, value_pos, ref_information.code);

        InventoryManager.inventoryManager.save_inventory();
    }

    private void use_item()
    {
        InventoryManager.inventoryManager.use_item(frame_enum, item_pos);

        PlayerManager.playerManager.refresh_stat_canvas();
        DungeonManager.dungeonManager.save_dungeon_struct();
        PlayerManager.playerManager.save_spec();
        InventoryManager.inventoryManager.save_inventory();
    }

    private void delete_item()
    {
        InventoryManager.inventoryManager.delete_item(frame_enum, item_pos);

        InventoryManager.inventoryManager.save_inventory();
        if(frame_enum==inventory_frame_enum.weapon || frame_enum==inventory_frame_enum.armor || frame_enum==inventory_frame_enum.accessory)
        {
            PlayerManager.playerManager.refresh_stat_canvas();
            PlayerManager.playerManager.save_spec();
        }
    }

    private void equip_item()
    {
        if(ref_information==null)
        {
            Debug.Log("NULL");
        }
        InventoryManager.inventoryManager.equip_item(ref_information.kind, item_pos);

        InventoryManager.inventoryManager.save_inventory();
        PlayerManager.playerManager.refresh_stat_canvas();
        PlayerManager.playerManager.save_spec();
    }

    private void unequip_item(int value_pos)
    {
        InventoryManager.inventoryManager.unequip_item(ref_information.kind, value_pos);

        InventoryManager.inventoryManager.save_inventory();
        PlayerManager.playerManager.refresh_stat_canvas();
        PlayerManager.playerManager.save_spec();
    }
}
