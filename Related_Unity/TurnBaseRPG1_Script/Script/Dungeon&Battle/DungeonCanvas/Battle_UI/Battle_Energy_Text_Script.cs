using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Battle_Energy_Text_Script : MonoBehaviour
{
    [SerializeField] private Image frame_image;
    [SerializeField] private Image energy_image;

    [SerializeField] private TextMeshProUGUI energy_text;

    private void OnEnable()
    {
        visualize();
    }

    public void visualize()
    {
        if(DungeonManager.dungeonManager.ret_energy_status()==false)
        {
            frame_image.color = new Color(0.33f, 0.33f, 0.33f);
            energy_image.color = new Color(0.33f, 0.33f, 0.33f);
        }
        else
        {
            frame_image.color = Color.white;
            energy_image.color = Color.white;
        }

        energy_text.text = DungeonManager.dungeonManager.ret_energy_status_string();
    }
}
