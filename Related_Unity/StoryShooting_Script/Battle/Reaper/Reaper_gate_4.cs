using UnityEngine;
using System.Collections;

public class Reaper_gate_4 : MonoBehaviour {

    public GameObject reaper_bullet;
    public float start_time, shoot_time, speed;
    private bool start;
    public int shoot_num;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (start_time >= 0.0f)
        {
            start_time -= Time.deltaTime;
        }
        else
        {
            if (!start)
            {
                start = true;
                StartCoroutine("bullet_gate", shoot_time);
                this.GetComponent<AudioSource>().Play();
            }
        }
    }

    IEnumerator bullet_gate(float shoot_time)
    {
        for (int i = 0; i < shoot_num ; i++)
        {
            for(int j=0;j<4;j++)
            {
                GameObject obj = (GameObject)Instantiate(reaper_bullet, transform.position, Quaternion.identity);
                obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * j / 4), speed * Mathf.Sin(Mathf.PI * 2 * j / 4)));
                obj.transform.Rotate(new Vector3(0f, 0f, 360 * j/4 - 270.0f));
            }
            yield return new WaitForSeconds(shoot_time);
        }
        Destroy(gameObject);
    }
}
