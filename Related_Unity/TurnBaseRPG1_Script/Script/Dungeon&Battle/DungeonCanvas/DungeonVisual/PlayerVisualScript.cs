using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualScript : MonoBehaviour
{

    [SerializeField] private Sprite none_weapon_sprite;
    [SerializeField] private Sprite sword_equip_sprite;
    [SerializeField] private Sprite dagger_equip_sprite;
    [SerializeField] private Sprite staff_equip_sprite;

    private SpriteRenderer sprite_render;

    // Start is called before the first frame update
    void Start()
    {
        sprite_render = this.GetComponent<SpriteRenderer>();

        change_visual_related_equip(InventoryManager.inventoryManager.ret_kind_of_equip_weapon());
    }

    public void change_visual_related_equip(kind_of_weapon value)
    {
        switch(value)
        {
            case kind_of_weapon.none:
                sprite_render.sprite = none_weapon_sprite;
                break;
            case kind_of_weapon.sword:
                sprite_render.sprite = sword_equip_sprite;
                break;
            case kind_of_weapon.dagger:
                sprite_render.sprite = dagger_equip_sprite;
                break;
            case kind_of_weapon.staff:
                sprite_render.sprite = staff_equip_sprite;
                break;
        }
    }
}
