using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following_Camera : MonoBehaviour
{
    public float Scene_min_x, Scene_max_x, Scene_min_y, Scene_max_y;
    public bool x_limit, y_limit, player_center;

    private Camera cam;
    private Transform camera_following_transform;
    private int x, y, sprite_size;
    private float camera_width, camera_height;
    // Start is called before the first frame update
    private void Awake()
    {
        x = Screen.currentResolution.width;
        y = Screen.currentResolution.height;
        sprite_size = 100; 
    }

    void Start()
    {
        cam = GetComponent<Camera>(); // 카메라를 찾습니다
        cam.orthographicSize = x / (((x / y) * 2) * sprite_size); // camera의 orthographic Size를 찾습니다
        camera_height = cam.orthographicSize * 2; 
        camera_width = camera_height * cam.aspect;
        // 거기에 대해 Camera의 width, height를 구합니다

        camera_following_transform = GameObject.Find("Player").GetComponent<Transform>();

        if (player_center) // PlayerCenter mode이면 Camera가 플레이어의 중앙에 있게 합니다
        {
            this.transform.position = camera_following_transform.position + new Vector3(0f, 0f, -10f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if(this.transform.position.x)
        float transform_x, transform_y;

        if(camera_following_transform != null)
        {
            transform_x = Vector3.Lerp(this.transform.position, camera_following_transform.position + new Vector3(0f, 5.2f, -10f), 10f*Time.deltaTime).x;
            transform_y = Vector3.Lerp(this.transform.position, camera_following_transform.position + new Vector3(0f, 5.2f, -10f), 10f * Time.deltaTime).y;
            if(player_center)
            {
                transform_y = Vector3.Lerp(this.transform.position, camera_following_transform.position + new Vector3(0f, 0f, -10f), 5f * Time.deltaTime).y;
            }
            // transform x, y를 구한다

            float clampX = Mathf.Clamp(transform_x, Scene_min_x , Scene_max_x );   
            float clampY = Mathf.Clamp(transform_y, Scene_min_y , Scene_max_y ); 
            // transform이 min, max를 넘었으면 그 안으로 보정한다

            if(!x_limit && !y_limit)
            {
                this.transform.position = new Vector3(clampX, clampY, this.transform.position.z); // 둘다 이동 함
            }
            else if(x_limit && !y_limit)
            {
                this.transform.position = new Vector3(this.transform.position.x, clampY, this.transform.position.z); // x는 이동 안함
            }
            else if (!x_limit && y_limit)
            {
                this.transform.position = new Vector3(clampX, this.transform.position.y, this.transform.position.z); // y는 이동 안함
            }
            else if(x_limit && y_limit)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z); // 둘 다 이동 안함
            }
        }    
    }
}
