using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleObject : MonoBehaviour
{
    [BoxGroup("reference")] [SerializeField] private List<ParticleSystem> particles;
    [BoxGroup("reference")] [SerializeField] private AudioSource audio;
    
    //[BoxGroup("reference")] [SerializeField] private AudioClip clip;

    private void Awake()
    {
        audio = this.GetComponent<AudioSource>();
    }

    public void ParticlePlay() // Particle 재생 
    {
        this.gameObject.SetActive(true);
        for(int i =0;i<particles.Count;i++)
        {
            particles[i].gameObject.SetActive(true);
            particles[i].Play();
        }
        audio.Play(); 
    }
}
