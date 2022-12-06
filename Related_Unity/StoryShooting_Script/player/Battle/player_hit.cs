using UnityEngine;
using System.Collections;

public class player_hit : MonoBehaviour {

    public PlayerBattleController player;
    public AudioClip hit;
	// Use this for initialization
	void Start () {
        player = GetComponentInParent<PlayerBattleController>();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(player==null)
        {
            player = GetComponentInParent<PlayerBattleController>();
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Bullet")
        {
            if (!player.blinking)
            {
                if (player.health_time < 0.0f)
                {
                    player.player_health -= 1;
                    AudioSource ad = GetComponent<AudioSource>();
                    ad.PlayOneShot(hit);
                    player.health_time = player.original_health_time;
                    Enemy_bullet e_bullet;
                    e_bullet = other.GetComponent<Enemy_bullet>();
                    if (e_bullet != null)
                    {
                        if(e_bullet.not_break_to_player != true)
                        {
                            e_bullet.Destroy_this();
                        }
                    }
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy_Bullet")
        {
            if (!player.blinking)
            {
                if (player.health_time < 0.0f)
                {
                    player.player_health -= 1;
                    player.health_time = player.original_health_time;
                    Enemy_bullet e_bullet;
                    e_bullet = other.GetComponent<Enemy_bullet>();
                    if (e_bullet != null)
                    {
                        if (e_bullet.not_break_to_player != true)
                        {
                            e_bullet.Destroy_this();
                        }
                    }
                }
            }
        }
    }
}
