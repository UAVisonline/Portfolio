using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerBattleController : MonoBehaviour {

    public float start_time,move_speed, blink_move_speed, blink_time,  recharge_time, bullet_time, health_time, original_health_time;
    private float original_blink_time, original_recharge_time, original_bullet_time;
    public float particle_time, original_particle;
    public int blink_chance, original_blink_chance, player_health;
    public bool player_cannot_move,blinking,Moving, ultimate_stop;
    public Text_manager t_manager;
    public Vector2 look, last_look;
    public Vector2 blink_dir;
    public GameObject blink_particle, player_bullet;
    public Transform tf;
    private Animator anim;
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public AudioClip blink;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        t_manager = FindObjectOfType<Text_manager>();
        original_blink_time = blink_time;
        blink_time = 0.0f;
        original_recharge_time = recharge_time;
        recharge_time = 0.0f;
        original_blink_chance = blink_chance;
        original_bullet_time = bullet_time;
        original_particle = particle_time;
        original_health_time = health_time;
        health_time = -0.10f;
	}
	
	// Update is called once per frame
	void Update () {
        if(start_time>=0.0f)
        {
            start_time -= Time.deltaTime;
            return;
        }
        if(player_health <= 0)
        {
            PlayerPrefs.SetInt("current_death", PlayerPrefs.GetInt("current_death")+1);
            PlayerPrefs.SetInt("total_death", PlayerPrefs.GetInt("total_death")+1);
            PlayerPrefs.SetInt("Death_Point", PlayerPrefs.GetInt("Death_Point")+1);
            SceneManager.LoadScene("game_over");
        }
        Moving = false;
        if(t_manager == null)
        {
            t_manager = FindObjectOfType<Text_manager>();
        }
        look = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        if(look.x - transform.position.x >= 0.0f)
        {
            last_look.x = 1.00f;
        }
        if(look.x - transform.position.x < 0.0f)
        {
            last_look.x = -1.00f;
        }
        if(look.y - transform.position.y >= 0.0f)
        {
            last_look.y = 1.00f;
        }
        if(look.y - transform.position.y < 0.00f)
        {
            last_look.y = -1.00f;
        }
        anim.SetFloat("x_dir", last_look.x);
        anim.SetFloat("y_dir", last_look.y);
        if (ultimate_stop || !t_manager.Player_moving || !t_manager.fade_moving)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (!blinking)
        {
            if (Input.GetMouseButton(0))
            {
                if (bullet_time >= original_bullet_time)
                {
                    bullet_time = 0.0f;
                    GameObject obj;
                    obj = (GameObject)Instantiate(player_bullet, transform.position, transform.rotation);
                    Vector2 bullet_dir = new Vector2(look.x - transform.position.x, look.y - transform.position.y);
                    bullet_dir.Normalize();
                    obj.GetComponent<Rigidbody2D>().AddForce(bullet_dir * 360);
                }
            }
        }
        if (bullet_time <= original_bullet_time)
        {
            bullet_time += Time.deltaTime;
        }
        if (blink_chance < original_blink_chance)
        {
            if (recharge_time <= original_recharge_time)
            {
                recharge_time += Time.deltaTime;
                if (recharge_time >= original_recharge_time)
                {
                    recharge_time = 0.0f;
                    blink_chance++;
                }
            }
        }
        if(health_time >= 0.0f)
        {
            sr.color = new Color(1.00f, 1.00f, 1.00f, 0.30f);
            health_time -= Time.deltaTime;
        }
        else// if(health_time < 0.0f)
        {
            sr.color = new Color(1.00f, 1.00f, 1.00f, 1.00f);
        }
        if (player_cannot_move)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if(!blinking)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.2f || Input.GetAxisRaw("Horizontal") < -0.2f)
            {
                if(Input.GetAxisRaw("Horizontal") > 0.2f)
                {
                    rb.velocity = new Vector2(1.00f * move_speed * Time.deltaTime, rb.velocity.y);
                }
                else if(Input.GetAxisRaw("Horizontal") < -0.2f)
                {
                    rb.velocity = new Vector2(-1.00f * move_speed * Time.deltaTime, rb.velocity.y);
                }
                Moving = true;
            }
            if(Input.GetAxisRaw("Vertical") > 0.2f || Input.GetAxisRaw("Vertical") < -0.2f)
            {
                if(Input.GetAxisRaw("Vertical") > 0.2f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 1.00f * move_speed * Time.deltaTime);
                }
                else if(Input.GetAxisRaw("Vertical") < -0.2f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -1.00f * move_speed * Time.deltaTime);
                }
                Moving = true;
            }
            if (Input.GetAxisRaw("Horizontal") < 0.2f && Input.GetAxisRaw("Horizontal") > -0.2f)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            if (Input.GetAxisRaw("Vertical") < 0.2f && Input.GetAxisRaw("Vertical") > -0.2f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
            if(Input.GetMouseButtonDown(1))
            {
                if(blink_chance>0)
                {
                    blinking = true;
                    blink_dir = look - new Vector2(transform.position.x, transform.position.y);
                    AudioSource ad_source = GetComponent<AudioSource>();
                    ad_source.PlayOneShot(blink);
                    blink_dir.Normalize();
                    blink_chance--;
                }
            }
        }
        if(blinking)
        {
            rb.velocity = new Vector2(blink_dir.x *blink_move_speed* Time.deltaTime, blink_dir.y* blink_move_speed * Time.deltaTime);
            if(blink_time< original_blink_time)
            {
                blink_time += Time.deltaTime;
                if(particle_time>= original_particle)
                {
                    Instantiate(blink_particle, transform.position, transform.rotation);
                    particle_time = 0.0f;
                }
                if(particle_time <= original_particle)
                {
                    particle_time += Time.deltaTime;
                }
            }
            if (blink_time >= original_blink_time)
            {
                blink_time = 0.0f;
                blinking = false;
            }
        }
        anim.SetBool("Moving", Moving);
        anim.SetBool("Blinking", blinking);
    }

}
