using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack_script_sword : MonoBehaviour
{
    [SerializeField] private int break_armor, damage;
    private AudioSource audio;

    public AudioClip damaged_enemy;

    void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
             collision.gameObject.GetComponent<Enemy>().Damaged_sword(this.transform.parent.transform.localScale.x, damage, break_armor);
             Player_Controller.player_controller.Player_Sound_Play(damaged_enemy, 1.9f,0.7f);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Damaged_sword(this.transform.parent.transform.localScale.x, damage, break_armor);
            Player_Controller.player_controller.Player_Sound_Play(damaged_enemy, 1.9f,0.7f);
        }
    }
}
