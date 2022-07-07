using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play_Button : MonoBehaviour
{

    public void Play_Button_Click()
    {
        Problem_Base.problem.Music_Play();
    }

    public void Back_Button_Click()
    {
        Problem_Base.problem.Music_back_ten_second();
    }
}
