using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : SetActiveButton // 게임 시작 버튼
{
    public override void execute()
    {
        base.execute();
        GameManager.gamemanager.click_play();
        GameManager.gamemanager.making_answer(); // 실제 범인, 장소, 도구 설정
        GameManager.gamemanager.init_informations(); // 정보Note 초기화
    }
}
