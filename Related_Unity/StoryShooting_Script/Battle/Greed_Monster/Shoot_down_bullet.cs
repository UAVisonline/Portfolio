using UnityEngine;
using System.Collections;

public class Shoot_down_bullet : MonoBehaviour {
    public GameObject big_bullet;
    public float min_x, max_x, shoot_cool, time;
    private bool shoot_sound;
    private float original_cool;
	// Use this for initialization
	void Start () {
        Enemy e = FindObjectOfType<Enemy>();
        if(e.first_health<=0)
        {
            time = e.rage_time;
        }
        original_cool = shoot_cool;
	}
	
	// Update is called once per frame
	void Update () {
        shoot_cool -= Time.deltaTime;
	if(time>=0.0f)
        {
            time -= Time.deltaTime;
        }
    else
        {
            Destroy(gameObject);
        }
    if(shoot_cool<0.0f)
        {
            if(!shoot_sound)
            {
                this.GetComponent<AudioSource>().Play();
                shoot_sound = true;
            }
            shoot_cool = original_cool;
            GameObject obj = (GameObject)Instantiate(big_bullet, new Vector3(transform.position.x + Random.Range(min_x, max_x), transform.position.y, 0.0f), transform.rotation);
            obj.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 140);
        }
	}
}
