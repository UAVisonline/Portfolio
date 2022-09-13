using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ReworkNote : MonoBehaviour
{
    [BoxGroup("Reference")] [ReadOnly] [SerializeField] private Rigidbody rigidbody;
    [BoxGroup("Reference")] [ReadOnly] [SerializeField] private Vector3 target_vector; // 노트 도착 위치
    [BoxGroup("Reference")] [ReadOnly] [SerializeField] private PlaneArea plane;

    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private float speed; // Note 속도
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private Vector3 start_position; // Note 시작 위치
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private float spawn_time; // Note 생성 시간
    [BoxGroup("Variable")] [SerializeField] private Note3D_type note_type;
    [BoxGroup("Variable")] [SerializeField] private Direction_type direction_type;
    [BoxGroup("Variable")] [SerializeField] private Move_type move_dir;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(this.GetComponent<Animator>()!=null)
        {
            this.GetComponent<Animator>().Play("Play");
        }
    }

    public Note3D_type get_type()
    {
        return note_type;
    }

    public Move_type get_move_dir()
    {
        return move_dir;
    }

    public Direction_type get_direction_type()
    {
        return direction_type;
    }

    public float get_spawn_time()
    {
        return spawn_time;
    }

    public void set_start_postion(Vector3 vector)
    {
        start_position = vector;
    }

    public void set_target_transform(Vector3 vector)
    {
        target_vector = vector;
    }

    public void set_speed(float value)
    {
        speed = value;
    }

    public void set_spawn_time(float value)
    {
        spawn_time = value;
    }

    public void set_plane_area(PlaneArea value)
    {
        plane = value;
    }

    private void Note_velocity_set()
    {
        rigidbody.velocity = target_vector * speed;
    }

    private void Note_velocity_zero()
    {
        rigidbody.velocity = Vector3.zero;
    }

    public void Note_on() // Note를 SetActive True 합니다
    {
        this.gameObject.SetActive(true);
        this.transform.position = start_position;
        Note_velocity_set(); // Note의 속도 설정
    }

    public void Note_off() // Note를 SetActive False 합니다
    {
        if(plane!=null)
        {
            plane.Remove_note_in_list(this);
            plane = null;
        }
        this.transform.position = start_position;
        this.gameObject.SetActive(false);
        Note_velocity_zero(); // 속도도 0으로 설정
    }

    public void plane_off()
    {
        if(plane!=null)
        {
            plane.Remove_note_in_list(this);
            plane = null;
        }
    }

    private void make_Particle(Judgement_type value)
    {
        //Debug.Log(this.transform.position);
        if(GameManager.gamemanager.get_director_mode()!=director_mode.nothing)
        {
            if (value == Judgement_type.perfect)
            {
                ParticleManager.particlemanager.perfect_particle_on(this.transform.position);
            }
            else if (value == Judgement_type.good)
            {
                ParticleManager.particlemanager.good_particle_on(this.transform.position);
            }
        }
    }

    public void note_interaction_correct(Judgement_type value)
    {
        if(note_type!=Note3D_type.move)
        {
            make_Particle(value);
        }

        ScoreObject.ScoreObj.set_type(value, note_type);
        ScoreObject.ScoreObj.correct_note();
        ScoreObject.ScoreObj.visualize_information();
        Note_off();
    }

    public void note_interaction_uncorrect()
    {
        ScoreObject.ScoreObj.set_type(Judgement_type.miss, note_type);
        ScoreObject.ScoreObj.incorrect_note();
        ScoreObject.ScoreObj.visualize_information();
        Note_off();
    }
}
