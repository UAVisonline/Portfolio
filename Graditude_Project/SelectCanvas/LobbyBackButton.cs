using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class LobbyBackButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    /// <summary>
    /// ���߿� ���־��ϴ� �׽�Ʈ �ڵ�
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
