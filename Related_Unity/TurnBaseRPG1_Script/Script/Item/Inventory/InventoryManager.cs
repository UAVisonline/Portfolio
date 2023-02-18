using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _invertoryManager;

    public static InventoryManager inventoryManager
    {
        get
        {
            if(_invertoryManager==null)
            {
                _invertoryManager = FindObjectOfType<InventoryManager>();
                if(_invertoryManager==null)
                {
                    Debug.LogError("Can't Find Invertory Manager");
                }
            }
            return _invertoryManager;
        }
    }

    [SerializeField] private GameObject inventory_canvas;
    private InventoryCanvas canvas_script;

    [SerializeField] private List<GameObject> item_resources;
    public Dictionary<int, GameObject> item_dictionary = new Dictionary<int, GameObject>();
    private List<int> item_code_list = new List<int>();

    [SerializeField] private string filename_inventory;
    public Inventory inventory;

    public Weapon_Item player_weapon;
    public Armed_Item player_armor;
    public Armed_Item player_accessory;
    public List<Armed_Item> armed_Items = new List<Armed_Item>();
    public List<Consumption_Item> consumption_Items = new List<Consumption_Item>();

    private bool inshop;
    private bool indrop;
    private List<Item_information> store_informations = new List<Item_information>();

    private void Awake()
    {
        if (_invertoryManager == null)
        {
            _invertoryManager = this.GetComponent<InventoryManager>();
            DontDestroyOnLoad(this.gameObject);

            for (int i = 0; i < item_resources.Count; i++)
            {
                Item temp = item_resources[i].GetComponent<Item>();
                item_dictionary.Add(temp.information.code, item_resources[i]);
                item_code_list.Add(temp.information.code);
            }

            canvas_script = inventory_canvas.GetComponent<InventoryCanvas>();
            inshop = false;
            store_informations.Clear();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if(SaveManager.saveManager.ExistFile(filename_inventory)==true)
        {
            inventory = SaveManager.saveManager.LoadJsonFile<Inventory>(filename_inventory);
            Debug.Log("Inventory File Find");
        }
        else
        {
            init_and_save_inventory();
            Debug.Log("Not Find Inventory File");
        }

        init_inventory_gameobject();
    }

    public void set_active_inventorycanvas(bool value)
    {
        inventory_canvas.SetActive(value);
    }

    #region interaction_related_inventory (item making, using, delete, equip, unequip)

    private void init_inventory_gameobject()
    {
        if (inventory.player_weapon_code == 0)
        {
            player_weapon = null;
        }
        else
        {
            if (item_dictionary.ContainsKey(inventory.player_weapon_code) == true)
            {
                player_weapon = Instantiate(item_dictionary[inventory.player_weapon_code], this.transform).GetComponent<Weapon_Item>(); // --------
            }
            else
            {
                player_weapon = null;
            }
        }

        if (inventory.player_armor_code == 0)
        {
            player_armor = null;
        }
        else
        {
            if (item_dictionary.ContainsKey(inventory.player_armor_code) == true)
            {
                player_armor = Instantiate(item_dictionary[inventory.player_armor_code], this.transform).GetComponent<Armed_Item>();
            }
            else
            {
                player_armor = null;
            }
        }

        if (inventory.player_accessory_code == 0)
        {
            player_accessory = null;
        }
        else
        {
            if (item_dictionary.ContainsKey(inventory.player_accessory_code) == true)
            {
                player_accessory = Instantiate(item_dictionary[inventory.player_accessory_code], this.transform).GetComponent<Armed_Item>();
            }
            else
            {
                player_accessory = null;
            }
        }

        for (int i = 0; i < inventory.armed_item_list.Count; i++)
        {
            armed_Items.Add(null);
            if (inventory.armed_item_list[i] == 0)
            {
                armed_Items[i] = null;
            }
            else
            {
                if (item_dictionary.ContainsKey(inventory.armed_item_list[i]) == true)
                {
                    armed_Items[i] = Instantiate(item_dictionary[inventory.armed_item_list[i]], this.transform).GetComponent<Armed_Item>();
                }
                else
                {
                    armed_Items[i] = null;
                }
            }
        }

        for (int i = 0; i < inventory.consumption_item_list.Count; i++)
        {
            consumption_Items.Add(null);
            if (inventory.consumption_item_list[i] == 0)
            {
                consumption_Items[i] = null;
            }
            else
            {
                if (item_dictionary.ContainsKey(inventory.consumption_item_list[i]) == true)
                {
                    consumption_Items[i] = Instantiate(item_dictionary[inventory.consumption_item_list[i]], this.transform).GetComponent<Consumption_Item>();
                }
                else
                {
                    consumption_Items[i] = null;
                }
            }
        }
    }

    public void init_inventory()
    {
        if(player_weapon!=null)
        {
            player_weapon.unequip();
            Destroy(player_weapon.gameObject);
            player_weapon = null;
        }
        if(player_armor!=null)
        {
            player_armor.unequip();
            Destroy(player_armor.gameObject);
            player_armor = null;
        }
        if(player_accessory!=null)
        {
            player_accessory.unequip();
            Destroy(player_accessory.gameObject);
            player_accessory = null;
        }

        List<GameObject> temp = new List<GameObject>();
        for(int i =0;i<armed_Items.Count;i++)
        {
            if(armed_Items[i]!=null)
            {
                Destroy(armed_Items[i].gameObject);
                armed_Items[i] = null;
                //temp.Add(armed_Items[i].gameObject);
            }
        }
        for(int i =0;i<consumption_Items.Count;i++)
        {
            if(consumption_Items[i]!=null)
            {
                Destroy(consumption_Items[i].gameObject);
                consumption_Items[i] = null;
                //temp.Add(consumption_Items[i].gameObject);
            }
        }

        inventory = new Inventory(3, 5);
    }

    public void making_item(kind_of_Item kind, int pos, int code)
    {
        if(kind == kind_of_Item.Potion)
        {
            inventory.consumption_item_list[pos] = code;
            consumption_Items[pos] = Instantiate(item_dictionary[code], this.transform).GetComponent<Consumption_Item>();
            canvas_script.visualize_consumption_frame(pos);
        }
        else
        {
            inventory.armed_item_list[pos] = code;
            armed_Items[pos] = Instantiate(item_dictionary[code], this.transform).GetComponent<Armed_Item>();
            canvas_script.visualize_armed_frame(pos);
        }

        //save_inventory();
    }

    public void use_item(inventory_frame_enum frame_enum, int pos)
    {
        if(frame_enum==inventory_frame_enum.player_consumption)
        {
            consumption_Items[pos].function();

            Destroy(consumption_Items[pos].gameObject);
            consumption_Items[pos] = null;
            inventory.consumption_item_list[pos] = 0;
            canvas_script.visualize_consumption_frame(pos);
        }
    }

    public void delete_item(inventory_frame_enum frame_enum, int pos)
    {
        if(frame_enum==inventory_frame_enum.weapon)
        {
            player_weapon.unequip();
            Destroy(player_weapon.gameObject);
            player_weapon = null;
            inventory.player_weapon_code = 0;
            canvas_script.visualize_weapon_frame();
        }
        else if(frame_enum==inventory_frame_enum.armor)
        {
            player_armor.unequip();
            Destroy(player_armor.gameObject);
            player_armor = null;
            inventory.player_armor_code = 0;
            canvas_script.visualize_armor_frame();
        }
        else if(frame_enum==inventory_frame_enum.accessory)
        {
            player_accessory.unequip();
            Destroy(player_accessory.gameObject);
            player_accessory = null;
            inventory.player_accessory_code = 0;
            canvas_script.visualize_accessory_frame();
        }
        else if(frame_enum==inventory_frame_enum.player_armed)
        {
            Destroy(armed_Items[pos].gameObject);
            armed_Items[pos] = null;
            inventory.armed_item_list[pos] = 0;
            canvas_script.visualize_armed_frame(pos);
        }
        else if(frame_enum==inventory_frame_enum.player_consumption)
        {
            Destroy(consumption_Items[pos].gameObject);
            consumption_Items[pos] = null;
            inventory.consumption_item_list[pos] = 0;
            canvas_script.visualize_consumption_frame(pos);
        }
    }

    public void unequip_item(kind_of_Item kind, int pos)
    {
        if(kind == kind_of_Item.Weapon)
        {
            armed_Items[pos] = player_weapon;
            player_weapon = null;
            inventory.player_weapon_code = 0;
            canvas_script.visualize_weapon_frame();

            DungeonManager.dungeonManager.player_equip_visualize(ret_kind_of_equip_weapon());
        }
        else if(kind == kind_of_Item.Armor)
        {
            armed_Items[pos] = player_armor;
            player_armor = null;
            inventory.player_armor_code = 0;
            canvas_script.visualize_armor_frame();
        }
        else if(kind == kind_of_Item.Accessories)
        {
            armed_Items[pos] = player_accessory;
            player_accessory = null;
            inventory.player_accessory_code = 0;
            canvas_script.visualize_accessory_frame();
        }

        inventory.armed_item_list[pos] = armed_Items[pos].information.code;
        armed_Items[pos].unequip();
        canvas_script.visualize_armed_frame(pos);
    }

    public void equip_item(kind_of_Item kind,int pos)
    {
        Armed_Item temp = armed_Items[pos];
        temp.equip();

        if (kind == kind_of_Item.Weapon)
        {
            armed_Items[pos] = player_weapon;
            player_weapon = temp.GetComponent<Weapon_Item>();
            inventory.player_weapon_code = temp.information.code;
            canvas_script.visualize_weapon_frame();

            DungeonManager.dungeonManager.player_equip_visualize(ret_kind_of_equip_weapon());
        }
        else if (kind == kind_of_Item.Armor)
        {
            armed_Items[pos] = player_armor;
            player_armor = temp;
            inventory.player_armor_code = temp.information.code;
            canvas_script.visualize_armor_frame();
        }
        else if (kind == kind_of_Item.Accessories)
        {
            armed_Items[pos] = player_accessory;
            player_accessory = temp;
            inventory.player_accessory_code = temp.information.code;
            canvas_script.visualize_accessory_frame();
        }

        if(armed_Items[pos]==null)
        {
            inventory.armed_item_list[pos] = 0;
        }
        else
        {
            inventory.armed_item_list[pos] = armed_Items[pos].information.code;
            armed_Items[pos].unequip();
        }

        canvas_script.visualize_armed_frame(pos);
    }

    #endregion

    public void set_inshop(bool value)
    {
        inshop = value;
        indrop = false;
    }

    public void set_indrop(bool value)
    {
        inshop = false;
        indrop = value;
    }

    public void save_inventory()
    {
        SaveManager.saveManager.SaveJsonFile(filename_inventory, inventory);
    }

    public void init_and_save_inventory()
    {
        init_inventory();
        save_inventory();
    }

    public void init_store(List<Item_information> value)
    {
        store_informations.Clear();
        for(int i =0;i<value.Count;i++)
        {
            store_informations.Add(value[i]);
        }
    }

    #region return_function

    public int ret_item_code_list_size()
    {
        return item_code_list.Count;
    }

    public int ret_item_code_list_pos(int pos)
    {
        return item_code_list[pos];
    }

    public bool ret_inshop()
    {
        return inshop;
    }

    public bool ret_indrop()
    {
        return indrop;
    }

    public int ret_empty_armed_pos()
    {
        for (int i = 0; i < armed_Items.Count; i++)
        {
            if (armed_Items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public int ret_empty_consumption_pos()
    {
        for (int i = 0; i < consumption_Items.Count; i++)
        {
            if (consumption_Items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public int ret_store_size()
    {
        return store_informations.Count;
    }

    public Item_information ret_store_item_information(int pos)
    {
        return store_informations[pos];
    }

    public InventoryCanvas ret_canvas_script()
    {
        return canvas_script;
    }

    public kind_of_weapon ret_kind_of_equip_weapon()
    {
        if(player_weapon != null)
        {
            return player_weapon.ret_weapon_kind();
        }
        return kind_of_weapon.none;
    }

    public bool ret_equiped_had_skill(int pos)
    {
        switch(pos)
        {
            case 0:
                if(player_weapon!=null)
                {
                    return player_weapon.ret_had_skill();
                }
                break;
            case 1:
                if (player_armor != null)
                {
                    return player_armor.ret_had_skill();
                }
                break;
            case 2:
                if (player_accessory != null)
                {
                    return player_accessory.ret_had_skill();
                }
                break;
        }
        return false;
    }

    public bool ret_equiped_skill_condtion(int pos)
    {
        switch (pos)
        {
            case 0:
                if (player_weapon != null)
                {
                    return player_weapon.ret_skill_condition();
                }
                break;
            case 1:
                if (player_armor != null)
                {
                    return player_armor.ret_skill_condition();
                }
                break;
            case 2:
                if (player_accessory != null)
                {
                    return player_accessory.ret_skill_condition();
                }
                break;
        }
        return false;
    }

    public Skill ret_equiped_skill(int pos)
    {
        switch (pos)
        {
            case 0:
                if (player_weapon != null)
                {
                    return player_weapon.ret_ref_skill();
                }
                break;
            case 1:
                if (player_armor != null)
                {
                    return player_armor.ret_ref_skill();
                }
                break;
            case 2:
                if (player_accessory != null)
                {
                    return player_accessory.ret_ref_skill();
                }
                break;
        }
        return null;
    }

    public bool ret_consumption_had(int pos)
    {
        if (consumption_Items[pos] != null)
        {
            return true;
        }
        return false;
    }

    #endregion

    public bool ret_end_condition()
    {
        for(int i=0;i<consumption_Items.Count;i++)
        {
            if(consumption_Items[i]!=null)
            {
                if(consumption_Items[i].information.code==2003)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void use_skill(int pos)
    {
        switch (pos)
        {
            case 0:
                if (player_weapon != null)
                {
                    player_weapon.use_skill();
                }
                break;
            case 1:
                if (player_armor != null)
                {
                    player_armor.use_skill();
                }
                break;
            case 2:
                if (player_accessory != null)
                {
                    player_accessory.use_skill();
                }
                break;
        }
    }

    public void reset_skill()
    {
        if (player_weapon != null)
        {
            player_weapon.reset_skill();
        }

        if (player_armor != null)
        {
            player_armor.reset_skill();
        }

        if (player_accessory != null)
        {
            player_accessory.reset_skill();
        }
    }
}
