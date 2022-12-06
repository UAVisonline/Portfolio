using UnityEngine;
using System.Collections;

public class trap : MonoBehaviour {

    public float Boom_Time;
    public GameObject child_Bullet;
    public PlayerBattleController player;
    public AudioClip shoot;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerBattleController>();
	}
	
	// Update is called once per frame
	void Update () {
        Boom_Time -= Time.deltaTime;
        if(Boom_Time<=0.0f)
        {
            Boom();
        }
	}
    public void Boom()
    {
        Vector2 dir = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        for (float power = 160f; power>=55.0f; power -= 15.0f)
        {
            GameObject bullet = (GameObject)Instantiate(child_Bullet, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(dir.normalized * power);
        }
        Sound_effect sound = FindObjectOfType<Sound_effect>();
        sound.Play_audio(shoot);
        this.GetComponent<Enemy_bullet>().Destroy_this();
    }
}
