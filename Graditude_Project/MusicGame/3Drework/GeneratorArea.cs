using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

#region enum_reference
public enum Judgement_type_3D { perfect, good, miss }

public enum Note3D_type { normal, slide, move };

public enum Move_type { left, right, power_left, power_right };

public enum Note_type { normal, slide, clide, move }

public enum Direction_type { up, down, left, right, left_up, right_up, left_down, right_down }

public enum Judgement_type { perfect, good, okay, bad, miss }
#endregion

public class GeneratorArea : MonoBehaviour
{
    #region reference
    [TabGroup("Target Start List")] [SerializeField] private Transform start_up;
    [TabGroup("Target Start List")] [SerializeField] private Transform start_left;
    [TabGroup("Target Start List")] [SerializeField] private Transform start_right;
    [TabGroup("Target Start List")] [SerializeField] private Transform start_middle;
    [TabGroup("Target Start List")] [SerializeField] private Transform start_bottom;
    // Note 시작 위치 8개

    [TabGroup("related Ray vector")] [SerializeField] private Transform start_position;
    [TabGroup("related Ray vector")] [SerializeField] private Transform end_position;
    [TabGroup("related Ray vector")] [ReadOnly] [SerializeField] private float length;
    [TabGroup("related Ray vector")] [ReadOnly] [SerializeField] private Vector3 move_vector;
    // Note 관련 Vector

    [TabGroup("Note Object List")] [SerializeField] private GameObject Normal_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Slide_up_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Slide_down_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Slide_left_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Slide_right_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Slide_left_up_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Slide_right_up_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Slide_left_down_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Slide_right_down_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Move_left_Note;
    [TabGroup("Note Object List")] [SerializeField] private GameObject Move_right_Note;
    // Note 종류 9개 (Normal 1 + Slide 8) + Move 4개
    #endregion

    [BoxGroup("Note References")] [SerializeField] private List<ReworkNote> notes;
    [BoxGroup("Variable")] [ReadOnly] [SerializeField] private int notes_position; // Note 배열 내에서 현재 보낼 위치 표기 

    private void Awake()
    {
        move_vector = -1 * (start_position.position - end_position.position).normalized;
        length = (start_position.position - end_position.position).magnitude;
    }

    private void Start() // GameManager로부터 pattern object 받아서 Note 생성
    {
        // make_note_using_pattern(GameManager.gamemanager.get_pattern());
        Invoke(nameof(make_pattern), 1f);
    }

    private void make_pattern()
    {
        make_note_using_pattern(GameManager.gamemanager.get_pattern());
    }

    private float caculate_speed(float time) // 시간에 따른 Note 속도 설정
    {
        return length / time;
    }

    private Vector3 get_start_position(float x_pos, float y_pos) // Note 위치에 따른 시작 위치 설정
    {
        if(x_pos<-1.0f)
        {
            x_pos = -1.0f;
        }
        else if(x_pos>1.0f)
        {
            x_pos = 1.0f;
        }

        if(y_pos>1.0f)
        {
            y_pos = 1.0f;
        }
        else if(y_pos<0.0f)
        {
            y_pos = 0.0f;
        }

        float new_x = start_left.position.x + (start_right.position.x - start_left.position.x)* ( (x_pos + 1.0f) / 2.0f);
        float new_y = start_middle.position.y + (start_up.position.y - start_middle.position.x) * (y_pos);

        Vector3 pos = new Vector3(new_x, new_y, start_middle.position.z);

        return pos;
    }

    public void Note_send(float playback_location) // 생성된 Note, 음악 시간에 맞춰서 보내기
    {
        if (notes.Count == 0 || notes_position >= notes.Count)
        {
            return;
        }

        if (notes[notes_position].get_spawn_time() <= playback_location)
        {
            notes[notes_position].Note_on();
            notes_position++;
            Note_send(playback_location);
        }
    }

    public void restart_function() // 게임 재시작 시 Note 위치 초기화
    {
        for (int i = 0; i < notes_position; i++)
        {
            notes[i].Note_off();
        }
        notes_position = 0;
    }

    private bool make_note_using_pattern(MusinPatternObject pattern)
    {
        if (pattern == null) return false;

        for (int i = 0; i < pattern.get_informations().Count; i++) // 패턴 배열 정보를 읽음
        {
            note_information tmp = pattern.get_informations()[i]; // Scriptable obejct 정보를 읽음

            if (tmp.speed_time == 0.0f)
            {
                tmp.speed_time = 1.0f;
            }

            Note3D_type change_type = Note3D_type.normal; // Note의 타입 선언

            if (tmp.note_type == Note_type.normal)
            {
                change_type = Note3D_type.normal;
            }
            else if (tmp.note_type == Note_type.slide || tmp.note_type == Note_type.clide)
            {
                change_type = Note3D_type.slide;
            } // Note 타입 초기화
            else if (tmp.note_type == Note_type.move)
            {
                change_type = Note3D_type.move;
            }

            GameObject obj = Note_Generate(change_type, tmp.x_pos, tmp.y_pos ,tmp.spawn_time, tmp.slide_direction, tmp.speed_time, tmp.move_direction); // Scriptable object information과 Note type을 통한 Note 생성 <실제 Note 생성 함수>
            ReworkNote note = obj.GetComponent<ReworkNote>(); // Note 스크립트 가져옴
            notes.Add(note); // 배열에 집어넣음
            note.Note_off(); // 오브젝트는 Setactive False
        }

        ScoreObject.ScoreObj.set_total_combo(pattern.get_informations().Count);
        Debug.Log("Generate AREA: total count is : " + ScoreObject.ScoreObj.get_total_combo());
        GameManager.gamemanager.set_rework_generator(this); // GameManager에게 자신을 참조하도록 설정
        GameManager.gamemanager.main_game_start(); // GameManager여, 난 준비가 다 되었다. 게임시작
        return true;
    }

