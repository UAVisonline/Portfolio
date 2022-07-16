using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lost_Knight : Enemy
{
    public string gate_name;
    public GameObject key;

    private bool Can_Move;
    private bool Hurting, Attack;
    private Rigidbody2D monster_rigidbody;
    private int explosion_time, bug_fixed_int;
    private float pattern_ready_time, term;//term : 공격후 움직임까지의 텀
    [SerializeField] private float move_speed;
    [SerializeField] private GameObject moon_blade, Doctor_Boom;
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
        detect_time = 0.0f;
        follow_time = 0.0f;
        pattern_ready_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (hp>0)
        {
            //Debug.Log(Hurting);
            RaycastHit2D[] Dection_range = Physics2D.RaycastAll(transform.position + new Vector3(0f, -2.0f, 0.0f), Vector2.right * this.transform.localScale.x, 12.0f); // 인식범위
            foreach (var hit in Dection_range)
            {
                if (hit.collider.tag == "Player")
                {
                    detection = true;
                    pattern_ready_time += Time.deltaTime;
                    //Debug.Log("Detect Player");
                }
            }
            Debug.DrawRay(transform.position + new Vector3(0f, -2.0f, 0.0f), Vector2.right * 12.0f * this.transform.localScale.x, Color.green);
            Debug.DrawRay(transform.position + new Vector3(0f, -1.0f, 0.0f), Vector2.right * 12.0f * this.transform.localScale.x, Color.blue);
            Debug.DrawRay(transform.position + new Vector3(0f, -1.0f, 0.0f), Vector2.left * 7.5f * this.transform.localScale.x, Color.red);
            if (detection == true)
            {
                Can_Move = true;
                if (!Attack)
                {
                    if (term > 0.0f)
                    {
                        term -= Time.deltaTime;
                    }
                    else
                    {
                        SeePlayer();
                    }
                }
                RaycastHit2D[] Attack_range = Physics2D.RaycastAll(transform.position + new Vector3(0f, -1.5f, 0.0f), Vector2.right * this.transform.localScale.x, 9.5f); // 인식범위
                RaycastHit2D[] Move_range = Physics2D.RaycastAll(transform.position + new Vector3(0f, -1.5f, 0.0f), Vector2.right * this.transform.localScale.x, 5.5f);
                foreach (var hits in Attack_range)
                {
                    if (hits.collider.tag == "Player")
                    {
                        if (!Attack)
                        {
                            if (pattern_ready_time >= 1.5f)
                            {
                                if (Player_Controller.player_controller.transform.position.x <= this.transform.position.x + 3.5f && Player_Controller.player_controller.transform.position.x >= this.transform.position.x - 3.5f)
                                {
                                    if (explosion_time == 0)
                                    {
                                        monster_animator.Play("Explosion_pattern");
                                        monster_animator.SetBool("Explosion", true);
                                        explosion_time += 2;
                                    }
                                    else
                                    {
                                        explosion_time -= 1;
                                        if (explosion_time == 0)
                                        {
                                            monster_animator.Play("Generate_pattern");
                                            monster_animator.SetBool("General", true);
                                        }
                                        if (explosion_time == 1)
                                        {
                                            monster_animator.Play("Sword_Laser_pattern");
                                            monster_animator.SetBool("Laser", true);
                                        }
                                    }

                                }
                                else
                                {
                                    int i = Random.Range(0, 3);
                                    if (i == 0)
                                    {
                                        monster_animator.Play("Sword_Laser_pattern");
                                        monster_animator.SetBool("Laser", true);
                                        
                                    }
                                    else
                                    {
                                        monster_animator.Play("Generate_pattern");
                                        monster_animator.SetBool("General", true);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var hit in Move_range)
                {
                    if (hit.collider.tag == "Ground" || hit.collider.tag == "Player")
                    {
                        Can_Move = false;
                    }
                }
                if (follow_time > 0.0f)
                {
                    if (Can_Move)
                    {
                        if (term <= 0.0f)
                        {
                            monster_animator.SetBool("Moving", true);
                            if (!Hurting)
                            {
                                if (!Attack) monster_rigidbody.AddForce(new Vector2(this.transform.localScale.x * Time.deltaTime * move_speed, 0.0f)); // Move enemy
                            }
                        }
                        else
                        {
                            monster_animator.SetBool("Moving", false);
                        }
                    }
                    else if (!Can_Move)
                    {
                        monster_animator.SetBool("Moving", false);
                    }
                }
                else if (follow_time <= 0.0f)
                {
                    monster_animator.SetBool("Moving", false);
                }
            }
        }
    }

    private void SeePlayer()
    {
        if (Player_transform.position.x > this.transform.position.x)
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    private void Animation__general_turn_and_Stop()
    {
        bool back = false;
        for(int i = 0;i<=3;i++)
        {
            RaycastHit2D[] Back_Attack_range = Physics2D.RaycastAll(transform.position + new Vector3(-0.5f, -2.1f + i*2.0f, 0.0f), Vector2.left * this.transform.localScale.x, 9.5f); // 인식범위
            foreach (var hit in Back_Attack_range)
            {
                if (hit.collider.tag == "Player")
                {
                    if (this.transform.localScale.x == -1.0f)
                    {
                        this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else if (this.transform.localScale.x == 1.0f)
                    {
                        this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                    back = true;
                }
            }
        }
        
        if(!back)
        {
            bool front = false;
            for(int i = 0;i<=3;i++)
            {
                RaycastHit2D[] Attack_range = Physics2D.RaycastAll(transform.position + new Vector3(0f, -2.1f + i*2.0f, 0.0f), Vector2.right * this.transform.localScale.x, 13.5f); // 인식범위
                foreach (var hit in Attack_range)
                {
                    if (hit.collider.tag == "Player")
                    {
                        front = true;
                    }
                } 
            }
            if (!front)
            {
                Animation_Set_Attack_end();
                monster_animator.Play("IDLE");
            }
        }  
    }

    private void Animation__laser_turn_and_Stop()
    {
        bool back = false;
        for(int i = 0;i<=3;i++)
        {
            RaycastHit2D[] Back_Attack_range = Physics2D.RaycastAll(transform.position + new Vector3(-0.5f, -2.1f + i * 2.0f, 0.0f), Vector2.left * this.transform.localScale.x, 5.0f); // 인식범위
            foreach (var hit in Back_Attack_range)
            {
                if (hit.collider.tag == "Player")
                {
                    if (this.transform.localScale.x == -1.0f)
                    {
                        this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    else if (this.transform.localScale.x == 1.0f)
                    {
                        this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                    back = true;
                }
            }
            if (back) break;
        }
        
        if (!back)
        {
            bool front = false;
            for (int i = 0;i<=3;i++)
            {
                RaycastHit2D[] Attack_range = Physics2D.RaycastAll(transform.position + new Vector3(0f, -2.1f + i * 2.0f, 0.0f), Vector2.right * this.transform.localScale.x, 8.0f); ; // 인식범위
                foreach (var hit in Attack_range)
                {
                    if (hit.collider.tag == "Player")
                    {
                        front = true;
                    }
                }
                if (front) break;
            }
            if (!front)
            {
                Animation_Set_Attack_end();
                monster_animator.Play("IDLE");
            }
        }
    }

    private void forward_dash_true()
    {
        monster_rigidbody.mass = 0.0f;
        monster_rigidbody.AddForce(new Vector2(this.transform.localScale.x * 40.0f * Time.deltaTime, 0.0f), ForceMode2D.Force);
    }

    private void dash_end()
    {
        monster_rigidbody.mass = 1.0f; 
        monster_rigidbody.velocity = new Vector3(this.transform.localScale.x * 400.0f * Time.deltaTime, 0.0f, 0.0f);
    }

    private void Animation_Set_Hurt_true()
    {
        Hurting = true;
        Attack = false;
        if(pattern_ready_time>=0.7f)
        {
            pattern_ready_time = 0.7f;
        }
        monster_animator.SetBool("Explosion", false);
        monster_animator.SetBool("Laser", false);
        monster_animator.SetBool("General", false);
        monster_rigidbody.mass = 1.0f;
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
        monster_animator.SetBool("Explosion", false);
        monster_animator.SetBool("Laser", false);
        monster_animator.SetBool("General", false);
        Attack = false;
        monster_rigidbody.mass = 1.0f;
        term = 0.3f;
        pattern_ready_time = 0.0f;
    }

    private void Boombastick()
    {
        Instantiate(Doctor_Boom, this.transform.position + new Vector3(0.0f,-0.6f,0.0f), Quaternion.identity);
        if(hp<=60)
        {
            Instantiate(Doctor_Boom, this.transform.position + new Vector3(4.5f, -0.6f, 0.0f), Quaternion.identity);
            Instantiate(Doctor_Boom, this.transform.position + new Vector3(-4.5f, -0.6f, 0.0f), Quaternion.identity);
        }
    }

    private void Animation_Moon_Blade(int _hp)
    {
        if(_hp>=hp)
        {
            GameObject arr = Instantiate(moon_blade, this.transform.position + new Vector3(this.transform.localScale.x*1.9f, -1.7f, 0.0f), Quaternion.identity);
            arr.GetComponent<Rigidbody2D>().AddForce(new Vector2(Time.deltaTime * this.transform.localScale.x * 30000.0f, 0f));
            arr.GetComponent<Transform>().localScale *= new Vector2(this.transform.localScale.x, 1.0f);
        }
    }

    public void key_intial()
    {
        if (!PlayerPrefs.HasKey(gate_name) || PlayerPrefs.GetString(gate_name) != "true")
        {
            if (key != null)
            {
                GameObject Key = Instantiate(key, this.transform.position, Quaternion.identity);
                Key.GetComponent<KEY>().set_gate_name(gate_name);
                PlayerPrefs.SetString(gate_name, "true");
            }
            //PlayerPrefs.DeleteKey("Sand_bag_key_event");
        }
    }

    private void bug_fixed()
    {
        //Debug.Log(monster_animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE"));
        if(monster_animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE"))
        {
            if (follow_time > 0.0f)
            {
                bug_fixed_int++;
                if (bug_fixed_int >= 5)
                {
                    monster_animator.Play("Explosion_pattern");
                    monster_animator.SetBool("Explosion", true);
                }
            }
        }
        else
        {
            bug_fixed_int = 0;
        }
        
        
    }
}
