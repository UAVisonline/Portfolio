using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private new Renderer renderer;
    public Camera camera;
    public float speed;
    private float offset;
    private float x_value;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        camera = GameObject.FindObjectOfType<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(this.transform.position.x, camera.transform.position.y);
        if(this.transform.position.x != camera.transform.position.x)
        {
            offset += Time.deltaTime * speed * (camera.transform.position.x - this.transform.position.x);
            this.transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y);
            //Debug.Log("ss");
        }
        //Debug.Log(offset);
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
