using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_013 : BaseAmuletScript
{
    public override void OnFunction(Amulet_timing timing)
    {
        if(timing == Amulet_timing.end_battle)
        {
            DungeonManager.dungeonManager.heal_player(0.05f, 0);
        }
    }
}
