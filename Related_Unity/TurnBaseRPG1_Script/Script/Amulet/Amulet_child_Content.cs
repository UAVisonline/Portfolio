using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Amulet_child_Content : MonoBehaviour, IPointerClickHandler
{
    private Amulet_Scroll_View view;
    private int pos;
    private int code = -1;
    [SerializeField] private Image sprite;

    [SerializeField] private bool protected_amulet;
    [SerializeField] private AudioClip amulet_select_sfx;

    public void set_view(Amulet_Scroll_View value)
    {
        view = value;
    }

    public void set_pos(int value)
    {
        pos = value;
    }

    public void set_code(int value)
    {
        code = value;
    }

    public void set_image_alpha(float value)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);
    }

    public void set_sprite(Sprite value)
    {
        sprite.sprite = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        view.Amulet_click(code, protected_amulet);
        Util_Manager.utilManager.play_clip(amulet_select_sfx);
    }
}
