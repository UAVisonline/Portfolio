using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetCommand : ButtonCommandObject // Game 초기화 버튼 (Pause가 아닌 상태에서 게임이 끝나 초기화해야하는 경우)
{
    [SerializeField] private GameSet gameset;

    [SerializeField] private GameObject off_obejct;

    public override void execute()
    {
        gameset.Game_set();
        off_obejct.SetActive(false);
    }
}
