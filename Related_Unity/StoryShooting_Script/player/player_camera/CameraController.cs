using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    //x_3.0,y_1.7
    
    private GameObject player;//플레이어 오브젝트를 찾음
    public float MIN_X,MAX_X,MIN_Y,MAX_Y;//현재 룸의 크기를 여기에 대입
    private Vector3 target;//플레이어의 현재 위치
    private Vector3 last_target;//플레이어의 이전 위치
    public float movespeed;//카메라 이동속도
    public bool not_player;
    //public static bool camera_exist;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");//오브젝트를 찾음
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10.0f);//카메라의 위치를 정함
        /*if(!camera_exist)
        {
            camera_exist = true;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }
	
	// Update is called once per frame
	void Update () {
        if(player == null)//플레이어오브젝트가 null이면
        {
            player = GameObject.FindGameObjectWithTag("Player");//다시 찾음
            //Debug.Log("1");
            return;
        }
        if(not_player)
        {
            player = GameObject.FindGameObjectWithTag("Enemy");
            transform.position = Vector3.Lerp(transform.position,new Vector3(player.transform.position.x,player.transform.position.y,-10.0f),Time.deltaTime);
            //transform.position.z = -10.0f;
        }
        if(!not_player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        //밑에 있는 스크립트는 플레이어가 씬 크기의 끝으로 가도 카메라가 제대로 위치를 찾는 것을 위한 스크립트임
        if(player.transform.position.x + 3.2 <=MAX_X && player.transform.position.x - 3.2 >=MIN_X && player.transform.position.y + 1.8 <= MAX_Y && player.transform.position.y - 1.8 >= MIN_Y)
        {
            last_target = target;
            target = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = target;
        }
        if (player.transform.position.x+3.2 >= MAX_X || player.transform.position.x - 3.2 <= MIN_X)
        {
            if(player.transform.position.x + 3.2 >= MAX_X)
            {
                if (player.transform.position.y + 1.8 <= MAX_Y && player.transform.position.y - 1.8 >= MIN_Y)
                {
                    target = new Vector3(MAX_X-3.2f, player.transform.position.y, transform.position.z);
                    last_target = target;
                    transform.position = new Vector3(target.x, target.y, target.z);
                }
                else
                {
                    if (player.transform.position.y + 1.8 >= MAX_Y)
                    {
                        target = new Vector3(MAX_X - 3.2f, MAX_Y - 1.8f, transform.position.z);
                        last_target = target;
                        transform.position = new Vector3(target.x, target.y, target.z);
                    }
                    else
                    {
                        target = new Vector3(MAX_X - 3.2f, MIN_Y + 1.8f, transform.position.z);
                        last_target = target;
                        transform.position = new Vector3(target.x, target.y, target.z);
                    }
                }
            }
            else
            {
                if (player.transform.position.y + 1.8 <= MAX_Y && player.transform.position.y - 1.8 >= MIN_Y)
                {
                    target = new Vector3(MIN_X + 3.2f, player.transform.position.y, transform.position.z);
                    last_target = target;
                    transform.position = new Vector3(target.x, target.y, target.z);
                }
                else
                {
                    if (player.transform.position.y + 1.8 >= MAX_Y)
                    {
                        target = new Vector3(MAX_X - 3.2f, MAX_Y - 1.8f, transform.position.z);
                        last_target = target;
                        transform.position = new Vector3(target.x, target.y, target.z);
                    }
                    else
                    {
                        target = new Vector3(MAX_X - 3.2f, MIN_Y + 1.8f, transform.position.z);
                        last_target = target;
                        transform.position = new Vector3(target.x, target.y, target.z);
                    }
                }
            }
        }
        if(player.transform.position.y +1.8 >= MAX_Y || player.transform.position.y - 1.8 <= MIN_Y)
        {
            if(player.transform.position.y + 1.8 >= MAX_Y)
            {
                if (player.transform.position.x + 3.2 <= MAX_X && player.transform.position.x - 3.2 >= MIN_X)
                {
                    target = new Vector3(player.transform.position.x, MAX_Y-1.8f, transform.position.z);
                    last_target = target;
                    transform.position = new Vector3(target.x, target.y, target.z);
                }
                else
                {
                    if(player.transform.position.x + 3.2 >= MAX_X)
                    {
                        target = new Vector3(MAX_X - 3.2f, MAX_Y - 1.8f, transform.position.z);
                        last_target = target;
                        transform.position = new Vector3(target.x, target.y, target.z);
                    }
                    else
                    {
                        target = new Vector3(MIN_X + 3.2f, MAX_Y - 1.8f, transform.position.z);
                        last_target = target;
                        transform.position = new Vector3(target.x, target.y, target.z);
                    }
                }
            }
            else
            {
                if (player.transform.position.x + 3.2 <= MAX_X && player.transform.position.x - 3.2 >= MIN_X)
                {
                    target = new Vector3(player.transform.position.x, MIN_Y + 1.8f, transform.position.z);
                    last_target = target;
                    transform.position = new Vector3(target.x, target.y, target.z);
                }
                else
                {
                    if (player.transform.position.x + 3.2 >= MAX_X)
                    {
                        target = new Vector3(MAX_X - 3.2f, MIN_Y + 1.8f, transform.position.z);
                        last_target = target;
                        transform.position = new Vector3(target.x, target.y, target.z);
                    }
                    else
                    {
                        target = new Vector3(MIN_X + 3.2f, MIN_Y + 1.8f, transform.position.z);
                        last_target = target;
                        transform.position = new Vector3(target.x, target.y, target.z);
                    }
                }
            }
        }
    }

}
