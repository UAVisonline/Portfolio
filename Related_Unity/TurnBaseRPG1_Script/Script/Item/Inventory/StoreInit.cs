using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInit : MonoBehaviour
{
    [SerializeField] private List<Item_information> informations;

    public void btn()
    {
        InventoryManager.inventoryManager.init_store(informations);
    }

    public void store_true()
    {
        InventoryManager.inventoryManager.set_inshop(true);
    }

    public void drop_true()
    {
        InventoryManager.inventoryManager.set_indrop(true);
    }

    public void set_informations(List<Item_information> value)
    {
        informations.Clear();
        for(int i=0;i<value.Count;i++)
        {
            informations.Add(value[i]);
        }
    }

    public int ret_item_information_size()
    {
        return informations.Count;
    }
}
