using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class ParticleDirectObject : MonoBehaviour // 이거 뭔가 싶었는데 지금 보니 Direct용 Particle 처리하는 스크립트였다
{
    [SerializeField] private List<ParticleSystem> particleSystem; // 해당 스크립트의 Particle 배열

    public void play() // 배열로 가진 Particle 전부 실행
    {
        for(int i =0;i<particleSystem.Count;i++)
        {
            particleSystem[i].Play();
        }

    }

    public void stop() // 배열로 가진 Particle 전부 종료
    {
        for (int i = 0; i < particleSystem.Count; i++)
        {
            particleSystem[i].Stop();
        }
    }
}
