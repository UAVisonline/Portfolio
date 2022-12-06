using UnityEngine;
using System.Collections;

public class Wood_big_bullet : MonoBehaviour {


    public bool shoot;
    public float shoot_num,speed;
    public GameObject small_bullet;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if(other.tag == "Wall" || other.tag == "Player")
        {
            if(other.tag == "Player")
            {
                PlayerBattleController player = other.GetComponent<PlayerBattleController>();
                if(!player.blinking)
                {
                    float time = Random.Range(0.00f, 0.04f);
                    StartCoroutine("bullet_boonsan", time); 
                }  
            }
            else
            {
                float time = Random.Range(0.00f, 0.04f);
                StartCoroutine("bullet_boonsan", time);
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Wall" || other.tag == "Player")
        {
            if (other.tag == "Player")
            {
                PlayerBattleController player = other.GetComponent<PlayerBattleController>();
                if (!player.blinking)
                {
                    float time = Random.Range(0.00f, 0.04f);
                    StartCoroutine("bullet_boonsan", time);
                }
            }
            else
            {
                float time = Random.Range(0.00f, 0.04f);
                StartCoroutine("bullet_boonsan", time);
            }

        }
    }

    IEnumerator bullet_boonsan(float time)
    {
        yield return new WaitForSeconds(time);
        if (!shoot)
        {
            for (int i = 0; i < shoot_num; i++)
            {
                if (i % 2 == 1)
                {
                    GameObject bullet = (GameObject)Instantiate(small_bullet, transform.position, Quaternion.identity);
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / shoot_num), Mathf.Sin(Mathf.PI * 2 * i / shoot_num));
                    bullet.GetComponent<Rigidbody2D>().AddForce(dir * speed);
                }
            }
            shoot = true;
        }
    }
}
