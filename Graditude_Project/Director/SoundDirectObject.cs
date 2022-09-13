using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDirectObject : DirectGameObject
{
    [SerializeField] private List<AudioClip> audioclips;

    public override void function0()
    {
        this.GetComponent<AudioSource>().PlayOneShot(audioclips[0]);
    }

    public override void function1()
    {
        this.GetComponent<AudioSource>().PlayOneShot(audioclips[1]);
    }

    public override void function2()
    {
        this.GetComponent<AudioSource>().PlayOneShot(audioclips[2]);
    }

    public override void function3()
    {
        this.GetComponent<AudioSource>().PlayOneShot(audioclips[3]);
    }

    public override void function4()
    {
        this.GetComponent<AudioSource>().PlayOneShot(audioclips[4]);
    }
}
