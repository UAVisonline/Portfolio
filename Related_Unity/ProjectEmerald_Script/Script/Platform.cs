using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour // Bottom에 존재하는 Platform
{
    public string name; // 체크할 PlayerPref 이름

    private bool first_on, second_on; // first_on : 첫번째 mode, second_on : 두번째 mode
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString(name) == "true")
        {
            second_on = true;
            this.GetComponent<Animator>().SetBool("Second_On", second_on); // second mode를 Animator에 대해 적용합니다
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString(name) == "true")
        {
            if (!first_on) first_on = true; 
            this.GetComponent<Animator>().SetBool("First_On", first_on); // first mode를 Animator에 대해 적용합니다
        }
    }

    // 실제 Animator에서는 first_on, second_on을 동시에 확인하여 Animation State를 이동합니다
}
