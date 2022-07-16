using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect_zone : MonoBehaviour
{
    public bool detect_mode_raycast;

    private bool Collied_Player; // 탐지 Collision내 플레이어가 있는 경우
    private Enemy parent_script;

    void Start()
    {
        parent_script = this.GetComponentInParent<Enemy>();
        //rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(!detect_mode_raycast) // Enemy Script가 Raycast로 플레이어르 체크하지 못했을 경우
            {
                if (parent_script.return_detection()) // 적이 플레이어를 탐지 했을 경우
                {
                    parent_script.detect(); // 플레이어 탐지 쿨타임 재설정
                }
            }
            else // RayCast로 체크했을 경우
            {
                parent_script.detect(); //플레이어 탐지 쿨타임 재설정
            }
            Collied_Player = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Collied_Player = false;
        }
    }

    public bool Return_Collied_Player()
    {
        return Collied_Player;
    }
}
