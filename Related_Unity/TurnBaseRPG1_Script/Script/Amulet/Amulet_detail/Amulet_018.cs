using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_018 : BaseAmuletScript
{
    public override void OnFunction(Amulet_timing timing)
    {
        if(timing == Amulet_timing.start_battle)
        {
            DungeonManager.dungeonManager.set_player_barrier(true);
        }
    }
}
