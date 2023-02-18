using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle_Result_script : MonoBehaviour
{
    [SerializeField] private DungeonCanvasScript dungeon_canvas_script;

    [SerializeField] private GameObject result_obj;
    [SerializeField] private GameObject drop_obj;

    private int drop_gold;
    private int drop_soul;
    [SerializeField] private StoreInit item_drop_init;

    [SerializeField] private TextMeshProUGUI gold_text;
    [SerializeField] private TextMeshProUGUI soul_text;
    [SerializeField] private GameObject item_frame;

    private void OnEnable()
    {
        result_obj.SetActive(true);
        drop_obj.SetActive(false);
    }

    public void set_drop_data(int gold, int soul, List<Item_information> drops)
    {
        drop_gold = gold;
        drop_soul = soul;
        item_drop_init.set_informations(drops);

        PlayerManager.playerManager.spec.cal_gold(drop_gold);
        PlayerManager.playerManager.spec.cal_soul(drop_soul);
        gold_text.text = "Gold : " + drop_gold.ToString();
        soul_text.text = "Soul : " + drop_soul.ToString();
        
        if(drops.Count==0)
        {
            item_frame.SetActive(false);
        }
        else
        {
            item_frame.SetActive(true);
        }

        this.gameObject.SetActive(true);
    }

    public void ok_btn_function()
    {
        if(item_drop_init.ret_item_information_size()>0)
        {
            result_obj.SetActive(false);
            drop_obj.SetActive(true);
            item_drop_init.btn();
            item_drop_init.drop_true();
            InventoryManager.inventoryManager.set_active_inventorycanvas(true);
        }
        else
        {
            result_obj.SetActive(false);
            this.gameObject.SetActive(false);
            DungeonManager.dungeonManager.room_clear_dungeon();
        }
    }

    public void back_btn_function()
    {
        drop_obj.SetActive(false);
        this.gameObject.SetActive(false);
        InventoryManager.inventoryManager.set_active_inventorycanvas(false);
        
        DungeonManager.dungeonManager.room_clear_dungeon();
    }
}
