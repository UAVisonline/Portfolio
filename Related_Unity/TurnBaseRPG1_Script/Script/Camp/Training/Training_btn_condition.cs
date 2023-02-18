using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Training_btn_condition : MonoBehaviour
{
    [SerializeField] private bool rest_bool;
    [SerializeField] private Button btn;

    private void OnEnable()
    {
        if(btn==null)
        {
            btn = this.GetComponent<Button>();
        }

        if(rest_bool)
        {
            if(PlayerManager.playerManager.spec.current_stress<=0)
            {
                btn.interactable = false;
            }
            else
            {
                btn.interactable = true;
            }
        }
        else
        {
            if(PlayerManager.playerManager.spec.current_stress>= PlayerManager.playerManager.spec.max_stress)
            {
                btn.interactable = false;
            }
            else
            {
                btn.interactable = true;
            }
        }
    }
}
