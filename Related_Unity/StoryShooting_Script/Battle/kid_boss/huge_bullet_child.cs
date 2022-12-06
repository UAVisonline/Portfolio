using UnityEngine;
using System.Collections;

public class huge_bullet_child : MonoBehaviour {

    public float bullet_time;
    private float original_bullet_time;
    public GameObject child_bullet;
	// Use this for initialization
	void Start () {
        original_bullet_time = bullet_time;
	}
	
	// Update is called once per frame
	void Update () {
	if(bullet_time>0.0f)
        {
            bullet_time -= Time.deltaTime;
        }
        else
        {
            bullet_time = original_bullet_time;
            Vector2 dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            GameObject bullet = (GameObject)Instantiate(child_bullet, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * 60);
        }
	}
}
