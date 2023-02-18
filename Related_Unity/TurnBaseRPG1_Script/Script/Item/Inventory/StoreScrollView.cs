using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoreScrollView : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parent;

    [SerializeField] private List<GameObject> items = new List<GameObject>();

    private void OnEnable()
    {
        int size = InventoryManager.inventoryManager.ret_store_size();

        for(int i =0;i<size;i++)
        {
            InventoryFrame frame = Instantiate(prefab, parent).GetComponentInChildren<InventoryFrame>();
            frame.store_frame_set(InventoryManager.inventoryManager.ret_store_item_information(i), i);
            items.Add(frame.transform.parent.gameObject);
        }
    }

    private void OnDisable()
    {
        for(int i =0;i<items.Count;i++)
        {
            Destroy(items[i]);
        }

        items.Clear();
    }
}
