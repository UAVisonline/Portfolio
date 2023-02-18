using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_048 : BaseAmuletScript
{
    public override void OnFunction(Amulet_timing timing)
    {
        if (timing == Amulet_timing.after_attack)
        {
            if(DungeonManager.dungeonManager.ret_player_attack() == attack_type.physical)
            {
                int value = DungeonManager.dungeonManager.ret_current_damage();

                value = Mathf.RoundToInt(value * 0.03f);

                if(value <= 0)
                {
                    value = Random.Range(0, 2); 
                }

                DungeonManager.dungeonManager.heal_player_fixed(value);
            }
        }
    }
}
