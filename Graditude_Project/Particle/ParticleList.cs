using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleList : MonoBehaviour // Particle이 실제로 저장된 오브젝트 (한 종류의 Particle만 저장)
{
    [BoxGroup("Reference")] [SerializeField] private List<ParticleObject> particleObjects;

    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private int particle_pos;

    public void particle_set(Vector3 vector) // 오브젝트 풀링을 이용해 미리 생성한 Particle을 SetActive True하자... Particle은 시간이 다 되면 알아서 False 되므로 신경쓰지말자
    {
        GameObject tmp = particleObjects[particle_pos].gameObject;
        tmp.transform.position = vector;
        particleObjects[particle_pos].ParticlePlay();

        particle_pos++;
        if(particle_pos>=particleObjects.Count)
        {
            particle_pos = 0;
        }
    }
}
