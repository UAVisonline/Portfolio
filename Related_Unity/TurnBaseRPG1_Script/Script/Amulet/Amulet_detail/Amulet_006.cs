using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_006 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.amulet_STR += 10;
        PlayerManager.playerManager.spec.amulet_DEX += 10;
        PlayerManager.playerManager.spec.amulet_INT += 10;

        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.amulet_STR -= 10;
        PlayerManager.playerManager.spec.amulet_DEX -= 10;
        PlayerManager.playerManager.spec.amulet_INT -= 10;

        base.OnDismiss();
    }
}
