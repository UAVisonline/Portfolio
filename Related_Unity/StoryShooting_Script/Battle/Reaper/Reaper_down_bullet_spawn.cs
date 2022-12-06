using UnityEngine;
using System.Collections;

public class Reaper_down_bullet_spawn : MonoBehaviour {

    public float min_x, max_x, min_power, max_power;
    public AudioClip shoot;
    public GameObject bullet;

	// Use this for initialization
	void Start () {
        int num = Random.Range(-1, 1);
        switch (num)
        {
            case -1:
                min_x -= 0.1f;
                break;
            case 0:
                break;
            case 1:
                min_x += 0.1f;
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {

        while (min_x<=max_x)
        {
            GameObject obj = (GameObject)Instantiate(bullet,new Vector2(min_x,this.transform.position.y), Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f,Random.Range(min_power,max_power)*-10f));
            min_x += 0.4f;
            this.GetComponent<AudioSource>().PlayOneShot(shoot);
        }
	}
}
