using UnityEngine;
using System.Collections;

public class Reaper_gate_1 : MonoBehaviour {

    public GameObject reaper_bullet;
    public float start_time, shoot_time,speed;
    private bool start;
    public int shoot_num;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(start_time>=0.0f)
        {
            start_time -= Time.deltaTime;
        }
        else
        {
            if(!start)
            {
                start = true;
                StartCoroutine("bullet_gate", shoot_time);
                this.GetComponent<AudioSource>().Play();
            }
        }
	}

    IEnumerator bullet_gate(float shoot_time)
    {
        for(int i=0;i<shoot_num/2;i++)
        {
            GameObject bullet = (GameObject)Instantiate(reaper_bullet, this.transform.position, Quaternion.identity);
            Vector2 dir = new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / shoot_num), speed * Mathf.Sin(Mathf.PI * 2 * i / shoot_num));
            bullet.GetComponent<Rigidbody2D>().AddForce(dir);
            bullet.transform.Rotate(new Vector3(0f, 0f, (float)360 * i / shoot_num - 270.0f));
            GameObject reverse_bullet = (GameObject)Instantiate(reaper_bullet, this.transform.position, Quaternion.identity);
            Vector2 reverse_dir = new Vector2(-speed * Mathf.Cos(Mathf.PI * 2 * i / shoot_num), -speed * Mathf.Sin(Mathf.PI * 2 * i / shoot_num));
            reverse_bullet.GetComponent<Rigidbody2D>().AddForce(reverse_dir);
            reverse_bullet.transform.Rotate(new Vector3(0f, 0f, (float)360 * i / shoot_num - 90.0f));
            yield return new WaitForSeconds(shoot_time);
        }
        Destroy(gameObject);
    }
}
