using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_032 : BaseAmuletScript
{
    public override void OnFunction(Amulet_timing timing)
    {
        if (timing == Amulet_timing.before_attack)
        {
            if (InventoryManager.inventoryManager.player_weapon != null)
            {
                int value = DungeonManager.dungeonManager.ret_player_standard_damage();

                value = Mathf.RoundToInt(value * 0.05f);
                if (value <= 0)
                {
                    value = 1;
                }

                DungeonManager.dungeonManager.damage += value;
            }
        }
    }
}
