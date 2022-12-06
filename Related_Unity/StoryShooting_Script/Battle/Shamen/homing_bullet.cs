using UnityEngine;
using System.Collections;

public class homing_bullet : MonoBehaviour {

    public PlayerBattleController player;
    public Vector2 direction;
    public float Speed, activate_time;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerBattleController>();
	}
	
	// Update is called once per frame
	void Update () {
        if(activate_time>0.0f)
        {
            activate_time -= Time.deltaTime;
        }
        else
        {
            direction = new Vector2(player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y);
            this.GetComponent<Rigidbody2D>().velocity = direction.normalized * Speed * Time.deltaTime;
        }
	}
}
