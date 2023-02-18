using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm_play_script : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    private void OnEnable()
    {
        Util_Manager.utilManager.bgm_play(bgm);
    }
}
