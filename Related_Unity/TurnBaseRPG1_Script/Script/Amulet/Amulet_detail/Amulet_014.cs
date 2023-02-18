using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_014 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.spec.max_turn += 2;
        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.spec.max_turn -= 2;
        base.OnDismiss();
    }
}
