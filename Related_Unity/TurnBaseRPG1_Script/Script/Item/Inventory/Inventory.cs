using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public int player_weapon_code;
    public int player_armor_code;
    public int player_accessory_code;

    public List<int> armed_item_list = new List<int>();

    public List<int> consumption_item_list = new List<int>();

    public Inventory(int armed_size, int consumption_size)
    {
        player_weapon_code = 0;
        player_armor_code = 0;
        player_accessory_code = 0;

        for(int i =0;i<armed_size;i++)
        {
            armed_item_list.Add(0);
        }

        for(int i =0; i < consumption_size;i++)
        {
            consumption_item_list.Add(0);
        }
    }

}
