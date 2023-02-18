using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_034 : BaseAmuletScript
{
    public override void OnFunction(Amulet_timing timing)
    {
        if(timing == Amulet_timing.before_attack)
        {
            if(DungeonManager.dungeonManager.ret_player_act_number() == 1)
            {
                int value = DungeonManager.dungeonManager.ret_current_damage();

                value = Mathf.RoundToInt(value * 1.0f);
                if(value <= 0)
                {
                    value = 3;
                }

                DungeonManager.dungeonManager.damage += value;
            }
        }
    }
}
