using UnityEngine;
using System.Collections;

public class shamen_rage : MonoBehaviour {

    public GameObject bullet;
    public Enemy enemy;
    public PlayerBattleController player;
    private bool go;
    public float speed, respawn_time,start_time;
    private float original_respawn;
	// Use this for initialization
	void Start () {
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<PlayerBattleController>();
        go = true;
        original_respawn = respawn_time;
	}
	
	// Update is called once per frame
	void Update () {
        if(start_time>=0.0f)
        {
            start_time -= Time.deltaTime;
        }
        else
        {
            if (go)
            {
                go = false;
                Vector2 dir = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;
                this.GetComponent<Rigidbody2D>().AddForce(dir * speed);
            }
            if (respawn_time > 0.0f)
            {
                respawn_time -= Time.deltaTime;
            }
            else
            {
                Instantiate(bullet, this.transform.position, Quaternion.identity);
                respawn_time = original_respawn;
            }
        }     
	if(enemy.second_health<=0.0f || enemy.rage_time<=0.0f)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Wall")
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Vector2 dir = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;
            this.GetComponent<Rigidbody2D>().AddForce(dir * speed);
            Debug.Log("aaaaa");
        }
    }
}
