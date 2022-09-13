using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class ScoreObject : MonoBehaviour
{
    private static ScoreObject _ScoreObject;

    public static ScoreObject ScoreObj
    {
        get
        {
            if(_ScoreObject==null)
            {
                _ScoreObject = FindObjectOfType<ScoreObject>();
                if(_ScoreObject==null)
                {
                    Debug.LogError("There is no ScoreObject");
                }
            }

            return _ScoreObject;
        }
    }

    [TabGroup("Judge Color")] [SerializeField] private Color perfect_color;
    [TabGroup("Judge Color")] [SerializeField] private Color good_color;
    [TabGroup("Judge Color")] [SerializeField] private Color okay_color;
    [TabGroup("Judge Color")] [SerializeField] private Color bad_color;
    [TabGroup("Judge Color")] [SerializeField] private Color miss_color;

    [BoxGroup("Reference")] [SerializeField] private TextMeshProUGUI judge_text;
    [BoxGroup("Reference")] [SerializeField] private TextMeshProUGUI combo_text;
    [BoxGroup("Reference")] [SerializeField] private Animator animator;

    [BoxGroup("Variable")] [SerializeField] private Judgement_type type;
    [BoxGroup("Variable")] [SerializeField] private int combo;
    [BoxGroup("Variable")] [SerializeField] private string state_name;

    [BoxGroup("Combo Variable")] [ReadOnly] [SerializeField] private int total_combo;
    [BoxGroup("Combo Variable")] [ReadOnly] [SerializeField] private int highest_combo;
    [BoxGroup("Combo Variable")] [ReadOnly] [SerializeField] private int perfect_number;
    [BoxGroup("Combo Variable")] [ReadOnly] [SerializeField] private int good_number;
    [BoxGroup("Combo Variable")] [ReadOnly] [SerializeField] private int miss_number;

    [SerializeField] private int note_number;
    [SerializeField] private int note_acr;
    [SerializeField] private float accruacy;

    private int normal_note_correct;
    private int normal_note_uncorrect;
    private int normal_note_good_correct;
    private int slide_note_correct;
    private int slide_note_uncorrect;
    private int slide_note_good_correct;
    private int move_note_correct;
    private int move_note_uncorrect;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        note_number = 0;
        note_acr = 0;
    }

    #region get_function
    public int get_combo()
    {
        return combo;
    }

    public int get_total_combo()
    {
        return total_combo;
    }

    public int get_highest_combo()
    {
        return highest_combo;
    }

    public int get_perfect_number()
    {
        return perfect_number;
    }

    public int get_good_number()
    {
        return good_number;
    }

    public int get_miss_number()
    {
        return miss_number;
    }

    public float get_accruacy()
    {
        return accruacy;
    }

    public int get_normal_correct()
    {
        return normal_note_correct;
    }

    public int get_normal_good()
    {
        return normal_note_good_correct;
    }

    public int get_normal_uncorrect()
    {
        return normal_note_uncorrect;
    }

    public int get_slide_correct()
    {
        return slide_note_correct;
    }

    public int get_slide_good()
    {
        return slide_note_good_correct;
    }

    public int get_slide_uncorrect()
    {
        return slide_note_uncorrect;
    }

    public int get_move_correct()
    {
        return move_note_correct;
    }

    public int get_move_uncorrect()
    {
        return move_note_uncorrect;
    }
    #endregion

    public void set_type(Judgement_type value,Note3D_type note_type)
    {
        type = value;
        switch(type)
        {
            case Judgement_type.perfect:
                judge_text.text = "PERFECT";
                judge_text.color = perfect_color;
                perfect_number += 1;
                if(note_type==Note3D_type.normal)
                {
                    normal_note_correct += 1;
                }
                else if(note_type==Note3D_type.slide)
                {
                    slide_note_correct += 1;
                }
                else if(note_type==Note3D_type.move)
                {
                    move_note_correct += 1;
                }
                break;
            case Judgement_type.good:
                judge_text.text = "GOOD";
                judge_text.color = good_color;
                good_number += 1;
                if (note_type == Note3D_type.normal)
                {
                    normal_note_good_correct += 1;
                }
                else if (note_type == Note3D_type.slide)
                {
                    slide_note_good_correct += 1;
                }
                break;
            case Judgement_type.okay:
                judge_text.text = "OKAY";
                judge_text.color = okay_color;
                break;
            case Judgement_type.bad:
                judge_text.text = "BAD";
                judge_text.color = bad_color;
                break;
            case Judgement_type.miss:
                judge_text.text = "MISS";
                judge_text.color = miss_color;
                miss_number += 1;
                if (note_type == Note3D_type.normal)
                {
                    normal_note_uncorrect += 1;
                }
                else if (note_type == Note3D_type.slide)
                {
                    slide_note_uncorrect += 1;
                }
                else if (note_type == Note3D_type.move)
                {
                    move_note_uncorrect += 1;
                }
                break;
        }
        
    }

    private void set_combo(int value)
    {
        combo = value;
        if(highest_combo < combo)
        {
            highest_combo = combo;
        }

        if(combo>=2)
        {
            combo_text.text = combo.ToString() + " Combo";
        }
        else
        {
            combo_text.text = "0 Combo";
        }

        if(note_number!=0)
        {
            accruacy = (note_acr / note_number);
        }

    }

    public void set_total_combo(int value)
    {
        total_combo = value;
        init_score();
    }

    public void visualize_information()
    {
        animator.Play(state_name, -1, 0.0f);
    }

    public void correct_note()
    {
        if (type == Judgement_type.perfect)
        {
            note_acr += 100;
        }
        else if(type == Judgement_type.good)
        {
            note_acr += 50;
        }
        note_number += 1;

        set_combo(combo + 1);
    }

    public void incorrect_note()
    {
        note_number += 1;
        set_combo(0);
    }

    public void init_score()
    {
        set_combo(0);

        highest_combo = 0;
        perfect_number = 0;
        good_number = 0;
        miss_number = 0;

        normal_note_correct = 0;
        normal_note_uncorrect = 0;
        normal_note_good_correct = 0;
        slide_note_correct = 0;
        slide_note_uncorrect = 0;
        slide_note_good_correct = 0;
        move_note_correct = 0;
        move_note_uncorrect = 0;
    }

}
