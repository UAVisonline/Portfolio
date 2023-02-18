using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Alchemy_btn_condition : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI soul_demand_text;

    [SerializeField] private int demand_soul;
    private int final_demand;

    private void OnEnable()
    {
        if(btn==null)
        {
            btn = this.GetComponent<Button>();
        }

        if(btn!=null)
        {
            final_demand = (int)(demand_soul * PlayerManager.playerManager.schedule_information.soul_sale_percent);
            soul_demand_text.text = "Soul : " + final_demand.ToString();

            if(PlayerManager.playerManager.schedule_information.soul_gacha==true || PlayerManager.playerManager.spec.currect_soul < final_demand)
            {
                btn.interactable = false;
            }
            else
            {
                btn.interactable = true;
            }
        }
    }

    public void do_alchemy()
    {
        if(PlayerManager.playerManager.soul_cal(final_demand * -1)==true)
        {
            PlayerManager.playerManager.schedule_information.soul_gacha = true;
            PlayerManager.playerManager.save_schedule_information();
        }
    }

}
