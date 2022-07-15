using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour // 적의 투사체
{
    public float Hurt_cooltime, KnockBack_Power;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground") // 땅에 충돌한 경우
        {
            this.GetComponent<Animator>().SetBool("Hit", true);
            this.transform.localScale = new Vector2(1.0f, 1.0f);
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        }
        else if (collision.tag == "Player") // 플레이어에 충돌한 경우
        {
            if(Player_Controller.player_controller.Return_Can_Hurt()) 
            {
                this.GetComponent<Animator>().SetBool("Hit", true);
                if (collision.transform.position.x > this.transform.position.x)
                {
                    Player_Controller.player_controller.Hurt(1.0f, Hurt_cooltime, KnockBack_Power);
                }
                else
                {
                    Player_Controller.player_controller.Hurt(-1.0f, Hurt_cooltime, KnockBack_Power);
                }
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                this.transform.localScale = new Vector2(1.0f, 1.0f);
            }
        }
    }
}
