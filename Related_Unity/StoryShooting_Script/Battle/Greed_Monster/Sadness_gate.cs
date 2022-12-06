using UnityEngine;
using System.Collections;

public class Sadness_gate : MonoBehaviour {
    public GameObject particle, bullet;
    public float bullet_time,original_bullet_time;
    public int shooting;
    public Enemy enemy;
    public PlayerBattleController player;
	// Use this for initialization
	void Start () {
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<PlayerBattleController>();
        original_bullet_time = bullet_time;
    }
	
	// Update is called once per frame
	void Update () {
        if(enemy == null)
        {
            enemy = FindObjectOfType<Enemy>();
        }
        /*if(enemy.first_health <= 0)
        {
            Destroy(gameObject);
        }*/
        if (player == null)
        {
            player = FindObjectOfType<PlayerBattleController>();
        }
        if(shooting>0)
        {
            if(bullet_time >= 0.0f)
            {
                bullet_time -= Time.deltaTime;
            }
            else
            {
                StartCoroutine("shoot_bullet");
                bullet_time = original_bullet_time;
                shooting--;
            }
        }
        else
        {
            if(particle != null)
            {
                Instantiate(particle, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }

    IEnumerator shoot_bullet()
    {
        GameObject obj = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        Vector3 go_player = new Vector3(player.transform.position.x - transform.position.x,player.transform.position.y-transform.position.y, 0f);
        go_player.Normalize();
        yield return new WaitForEndOfFrame();
        obj.GetComponent<Rigidbody2D>().AddForce(go_player * 160);
    }
}
