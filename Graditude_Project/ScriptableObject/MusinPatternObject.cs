using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct note_information
{
    [SerializeField] public Note_type note_type;
    [SerializeField] public Direction_type direction_target;
    [SerializeField] public Direction_type slide_direction;
    [SerializeField] public float spawn_time;
    [SerializeField] public float speed_time;

    [SerializeField] public Move_type move_direction;

    [SerializeField] public float x_pos;
    [SerializeField] public float y_pos;
}

[CreateAssetMenu(fileName = "Music_Pattern_Object", menuName = "Scriptable Object/Music_Pattern_Object")]
public class MusinPatternObject : ScriptableObject
{
    [SerializeField] private TextAsset reference;
    [SerializeField] private MusinPatternObject reference_obj;
    [TableList] [SerializeField] private List<note_information> pattern;
    
    public List<note_information> get_informations()
    {
        return pattern;
    }

    [Button]
    public void set_temp_position()
    {
        
        for(int i =0;i<pattern.Count;i++)
        {
            if(pattern[i].note_type != Note_type.move /*&& pattern[i].x_pos==0.0f && pattern[i].y_pos==0.0f*/)
            {
                switch (pattern[i].direction_target)
                {
                    case Direction_type.left:
                        pattern[i] = change_position(pattern[i], -0.6f, 0.0f);
                        break;
                    case Direction_type.right:
                        pattern[i] = change_position(pattern[i], 0.6f, 0.0f);
                        break;
                    case Direction_type.up:
                        pattern[i] = change_position(pattern[i], 0.0f, 0.35f);
                        break;
                    case Direction_type.left_up:
                        pattern[i] = change_position(pattern[i], -0.6f, 0.35f);
                        break;
                    case Direction_type.right_up:
                        pattern[i] = change_position(pattern[i], 0.6f, 0.35f);
                        break;
                    default:
                        break;
                }
                
            }
        }
        
    }

    private note_information change_position(note_information information, float x_val, float y_val)
    {
        note_information temp = information;
        temp.x_pos = x_val;
        temp.y_pos = y_val;
        return temp;
    }

    [Button]
    public void apply_reference()
    {
        List<float> times = new List<float>();
        List<string> positions = new List<string>();
        string time = "";
        string position = "";
        bool time_check = true;
        for(int i =0;i<reference.text.Length;i++)
        {
            if(reference.text[i]=='{')
            {
                time_check = false;
            }
            else if(reference.text[i]=='}')
            {
                time_check = true;
                float f_time = float.Parse(time);
                times.Add(f_time);
                positions.Add(position);
                time = "";
                position = "";
            }
            else if(reference.text[i]!='\n')
            {
                if(time_check==true)
                {
                    time += reference.text[i];
                }
                else
                {
                    position += reference.text[i];
                }
            }
        }

        for(int i =0;i<times.Count;i++)
        {
            //Debug.Log(times[i] + " /// " + positions[i]);
        }

        make_pattern_using_reference(times,positions);
        
    }

    private void make_pattern_using_reference(List<float> time,List<string> position)
    {
        int count = time.Count;
        int pos = 0;
        for(int i =0;i<count;)
        {
            if(pos<pattern.Count)
            {
                if(time[i]>=pattern[pos].spawn_time)
                {
                    pos += 1;
                }
                else
                {
                    note_information tmp = make_note(time[i], position[i]);
                    pattern.Insert(pos, tmp);
                    i++;
                }
            }
            else
            {
                note_information tmp = make_note(time[i], position[i]);
                pattern.Add(tmp);
                i++;
            }
        }
    }

    private note_information make_note(float time, string position)
    {
        note_information temp = new note_information();
        temp.note_type = Note_type.normal;
        temp.slide_direction = Direction_type.up;
        temp.move_direction = Move_type.left;
        //temp.direction_target = Direction_type.up;
        temp.speed_time = 1.0f;
        temp.spawn_time = time;
        temp.x_pos = 0.0f;
        temp.y_pos = 0.0f;

        switch (position)
        {
            case "Move Left":
                temp.note_type = Note_type.move;
                temp.move_direction = Move_type.left;
                break;
            case "Move Right":
                temp.note_type = Note_type.move;
                temp.move_direction = Move_type.right;
                break;
            case "Left_Down":
                temp.direction_target = Direction_type.left_down;
                temp.x_pos = -0.6f;
                break;
            case "Left":
                temp.direction_target = Direction_type.left;
                temp.x_pos = -0.6f;
                break;
            case "Left_Up":
                temp.direction_target = Direction_type.left_up;
                temp.x_pos = -0.6f;
                temp.y_pos = 0.35f;
                break;
            case "Right_Down":
                temp.direction_target = Direction_type.right_down;
                temp.x_pos = 0.6f;
                break;
            case "Right":
                temp.direction_target = Direction_type.right;
                temp.x_pos = 0.6f;
                break;
            case "Right_Up":
                temp.direction_target = Direction_type.right_up;
                temp.x_pos = 0.6f;
                temp.y_pos = 0.35f;
                break;
            case "Up":
                temp.direction_target = Direction_type.up;
                temp.x_pos = 0.0f;
                temp.y_pos = 0.35f;
                break;
            case "Down":
                temp.direction_target = Direction_type.down;
                break;
            case "Middle":
                temp.x_pos = 0.0f;
                temp.y_pos = 0.0f;
                break;
            default:
                break;
        }

        return temp;
    }

    [Button]
    public void apply_obj()
    {
        if(reference_obj!=null)
        {
            pattern.Clear();
            pattern = reference_obj.get_informations();
        }
    }
}
