using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationExecution : Execution // 정보GameObject에 대하여
{
    public override void OnEnable()
    {
        base.OnEnable(); // 대사 설정 및 버튼 Command 할당 (뒤로가기 Button 때문에)
        GameManager.gamemanager.set_visualize(true); // 정보 Note 시각화
    }

    private void OnDisable()
    {
        //GameManager.gamemanager.set_visualize(false);
    }
}
