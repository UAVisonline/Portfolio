using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleManager : MonoBehaviour // Note 처리에 맞추어 파티클을 생성, 처리해주는 스크립트 <현재 3D 기반이라서 ParticleManager는 게임에서 제외 : 2D 기반으로 이 스크립트를 생성했음, 파티클 모양만 바꾸면 바로 적용가능> 
{
    private static ParticleManager _particlemanager;

    public static ParticleManager particlemanager // 싱클톤으로 쉽게 불러올 수 있다
    {
        get
        {
            if (_particlemanager == null)
            {
                _particlemanager = FindObjectOfType<ParticleManager>();
                if (_particlemanager == null)
                {
                    Debug.LogError("There is no ParticleManager Class");
                }
            }
            return _particlemanager;
        }
    }

    [BoxGroup("Reference")] [SerializeField] private ParticleList perfect; // perfect Particle 배열
    [BoxGroup("Reference")] [SerializeField] private ParticleList good; // good Particle 배열

    private void Awake() // 다른 씬을 열었는데 ParticleManager가 존재? -> 그 놈은 삭제다... 근데 여기에는 별로 필요없는 기능 같은데
    {
        if (_particlemanager == null)
        {
            _particlemanager = FindObjectOfType<ParticleManager>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void perfect_particle_on(Vector3 tmp)
    {
        perfect.particle_set(tmp);
    }

    public void good_particle_on(Vector3 tmp)
    {
        good.particle_set(tmp);
    }

    // tmp로 받은 위치에 particle을 생성하자
}
