using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand_bag : Enemy
{
    public GameObject key;
    public string gate_name;
    private void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public void key_intial()
    {
        if(!PlayerPrefs.HasKey(gate_name) || PlayerPrefs.GetString(gate_name) != "true")
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
}
