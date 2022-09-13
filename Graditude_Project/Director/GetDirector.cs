using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDirector : MonoBehaviour // 메인 게임 시작하면 존재하는 Obejct... GameManager는 이걸 이용해 연출용 장치를 생성한다
{
    private void Awake()
    {
        GameManager.gamemanager.making_Stage();
    }
}
