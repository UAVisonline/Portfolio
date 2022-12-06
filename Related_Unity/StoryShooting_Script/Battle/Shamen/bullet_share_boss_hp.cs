using UnityEngine;
using System.Collections;

public class bullet_share_boss_hp : MonoBehaviour {

    public Enemy_bullet e_bullet;
    public Enemy boss;
    public float health;
    public bool boss_hit;
	// Use this for initialization
	void Start () {
        e_bullet = GetComponent<Enemy_bullet>();
        boss = FindObjectOfType<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player_Bullet")
        {
            health--;
            if (health<=0.0f)
            {
                e_bullet.Destroy_this();
            }
            if(boss_hit)
            {
                if(boss.first_health>0.0f)
                {
                    boss.first_health--;
                }
                else if(boss.second_health>0.0f && boss.first_health<=0.0f)
                {
                    boss.second_health--;
                }
            }
        }
    }
}
