using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrainingRoomScript : MonoBehaviour
{
    [SerializeField] private TrainingWindowScript trainwindow;

    public void STR_training()
    {
        int value = 0;
        int good_line = PlayerManager.playerManager.schedule_information.training_good_line;
        int normal_line = PlayerManager.playerManager.schedule_information.training_normal_line;
        int used_stress = Random.Range(20, 26);

        if (PlayerManager.playerManager.spec.current_stress <= good_line)
        {
            value = Random.Range(4, 9);
            PlayerManager.playerManager.spec.temp_STR += value;
            PlayerManager.playerManager.spec.correction_STR += Random.Range(1, 4);
        }
        else if(PlayerManager.playerManager.spec.current_stress > good_line && PlayerManager.playerManager.spec.current_stress <= normal_line)
        {
            value = Random.Range(2, 6);
            PlayerManager.playerManager.spec.temp_STR += value;
            PlayerManager.playerManager.spec.correction_STR += Random.Range(0, 2);
        }
        else
        {
            value = Random.Range(1, 4);
            PlayerManager.playerManager.spec.temp_STR += value;
            // PlayerManager.playerManager.spec.correction_STR += Random.Range(0, 2);
        }

        PlayerManager.playerManager.spec.cal_stress(used_stress);
        PlayerManager.playerManager.next_turn();

        PlayerManager.playerManager.save_spec();

        trainwindow.gameObject.SetActive(true);
        trainwindow.STR_window(value);
    }

    public void DEX_training()
    {
        int value = 0;
        int good_line = PlayerManager.playerManager.schedule_information.training_good_line;
        int normal_line = PlayerManager.playerManager.schedule_information.training_normal_line;
        int used_stress = Random.Range(20, 26);

        if (PlayerManager.playerManager.spec.current_stress <= good_line)
        {
            value = Random.Range(4, 9);
            PlayerManager.playerManager.spec.temp_DEX += value;
            PlayerManager.playerManager.spec.correction_DEX += Random.Range(1, 4);
        }
        else if (PlayerManager.playerManager.spec.current_stress > good_line && PlayerManager.playerManager.spec.current_stress <= normal_line)
        {
            value = Random.Range(2, 6);
            PlayerManager.playerManager.spec.temp_DEX += value;
            PlayerManager.playerManager.spec.correction_DEX += Random.Range(0, 2);
        }
        else
        {
            value = Random.Range(1, 4);
            PlayerManager.playerManager.spec.temp_DEX += value;
            //PlayerManager.playerManager.spec.correction_DEX += Random.Range(0, 2);
        }

        PlayerManager.playerManager.spec.cal_stress(used_stress);
        PlayerManager.playerManager.next_turn();

        PlayerManager.playerManager.save_spec();

        trainwindow.gameObject.SetActive(true);
        trainwindow.DEX_window(value);
    }

    public void INT_training()
    {
        int value = 0;
        int good_line = PlayerManager.playerManager.schedule_information.training_good_line;
        int normal_line = PlayerManager.playerManager.schedule_information.training_normal_line;
        int used_stress = Random.Range(20, 26);

        if (PlayerManager.playerManager.spec.current_stress <= good_line)
        {
            value = Random.Range(4, 9);
            PlayerManager.playerManager.spec.temp_INT += value;
            PlayerManager.playerManager.spec.correction_INT += Random.Range(1, 4);
        }
        else if (PlayerManager.playerManager.spec.current_stress > good_line && PlayerManager.playerManager.spec.current_stress <= normal_line)
        {
            value = Random.Range(2, 6);
            PlayerManager.playerManager.spec.temp_INT += value;
            PlayerManager.playerManager.spec.correction_INT += Random.Range(0, 2);
        }
        else
        {
            value = Random.Range(1, 4);
            PlayerManager.playerManager.spec.temp_INT += value;
            //PlayerManager.playerManager.spec.correction_INT += Random.Range(0, 2);
        }

        PlayerManager.playerManager.spec.cal_stress(used_stress);
        PlayerManager.playerManager.next_turn();

        PlayerManager.playerManager.save_spec();

        trainwindow.gameObject.SetActive(true);
        trainwindow.INT_window(value);
    }

    public void atk_training()
    {
        int value = 0;
        int good_line = PlayerManager.playerManager.schedule_information.training_good_line;
        int normal_line = PlayerManager.playerManager.schedule_information.training_normal_line;
        int used_stress = Random.Range(20, 26);

        if (PlayerManager.playerManager.spec.current_stress <= good_line)
        {
            value = Random.Range(1, 5);
            PlayerManager.playerManager.spec.temp_ATK += value;
            PlayerManager.playerManager.spec.correction_ATK += Random.Range(0, 3);
        }
        else if (PlayerManager.playerManager.spec.current_stress > good_line && PlayerManager.playerManager.spec.current_stress <= normal_line)
        {
            value = Random.Range(1, 3);
            PlayerManager.playerManager.spec.temp_ATK += value;
            PlayerManager.playerManager.spec.correction_ATK += Random.Range(0, 2);
        }
        else
        {
            value = Random.Range(0, 2);
            PlayerManager.playerManager.spec.temp_ATK += value;
            // PlayerManager.playerManager.spec.correction_ATK += Random.Range(0, 1);
        }

        PlayerManager.playerManager.spec.cal_stress(used_stress);
        PlayerManager.playerManager.next_turn();

        trainwindow.gameObject.SetActive(true);
        trainwindow.ATK_window(value);
    }

    public void Rest_training()
    {
        int value = 0;
        int minus_stress = Random.Range(0, 11) + 25 + PlayerManager.playerManager.schedule_information.training_rest_bonus;

        if(PlayerManager.playerManager.spec.current_stress <= minus_stress)
        {
            value = PlayerManager.playerManager.spec.current_stress;
        }
        else
        {
            value = minus_stress;
        }
        PlayerManager.playerManager.spec.cal_stress(-1 * minus_stress);
        PlayerManager.playerManager.next_turn();

        trainwindow.gameObject.SetActive(true);
        trainwindow.Rest_window(value);
    }
}
