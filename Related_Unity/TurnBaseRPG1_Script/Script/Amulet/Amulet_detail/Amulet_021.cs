using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet_021 : BaseAmuletScript
{
    public override void OnAcquire()
    {
        PlayerManager.playerManager.schedule_information.gold_sale_percent -= 0.1f;
        base.OnAcquire();
    }

    public override void OnDismiss()
    {
        PlayerManager.playerManager.schedule_information.gold_sale_percent += 0.1f;
        base.OnDismiss();
    }
}
