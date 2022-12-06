using UnityEngine;
using System.Collections;

public class Go_to_player : MonoBehaviour {

    private PlayerBattleController player;
    public float start_time,Speed;
    private Vector2 direction;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerBattleController>();
	}
	
	// Update is called once per frame
	void Update () {
	if(start_time>0.0f)
        {
            start_time -= Time.deltaTime;
            if(start_time<=0.0f)
            {
                direction = new Vector2(player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y);
                this.GetComponent<Rigidbody2D>().AddForce(direction.normalized * Speed );
            }
        }
	}
}
