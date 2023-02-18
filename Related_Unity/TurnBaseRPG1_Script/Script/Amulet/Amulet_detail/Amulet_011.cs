using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_011 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.amulet_MDEF += 3;

        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.amulet_MDEF -= 3;

        base.OnDismiss();
    }
}
