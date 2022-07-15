using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_attack_range : MonoBehaviour // 적 공격 관련 스크립트
{
    Transform tf;
    public float Hurt_cooltime, KnockBack_Power; // 플레이어 피격 쿨타임 및 플레이어 넉백 강도

    // Start is called before the first frame update
    void Start()
    {
        if(this.transform.parent!=null)
        {
            tf = transform.parent.gameObject.GetComponent<Transform>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision.transform.position.x > this.transform.position.x) // 콜리전이 플레이어보다 오른쪽에 있으면
            {
                Player_Controller.player_controller.Hurt(1.0f, Hurt_cooltime, KnockBack_Power);
            }
            else // 콜리전이 플레이어보다 왼쪽에 있으면
            {
                Player_Controller.player_controller.Hurt(-1.0f, Hurt_cooltime, KnockBack_Power);
            }
        }
    }
}
