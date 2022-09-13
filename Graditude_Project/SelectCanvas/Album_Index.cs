using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Album_Index : MonoBehaviour, IPointerClickHandler
{
    [BoxGroup("UI member")] [SerializeField] private Sprite album_sprite;

    [BoxGroup("variable")] [ReadOnly] [SerializeField] private bool centered;
    [BoxGroup("variable")] [ReadOnly] [SerializeField] private bool righted;
    [BoxGroup("variable")] [ReadOnly] [SerializeField] private bool lefted;

    [BoxGroup("variable")] [SerializeField] private int list_index;
    [BoxGroup("variable")] [SerializeField] private int music_index;

    [BoxGroup("Reference")] [SerializeField] private Album_Select parent_album_select;
    [BoxGroup("Reference")] [SerializeField] private Album_Index left_index_obj;
    [BoxGroup("Reference")] [SerializeField] private Album_Index right_index_obj;

    [BoxGroup("Reference")] [SerializeField] private RectTransform rect;

    private void Awake()
    {
        if(parent_album_select==null)
        {
            parent_album_select = this.transform.parent.GetComponent<Album_Select>();
        }

        rect = this.GetComponent<RectTransform>();
    }

    public bool get_centered()
    {
        return centered;
    }

    public bool get_lefted()
    {
        return lefted;
    }

    public bool get_righted()
    {
        return righted;
    }

    public int get_list_index()
    {
        return list_index;
    }

    public int get_music_index()
    {
        return music_index;
    }

    public void set_centered(bool value)
    {
        centered = value;
    }

    public void set_lefted(bool value)
    {
        lefted = value;
    }

    public void set_righted(bool value)
    {
        righted = value;
    }

    public void set_music_index(int value)
    {
        music_index = value;
    }

    private void set_album_sprite(Sprite value)
    {
        album_sprite = value;
        this.GetComponent<Image>().sprite = album_sprite;
    }

    public void change_bool_position_near() // 이 이미지 및 좌우 이미지 boolean 변수 변경 (center에 있는가? left에 있는가? right에 있는가?)
    {
        set_centered(true);
        set_lefted(false);
        set_righted(false);
        big_size_rect();

        right_index_obj.set_centered(false);
        right_index_obj.set_lefted(false);
        right_index_obj.set_righted(true);
        right_index_obj.small_size_rect();

        left_index_obj.set_centered(false);
        left_index_obj.set_lefted(true);
        left_index_obj.set_righted(false);
        left_index_obj.small_size_rect();
    }

    public void change_music_index_near() // 이 이미지 및 좌우 이미지가 현재 참조하는 musiclist의 index를 반영하도록 설정
    {
        int left_index = parent_album_select.change_value_to_music_index(music_index - 1);
        int right_index = parent_album_select.change_value_to_music_index(music_index + 1);

        music_index = parent_album_select.get_music_index();
        left_index_obj.set_music_index(left_index);
        right_index_obj.set_music_index(right_index);
    }

    public void change_music_sprite_near() // 이 이미지 및 좌우 이미지가 현재 참조하는 index를 이용해 music jacket을 가져옴
    {
        Sprite sprite = parent_album_select.get_music_sprite(music_index);
        Sprite left_sprite = parent_album_select.get_music_sprite(left_index_obj.get_music_index());
        Sprite right_sprite = parent_album_select.get_music_sprite(right_index_obj.get_music_index());

        set_album_sprite(sprite);
        left_index_obj.set_album_sprite(left_sprite);
        right_index_obj.set_album_sprite(right_sprite);
    }

    public void OnPointerClick(PointerEventData eventData) // 클릭을 통한 jacket 배열 회전 구현
    {
        /*
        if(parent_album_select.get_work_bool()==true)
        {
            return;
        }


        if(lefted==true || righted==true)
        {
            if (lefted == true) // left 이미지였으면
            {
                parent_album_select.set_remain_angle(45.0f);

            }
            else if (righted == true) // right 이미지였으면
            {
                parent_album_select.set_remain_angle(-45.0f);
            }

            parent_album_select.set_current_music_index(music_index); // Album select의 music index 변경

            change_bool_position_near(); // 좌우 이미지 boolean 변경(이것이 없으면 클릭을 통한 회전이 불가)
            change_music_index_near(); // 좌우 이미지의 music index 변경
            change_music_sprite_near(); // 좌우 이미지의 music jacket 변경
        }
        */
    }

    public void big_size_rect()
    {
        rect.sizeDelta = new Vector2(200.0f, 200.0f);
    }

    public void small_size_rect()
    {
        rect.sizeDelta = new Vector2(100.0f, 100.0f);
    }
}
