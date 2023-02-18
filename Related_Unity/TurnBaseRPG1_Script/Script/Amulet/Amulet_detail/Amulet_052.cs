using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_052 : BaseAmuletScript
{
    public override void OnFunction(Amulet_timing timing)
    {
        if (timing == Amulet_timing.before_attack)
        {
            int value = DungeonManager.dungeonManager.ret_player_standard_damage();
            int number = PlayerManager.playerManager.ret_total_amulet_count();

            value = Mathf.RoundToInt(value * (0.02f * number));
            if(value < number)
            {
                if(number <= 8)
                {
                    value = Mathf.RoundToInt(number * 0.5f);
                }
                else
                {
                    value = 4;
                }
            }
            // Debug.Log("UP : " + value);

            DungeonManager.dungeonManager.damage += value;
        }
    }
}
