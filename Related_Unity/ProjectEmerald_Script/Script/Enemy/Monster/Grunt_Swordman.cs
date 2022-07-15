using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt_Swordman : Enemy
{
    private bool Can_Move;
    private bool Hurting, Attack;
    private Rigidbody2D monster_rigidbody;
    [SerializeField] private float move_speed;
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
        Can_Move = false;
        Hurting = false;
        Attack = false;
        detect_time = 0.0f;
        follow_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        RaycastHit2D[] check_ground_1 = Physics2D.RaycastAll(transform.position + new Vector3(-1.0f*this.transform.localScale.x, -0.7f), Vector2.down, 0.5f);
        RaycastHit2D[] check_ground_2 = Physics2D.RaycastAll(transform.position + new Vector3(-0.5f * this.transform.localScale.x, 0f,0f), Vector2.left * this.transform.localScale.x, 1.0f);
        Debug.DrawRay(transform.position + new Vector3(-1.0f * this.transform.localScale.x, -0.7f), Vector2.down* 0.5f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(-0.5f * this.transform.localScale.x, 0f, 0f), Vector2.left * this.transform.localScale.x * 1.0f, Color.red);

        foreach(var ground in check_ground_1)
        {
            if(ground.collider.tag == "Ground")
            {
                Can_Move = true;
                break;
            }
        }

        if(Can_Move)
        {
            foreach (var ground in check_ground_2)
            {
                if (ground.collider.tag == "Ground")
                {
                    Can_Move = false;
                }
            }
        }

        if (!detection)
        {
            RaycastHit2D[] Dection_range = Physics2D.RaycastAll(transform.position, Vector2.left * this.transform.localScale.x, 9.0f); // 인식범위
            foreach (var hit in Dection_range)
            {
                if (hit.collider.tag == "Player")
                {
                    detection = true;
                    //Debug.Log("Detect Player");
                }
            }
        }
        Debug.DrawRay(transform.position, Vector2.left * 9.0f * this.transform.localScale.x, Color.green);

        if(detection == true)
        {
            if(!Attack)
            {
                SeePlayer();
            }
            RaycastHit2D[] Attack_range = Physics2D.RaycastAll(transform.position, Vector2.left * this.transform.localScale.x, 1.5f);
            Debug.DrawRay(transform.position, Vector2.left * 1.5f * this.transform.localScale.x, Color.blue);
            foreach(var range in Attack_range)
            {
                if(range.collider.tag == "Player")
                {
                    monster_animator.SetBool("Attack", true);
                }
            }
            if (follow_time>0.0f)
            {
                if(Can_Move)
                {
                    monster_animator.SetBool("Moving", true);
                    if(!Hurting)
                    {
                        if(!monster_animator.GetBool("Attack"))
                        {
                            //Debug.Log("exceed");
                            monster_rigidbody.AddForce(new Vector2(this.transform.localScale.x * Time.deltaTime * -move_speed, 0.0f)); // Move enemy
                        }
                    }
                    Can_Move = false;
                }
                else if(!Can_Move)
                {
                    monster_animator.SetBool("Moving", false);
                }
            }
            else if(follow_time<=0.0f)
            {
                monster_animator.SetBool("Moving", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player_attack")
        {
            detect();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            detect();
        }
    }

    private void SeePlayer()
    {
        if(Player_transform.position.x > this.transform.position.x)
        {
            this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void Animation_Set_Hurt_true()
    {
        Hurting = true;
        Attack = false;
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
    }


}
