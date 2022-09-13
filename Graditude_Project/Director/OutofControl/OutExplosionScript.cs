using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class OutExplosionScript : DirectGameObject
{
    [BoxGroup("Particle")] [SerializeField] List<ParticleDirectObject> particle_left;
    [BoxGroup("Particle")] [SerializeField] List<ParticleDirectObject> particle_right;

    WaitForSeconds long_wait_time = new WaitForSeconds(0.25f);
    WaitForSeconds short_wait_time = new WaitForSeconds(0.05f);

    public override void function0()
    {
        StartCoroutine("explosion_0");
    }

    public override void function1()
    {
        StartCoroutine("explosion_1");
    }

    IEnumerator explosion_0()
    {
        for(int i =0;i<particle_left.Count;i++)
        {
            particle_left[i].play();
            yield return short_wait_time;
        }

        for (int i = 0; i < particle_right.Count; i++)
        {
            particle_right[i].play();
            yield return short_wait_time;
        }

    }

    IEnumerator explosion_1()
    {
        int left = Random.Range(0, particle_left.Count);
        int left_2 = left + 3;
        if (left_2 >= particle_left.Count)
        {
            left_2 -= particle_left.Count;
        }

        int right = Random.Range(0, particle_right.Count);
        int right_2 = right + 3;
        if (right_2 >= particle_right.Count)
        {
            right_2 -= particle_right.Count;
        }

        particle_left[left].play();
        yield return long_wait_time;
        particle_right[right].play();
        yield return long_wait_time;
        particle_left[left_2].play();
        yield return long_wait_time;
        particle_right[right_2].play();
    }
}
