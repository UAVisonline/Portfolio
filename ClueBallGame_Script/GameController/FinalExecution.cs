using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalExecution : Execution
{
    [SerializeField] private string animation_name;

    public override void OnEnable()
    {
        base.OnEnable();
        GameManager.gamemanager.information_animation_start(animation_name); // 최종 추리의 경우에는 정보 Note를 일부 불러와야하기 때문에 이와 같은 코드가 추가 (Execution Script와 별개로 작성)
    }

}
