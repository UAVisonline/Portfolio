using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSetting : MonoBehaviour
{
    public void frame_set_low()
    {
        GameManager.gamemanager.frame_set(frame_setting.low);
        this.GetComponent<AudioSource>().Play();
    }

    public void frame_set_normal()
    {
        GameManager.gamemanager.frame_set(frame_setting.medium);
        this.GetComponent<AudioSource>().Play();
    }

    public void frame_set_high()
    {
        GameManager.gamemanager.frame_set(frame_setting.high);
        this.GetComponent<AudioSource>().Play();
    }

    public void frame_set_very_high()
    {
        GameManager.gamemanager.frame_set(frame_setting.very_high);
        this.GetComponent<AudioSource>().Play();
    }
}
