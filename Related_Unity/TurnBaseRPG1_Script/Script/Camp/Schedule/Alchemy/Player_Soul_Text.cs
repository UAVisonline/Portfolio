using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Soul_Text : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    void OnEnable()
    {
        if(text==null)
        {
            text = this.GetComponent<TextMeshProUGUI>();
        }

        text.text = "Soul : " + PlayerManager.playerManager.spec.currect_soul;
    }
}
