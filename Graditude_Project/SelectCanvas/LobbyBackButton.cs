using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class LobbyBackButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    /// <summary>
    /// 나중에 없애야하는 테스트 코드
    /// </summary>
    [Button]
    public void Click_button()
    {
        bnt_click();
    }
    public void bnt_click()
    {
        animator.SetBool("MultiPlay", false);
        GameManager.gamemanager.set_multi_game(false);
        NetworkManager.UIControlInstance.SetReady(false);
        NetworkManager.UIControlInstance.LeaveRoom();
    }
}
