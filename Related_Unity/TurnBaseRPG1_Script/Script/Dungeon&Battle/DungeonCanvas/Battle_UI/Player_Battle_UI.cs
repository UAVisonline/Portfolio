using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Battle_UI : MonoBehaviour
{
    [SerializeField] private Image player_hp_frame;
    [SerializeField] private Image player_hp_image;
    [SerializeField] private TextMeshProUGUI player_hp_text;

    [SerializeField] private GameObject player_guard_icon;
    [SerializeField] private GameObject player_barrier_icon;

    [SerializeField] private Color hp_color_safe;
    [SerializeField] private Color hp_color_warning;
    [SerializeField] private Color hp_color_dangerous;

    public void set_player_hp_UI(int current_hp, int max_hp)
    {
        if(current_hp <= 0)
        {
            player_hp_text.text = "Dead...";
            player_hp_image.fillAmount = 0.0f;
            player_hp_image.color = hp_color_dangerous;
        }
        else
        {
            player_hp_text.text = current_hp.ToString() + "/" + max_hp.ToString();
            player_hp_image.fillAmount = (float)current_hp / (float)max_hp;

            if(player_hp_image.fillAmount > 0.66f)
            {
                player_hp_image.color = hp_color_safe;
            }
            else if(player_hp_image.fillAmount < 0.33f)
            {
                player_hp_image.color = hp_color_dangerous;
            }
            else
            {
                player_hp_image.color = hp_color_warning;
            }
        }
    }

    public void visualize_player_barrier_icon(bool value)
    {
        if(value==true)
        {
            player_barrier_icon.SetActive(true);
        }
        else
        {
            player_barrier_icon.SetActive(false);
        }
    }

    public void visualize_player_guard_icon(bool value)
    {
        if (value == true)
        {
            player_guard_icon.SetActive(true);
        }
        else
        {
            player_guard_icon.SetActive(false);
        }
    }
}
