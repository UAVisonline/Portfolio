using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlaneArea : MonoBehaviour
{
    [BoxGroup("Reference")] [SerializeField] private Animator animator;
    [BoxGroup("Reference")] [SerializeField] private AudioSource audio;

    [BoxGroup("Note")] [SerializeField] private List<ReworkNote> bodynotes;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        audio = this.GetComponent<AudioSource>();
    }

    public void moving_player(int x)
    {
        Debug.Log("Moving Start : " + bodynotes.Count);
        if(bodynotes.Count>0)
        {
            if (x < 0)
            {
                check_left();
            }

            if(x>0)
            {
                check_right();
            }
        }
    }

    public void Remove_note_in_list(ReworkNote value)
    {
        bodynotes.Remove(value);
    }

    private void check_left() // 파티클 생성 하면 안되서 Body note에 대해서는 이렇게 처리함
    {
        if(bodynotes[0].get_move_dir()==Move_type.left)
        {
            bodynotes[0].note_interaction_correct(Judgement_type.perfect);
            //ScoreObject.ScoreObj.set_type(Judgement_type.perfect);
            //ScoreObject.ScoreObj.correct_note();
            //ScoreObject.ScoreObj.visualize_information();
            if(GameManager.gamemanager.get_director_mode() != director_mode.nothing)
            {
                audio.Play();
                animator.Play("Left", -1, 0.0f);
            }
        }
        else
        {
            //bodynotes[0].note_interaction_uncorrect();
            //ScoreObject.ScoreObj.set_type(Judgement_type.miss);
            //ScoreObject.ScoreObj.incorrect_note();
            //ScoreObject.ScoreObj.visualize_information();
        }

        //bodynotes[0].Note_off();
    }

    private void check_right()
    {
        if (bodynotes[0].get_move_dir() == Move_type.right)
        {
            bodynotes[0].note_interaction_correct(Judgement_type.perfect);
            //ScoreObject.ScoreObj.set_type(Judgement_type.perfect);
            //ScoreObject.ScoreObj.correct_note();
            //ScoreObject.ScoreObj.visualize_information();
            if (GameManager.gamemanager.get_director_mode() != director_mode.nothing)
            {
                audio.Play();
                animator.Play("Right", -1, 0.0f);
            }

        }
        else
        {
            //bodynotes[0].note_interaction_uncorrect();
            //ScoreObject.ScoreObj.set_type(Judgement_type.miss);
            //ScoreObject.ScoreObj.incorrect_note();
            //ScoreObject.ScoreObj.visualize_information();
        }
        //bodynotes[0].Note_off();
    }

    private void OnTriggerEnter(Collider note)
    {
        if(note.GetComponent<ReworkNote>()!=null)
        {
            if(note.GetComponent<ReworkNote>().get_type()==Note3D_type.move)
            {
                note.GetComponent<ReworkNote>().set_plane_area(this);
                bodynotes.Add(note.GetComponent<ReworkNote>());
            }
        }
    }

    private void OnTriggerExit(Collider note)
    {
        if(note.GetComponent<ReworkNote>()!=null)
        {
            if (note.GetComponent<ReworkNote>().get_type() == Note3D_type.move)
            {
                note.GetComponent<ReworkNote>().plane_off();
            }
        }
    }
}
