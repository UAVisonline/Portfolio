using UnityEngine;
using System.Collections;

public class Player_Bullet : MonoBehaviour {

    public GameObject blue_particle;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
         if(other.tag == "Wall" || other.tag == "Enemy" || other.tag == "Shield" || other.tag == "Player_Wall")
        {
            if(other.tag == "Enemy")
            {
                Enemy e = other.GetComponent<Enemy>();
                if(!e.no_hit)
                {
                    if (blue_particle != null)
                    {
                        Instantiate(blue_particle, transform.position, transform.rotation);
                    }
                    
                    Destroy(gameObject);
                }
            }
            if(other.tag == "Wall")
            {
                if (blue_particle != null)
                {
                    Instantiate(blue_particle, transform.position, transform.rotation);
                }
                
                Destroy(gameObject);
            }   
            if(other.tag == "Shield")
            {
                Shield shield = other.GetComponent<Shield>();
                if(shield!=null)
                {
                    shield.health -= 1;
                    if (blue_particle != null)
                    {
                        Instantiate(blue_particle, transform.position, transform.rotation);
                    }

                    Destroy(gameObject);
                }
            }
            if (other.tag == "Player_Wall")
            {
                if (blue_particle != null)
                {
                    Instantiate(blue_particle, transform.position, transform.rotation);
                }

                Destroy(gameObject);
            }
        }
         if(other.GetComponent<bullet_share_boss_hp>() != null)
        {
            if (blue_particle != null)
            {
                Instantiate(blue_particle, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
