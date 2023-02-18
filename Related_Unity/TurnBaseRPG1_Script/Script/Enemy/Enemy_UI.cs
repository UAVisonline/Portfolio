using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp_text;
    [SerializeField] private Image hp_bar;

    public void visualize(int current_hp, int max_hp)
    {
        if(current_hp <= 0)
        {
            hp_text.text = "Dead...";
        }
        else
        {
            hp_text.text = current_hp.ToString() + "/" + max_hp.ToString();
            float ratio = (float)current_hp / (float)max_hp;

            hp_bar.fillAmount = ratio;
            Color temp = Color.white;
            if (ratio>0.66f)
            {
                ColorUtility.TryParseHtmlString("#4DFF1E",out temp);
            }
            else if(ratio<0.33f)
            {
                ColorUtility.TryParseHtmlString("#FF0000", out temp);
            }
            else
            {
                ColorUtility.TryParseHtmlString("#FFFE1E", out temp);
            }

            hp_bar.color = temp;
        }
    }
}

