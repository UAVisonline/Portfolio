using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_004 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.amulet_DEX += 15;

        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.amulet_DEX -= 15;

        base.OnDismiss();
    }
}
