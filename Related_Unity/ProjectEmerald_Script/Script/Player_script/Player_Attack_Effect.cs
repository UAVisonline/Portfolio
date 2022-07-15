using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack_Effect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void Animation_end()
    {
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }
}
