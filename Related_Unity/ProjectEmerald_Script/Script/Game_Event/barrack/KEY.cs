using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KEY : MonoBehaviour
{
    public string gate_name;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(key_event(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if(collision.gameObject.name == gate_name)
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<barricade>().open();
            this.GetComponent<Animator>().Play("KEY_USED");
        }
    }

    public void set_gate_name(string name)
    {
        gate_name = name;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.name == "barricade")
        {
            collision.gameObject.GetComponent<barricade>().open();
            this.GetComponent<Animator>().Play("KEY_USED");
        }
    }*/

    IEnumerator key_event(float time)
    {
        float up_time = 0.0f;
        Vector3 dir = new Vector3(this.transform.position.x, this.transform.position.y + 3.0f, 0.0f);
        Vector3 destination = GameObject.Find(gate_name).GetComponent<Transform>().position;
        while (up_time<0.5f)
        {
            Vector3 pos = Vector3.Lerp(this.transform.position, dir, 0.2f);
            this.transform.position = pos;
            up_time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(time);
        this.GetComponent<Rigidbody2D>().velocity = (destination - this.transform.position).normalized * 5.0f;
    }
}
