using UnityEngine;
using System.Collections;

public class testing_player_controller : MonoBehaviour {

    public float Movespeed;
    private bool Moving;
    private Animator anim;
    private Rigidbody2D rb;
    private static bool player_exist;
    public Vector2 lastmove;
    //public PositionManager p_manager;
    //public move_world move_manager;
    public CameraController cam;
    public Text_manager text_manager;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (!player_exist)
        {
            player_exist = true;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        //p_manager = FindObjectOfType<PositionManager>();

        /*if (p_manager != null)
        {
            this.transform.position = new Vector3(p_manager.x_pos, p_manager.y_pos, 0);
            lastmove = new Vector2(p_manager.x_dir, p_manager.y_dir);
        }*/
        text_manager = FindObjectOfType<Text_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!text_manager.Player_moving)
        {
            return;
        }
        Moving = false;
        if (Input.GetAxisRaw("Horizontal") > 0.2f || Input.GetAxisRaw("Horizontal") < -0.2f)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.2f)
            {
                rb.velocity = new Vector2(1.0f * Movespeed * Time.deltaTime, rb.velocity.y);
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.2f)
            {
                rb.velocity = new Vector2(-1.0f * Movespeed * Time.deltaTime, rb.velocity.y);
            }
            //anim.SetFloat("x_move", Input.GetAxisRaw("Horizontal"));
            //anim.SetFloat("x_last", lastmove.x);
            Moving = true;
            lastmove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") > 0.2f || Input.GetAxisRaw("Vertical") < -0.2f)
        {
            if (Input.GetAxisRaw("Vertical") > 0.2f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 1.0f * Movespeed * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") < -0.2f)
            {
                rb.velocity = new Vector2(rb.velocity.x, -1.0f * Movespeed * Time.deltaTime);
            }
            //anim.SetFloat("y_move", Input.GetAxisRaw("Vertical"));
            //anim.SetFloat("y_last", lastmove.y);
            Moving = true;
            lastmove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
        if (Input.GetAxisRaw("Horizontal") < 0.2f && Input.GetAxisRaw("Horizontal") > -0.2f)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") < 0.2f && Input.GetAxisRaw("Vertical") > -0.2f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        anim.SetFloat("x_move", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("y_move", Input.GetAxisRaw("Vertical"));
        anim.SetFloat("x_last", lastmove.x);
        anim.SetFloat("y_last", lastmove.y);
        anim.SetBool("moving", Moving);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.tag == "move_world")
        {
            cam = FindObjectOfType<CameraController>();
            move_manager = other.gameObject.GetComponent<move_world>();
            p_manager.x_pos = move_manager.x_start;
            p_manager.y_pos = move_manager.y_start;
            p_manager.x_dir = move_manager.x_dir;
            p_manager.y_dir = move_manager.y_dir;
            cam.MAX_X = move_manager.Max_x;
            cam.MAX_Y = move_manager.Max_y;
            cam.MIN_X = move_manager.Min_x;
            cam.MIN_Y = move_manager.Min_y;
            SceneManager.LoadScene(move_manager.worldname);
            this.transform.position = new Vector2(move_manager.x_start, move_manager.y_start);
            lastmove = new Vector2(move_manager.x_dir, move_manager.y_dir);

        }*/
    }
}
