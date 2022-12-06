using UnityEngine;
using System.Collections;

public class Time_Bullet_Destroy : MonoBehaviour {
    public float destroy_time;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        destroy_time -= Time.deltaTime;
        if (destroy_time <= 0.0f)
        {
            Enemy_bullet e_bullet = GetComponent<Enemy_bullet>();
            e_bullet.Destroy_this();
        }
    }
}