    private GameObject Note_Generate(Note3D_type note, float x_pos, float y_pos, float spawn_time, Direction_type slide_direction, float time = 1.0f, Move_type move_dir = Move_type.left)
    {
        Vector3 target = move_vector; // Note 이동 동선
        float speed = caculate_speed(time); // Note 시간
        spawn_time -= time; // Note의 생성 시간 (information의 생성 시간 - Note의 이동 시간)
        Vector3 start_position = get_start_position(x_pos, y_pos); // Note 생성 위치

        switch (note)
        {
            case Note3D_type.normal: // Normal Type
                GameObject normal = Instantiate(Normal_Note, start_position, Quaternion.identity, this.transform);
                if (normal.GetComponent<ReworkNote>() != null)
                {
                    normal.GetComponent<ReworkNote>().set_target_transform(target);
                    normal.GetComponent<ReworkNote>().set_speed(speed);
                    normal.GetComponent<ReworkNote>().set_start_postion(start_position);
                    normal.GetComponent<ReworkNote>().set_spawn_time(spawn_time);
                }
                return normal;

            case Note3D_type.slide: // Slide Type
                GameObject slide = null;
                switch (slide_direction)
                {
                    case Direction_type.left:
                        slide = Instantiate(Slide_left_Note, start_position, Quaternion.identity, this.transform);
                        break;
                    case Direction_type.right:
                        slide = Instantiate(Slide_right_Note, start_position, Quaternion.identity, this.transform);
                        break;
                    case Direction_type.up:
                        slide = Instantiate(Slide_up_Note, start_position, Quaternion.identity, this.transform);
                        break;
                    case Direction_type.down:
                        slide = Instantiate(Slide_down_Note, start_position, Quaternion.identity, this.transform);
                        break;
                    case Direction_type.left_up:
                        slide = Instantiate(Slide_left_up_Note, start_position, Quaternion.identity, this.transform);
                        break;
                    case Direction_type.left_down:
                        slide = Instantiate(Slide_left_down_Note, start_position, Quaternion.identity, this.transform);
                        break;
                    case Direction_type.right_up:
                        slide = Instantiate(Slide_right_up_Note, start_position, Quaternion.identity, this.transform);
                        break;
                    case Direction_type.right_down:
                        slide = Instantiate(Slide_right_down_Note, start_position, Quaternion.identity, this.transform);
                        break;
                }

                if (slide.GetComponent<ReworkNote>() != null)
                {
                    slide.GetComponent<ReworkNote>().set_target_transform(target);
                    slide.GetComponent<ReworkNote>().set_speed(speed);
                    slide.GetComponent<ReworkNote>().set_start_postion(start_position);
                    slide.GetComponent<ReworkNote>().set_spawn_time(spawn_time);
                }
                return slide;


            case Note3D_type.move: // Body Slide Type 추가 
                GameObject move = null;
                start_position = start_bottom.position;
                switch (move_dir)
                {
                    case Move_type.left:
                        move = Instantiate(Move_left_Note, start_position, Quaternion.Euler(-90.0f,0.0f,0.0f), this.transform);
                        break;
                    case Move_type.right:
                        move = Instantiate(Move_right_Note, start_position, Quaternion.Euler(-90.0f, 0.0f, 0.0f), this.transform);
                        break;
                    case Move_type.power_left:
                        move = Instantiate(Move_left_Note, start_position, Quaternion.Euler(-90.0f, 0.0f, 0.0f), this.transform);
                        break;
                    case Move_type.power_right:
                        move = Instantiate(Move_right_Note, start_position, Quaternion.Euler(-90.0f, 0.0f, 0.0f), this.transform);
                        break;
                }

                if (move.GetComponent<ReworkNote>() != null)
                {
                    move.GetComponent<ReworkNote>().set_target_transform(target);
                    move.GetComponent<ReworkNote>().set_speed(speed);
                    move.GetComponent<ReworkNote>().set_start_postion(start_position);
                    move.GetComponent<ReworkNote>().set_spawn_time(spawn_time);
                }

                return move;
        }
        return null;
    }

    public void add_transform_y(float y)
    {
        transform.position += new Vector3(0, y, 0);
    }
}
