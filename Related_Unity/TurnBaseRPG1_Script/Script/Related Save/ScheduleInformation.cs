using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScheduleInformation
{
    public bool soul_gacha;
    public List<int> random_shop = new List<int>();

    public int training_good_line;
    public int training_normal_line;

    public int training_rest_bonus;

    public float gold_sale_percent;
    public float gold_sell_percent;
    public float soul_sale_percent;

    public ScheduleInformation(int number_1, int number_2)
    {
        soul_gacha = false;
        random_shop.Clear();
        random_shop.Add(number_1);
        random_shop.Add(number_2);

        training_good_line = 33;
        training_normal_line = 66;
        training_rest_bonus = 0;

        gold_sale_percent = 1.0f;
        gold_sell_percent = 0.2f;
        soul_sale_percent = 1.0f;
    }

    public void change_random_shop(int number_1, int number_2)
    {
        random_shop.Clear();
        random_shop.Add(number_1);
        random_shop.Add(number_2);
    }
}
