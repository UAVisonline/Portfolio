using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseNoButton : SetActiveButton
{
    public override void execute()
    {
        GameManager.gamemanager.set_pause(false); // pause 상태 종료
        GameManager.gamemanager.click_play();
        base.execute(); // PauseCanvas False
    }
}
