using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum inventory_frame_enum
{
    weapon, armor, accessory, player_armed, player_consumption, store_armed, store_consumption
}

public class InventoryFrame : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InventoryCanvas canvas_script;

    [SerializeField] private inventory_frame_enum frame_enum;
    [SerializeField] private int frame_pos;

    [SerializeField] private Image image;
    [SerializeField] private Sprite null_image;
    private Item_information information;
    private RectTransform rect_transform;
    private bool acquired = false;

    [SerializeField] private AudioClip select_sfx;

    void Awake()
    {
        if(image==null)
        {
            image = this.GetComponent<Image>();
        }

        if(rect_transform == null)
        {
            rect_transform = this.GetComponent<RectTransform>();
        }

        if(canvas_script==null)
        {
            canvas_script = InventoryManager.inventoryManager.ret_canvas_script();
        }
    }

    public void set_information(Item_information value)
    {
        information = value;
        if(image==null)
        {
            image = this.GetComponent<Image>();
        }

        if(information==null)
        {
            image.sprite = null_image;
        }
        else
        {
            image.sprite = value.jacket;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(information == null || acquired==true)
        {
            return;
        }

        Util_Manager.utilManager.play_clip(select_sfx);
        canvas_script.set_canvas_information(this, frame_pos, information, rect_transform);
    }

    public inventory_frame_enum ret_frame_enum()
    {
        return frame_enum;
    }

    public void store_frame_set(Item_information value, int pos)
    {
        set_information(value);
        frame_pos = pos;

        if(value.kind==kind_of_Item.Potion)
        {
            frame_enum = inventory_frame_enum.store_consumption;
        }
        else
        {
            frame_enum = inventory_frame_enum.store_armed;
        }
    }

    public void acquire_true()
    {
        acquired = true;
        image.color = new Color(0.33f, 0.33f, 0.33f);
    }
}
