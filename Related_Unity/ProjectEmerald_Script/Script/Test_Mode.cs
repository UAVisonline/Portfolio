﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Mode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClike_PlayerPrefs_Delete()
    {
        PlayerPrefs.DeleteAll();
    }
}
