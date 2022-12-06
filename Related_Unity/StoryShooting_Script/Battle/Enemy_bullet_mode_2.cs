using UnityEngine;
using System.Collections;

public class Enemy_bullet_mode_2 : MonoBehaviour {

    public bool hit, rage;
    public GameObject particle;
    public PlayerBattleController player;
    public Enemy enemy;
    // Use this for initialization
    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
        if (enemy.first_health <= 0)
        {
            rage = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            enemy = FindObjectOfType<Enemy>();
        }
        if (!rage)
        {
            if (enemy.first_health <= 0)
            {
                Destroy_this();
            }
        }
        else
        {
            if (enemy.second_health <= 0 || enemy.rage_time < 0.0f)
            {
                Destroy_this();
            }
        }
        /*if(enemy.first_health <= 0.0f)
        {
            Destroy_this();
        }*/

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "I_Wall")
        {
            Destroy_this();
        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "I_Wall")
        {
            Destroy_this();
        }
       
    }

    public void Destroy_this()
    {
        if (particle != null)
        {
            Instantiate(particle, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
