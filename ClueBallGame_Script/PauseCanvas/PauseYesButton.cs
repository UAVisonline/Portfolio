using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseYesButton : SetActiveButton
{
    [SerializeField] private GameSet gameSet;

    public override void execute()
    {
        GameManager.gamemanager.set_pause(false); // pause 상태 종료
        GameManager.gamemanager.click_play();
        GameManager.gamemanager.background_music_slow_stop(); // bgm을 자연스럽게 종료

        gameSet.Game_set(); // 게임 정보 초기화 후 메인화면으로 이동
        base.execute(); // PauseCanvas False
    }
}
