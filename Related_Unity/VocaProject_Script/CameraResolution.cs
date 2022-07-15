using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake() // 모바일 환경을 위한 Screen 해상도 설정 (그러나 원하는대로 동작하지는 않음)
    {
        Screen.SetResolution(1600, 900, true);
    }
}
