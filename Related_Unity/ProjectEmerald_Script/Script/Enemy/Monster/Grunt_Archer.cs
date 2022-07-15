using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt_Archer : Enemy
{
    public float arrow_power;

    private float shoot_time;
    private bool Hurting, Attack;
    private Rigidbody2D monster_rigidbody;
    [SerializeField] private GameObject arrow;
    private void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        monster_rigidbody = this.GetComponent<Rigidbody2D>();
        detection = false;
        Hurting = false;
        Attack = false;
        detect_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        bool Can_Attack = false;
        RaycastHit2D[] check_ground = Physics2D.RaycastAll(transform.position + new Vector3(0.0f, -0.7f), Vector2.down, 0.5f);
        Debug.DrawRay(transform.position + new Vector3(0.0f, -0.7f), Vector2.down * 0.5f, Color.red);
        foreach (var ground in check_ground)
        {
            if (ground.collider.tag == "Ground")
            {
                Can_Attack = true;
                break;
            }
        }

        if (detection)
        {
            if(shoot_time>0.0f)
            {
                shoot_time -= Time.deltaTime;
                SeePlayer();
            }
            else if(shoot_time<=0.0f)
            {
                if(Can_Attack)
                {
                    if(this.gameObject.GetComponentInChildren<Detect_zone>().Return_Collied_Player())
                    {
                        monster_animator.SetBool("Attack", true);
                    }
                    else
                    {
                        shoot_time = 1.2f;
                    }
                }
                else
                {
                    shoot_time = 2.0f;
                }
            }
            //RaycastHit2D[] Attack_Range = Physics2D.RaycastAll
        }
        else if(!detection)
        {
            if(shoot_time!=2.0f)
            {
                shoot_time = 2.0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player_attack")
        {
            detect();
            SeePlayer();
            //Debug.Log("Detect by player attack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            detect();
            SeePlayer();
            //Debug.Log("Detect by collide Player");
        }
    }

    private void SeePlayer()
    {
        if(monster_animator.GetCurrentAnimatorStateInfo(0).IsName("Archer_Attack") != true)
        {
            if (Player_transform.position.x > this.transform.position.x)
            {
                this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }

    private void Animation_Arrow_shoot()
    {
        GameObject arr = Instantiate(arrow, this.transform.position + new Vector3(0.0f,0.15f,0.0f), Quaternion.identity);
        arr.GetComponent<Rigidbody2D>().AddForce(new Vector2(Time.deltaTime * -this.transform.localScale.x * arrow_power,0f));
        arr.GetComponent<Transform>().localScale *= new Vector2(this.transform.localScale.x, 1.0f);
    }

    private void Animation_Set_Hurt_true()
    {
        Hurting = true;
        Attack = false;
        shoot_time = 1.5f;
        this.GetComponent<AudioSource>().Stop();
    }

    private void Animation_Set_Hurt_false()
    {
        Hurting = false;
        Attack = false;
    }

    private void Animation_Set_Attack_true()
    {
        Attack = true;
    }

    private void Animation_Set_Attack_end()
    {
        monster_animator.SetBool("Attack", false);
        Attack = false;
        shoot_time = 3.0f;
    }


}
