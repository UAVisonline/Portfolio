using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_objects : MonoBehaviour // List 내 Particle을 Random으로 재생하는 목적으로 생성한 듯, 그러나 실제로는 사용 X
{
    [SerializeField] private List<ParticleSystem> particles;

    private void OnEnable() 
    {
        int rand = Random.Range(0, particles.Count);
        particles[rand].Play();
    }
}
