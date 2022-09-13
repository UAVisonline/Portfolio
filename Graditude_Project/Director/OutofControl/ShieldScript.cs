using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShieldScript : DirectGameObject
{
    [SerializeField] private List<ParticleDirectObject> particles;

    [SerializeField] private ParticleDirectObject shield_ready;
    [SerializeField] private ParticleDirectObject shield_attack;

    int number = 0;

    public override void function0()
    {
        if(particles.Count>0)
        {
            particles[number].play();
            number++;
            if(number>particles.Count)
            {
                number = 0;
            }

            this.GetComponent<AudioSource>().Play();
        }
    }

    public override void function1()
    {
        if(shield_ready.gameObject.activeSelf==false)
        {
            shield_ready.gameObject.SetActive(true);
            shield_ready.play();
        }
        else if(shield_ready.gameObject.activeSelf == true)
        {
            shield_ready.stop();
            shield_ready.gameObject.SetActive(false);
            shield_attack.play();
            play_animation("Play2");
        }
    }
}
