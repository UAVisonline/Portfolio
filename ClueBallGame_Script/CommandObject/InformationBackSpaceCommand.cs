using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationBackSpaceCommand : BackSpace_Command // 정보 Note에서 뒤로가기 버튼
{
    public override void execute()
    {
        base.execute();
        GameManager.gamemanager.set_visualize(false); // 정보 Note visualize False때문에 따로 Script 생성
    }
}
