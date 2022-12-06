using UnityEngine;
using System.Collections;

public class Child_bullet : MonoBehaviour {

    public PlayerBattleController player;
    public Vector3 shoot_direction;
    public float shoot_time;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerBattleController>();
        
        StartCoroutine("shoot_player", shoot_time);
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    IEnumerator shoot_player(float time)
    {
        yield return new WaitForSeconds(time);
        shoot_direction = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, 0.0f);
        shoot_direction.Normalize();
        this.GetComponent<Rigidbody2D>().AddForce(shoot_direction * 80);
        this.GetComponent<AudioSource>().Play();
    }
}
