using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information_list : MonoBehaviour
{
    [SerializeField] private List<suspect_information> suspect_informations;
    [SerializeField] private List<tool_information> tool_informations;
    [SerializeField] private List<place_information> place_informations;

    [SerializeField] private Information_update human_update;
    [SerializeField] private Information_update tool_update;
    [SerializeField] private Information_update place_update;
    [SerializeField] private Guess_information guess_update;

    [SerializeField] private bool need_updating;

    private Animator animator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        set_visualize(false);
    }

    public information get_suspect_computer_information(int index)
    {
        if (index >= suspect_informations.Count) return information.none;

        return suspect_informations[index].get_auto_cacl();
    }

    public information get_tool_computer_information(int index)
    {
        if (index >= tool_informations.Count) return information.none;

        return tool_informations[index].get_auto_cacl();
    }

    public information get_place_computer_information(int index)
    {
        if (index >= place_informations.Count) return information.none;

        return place_informations[index].get_auto_cacl();
    }

    public void set_suspect_computer_information(int index, information value)
    {
        if (index >= suspect_informations.Count) return;

        suspect_informations[index].set_auto_cacl(value);
    }

    public void set_tool_computer_information(int index, information value)
    {
        if (index >= tool_informations.Count) return;

        tool_informations[index].set_auto_cacl(value);
    }

    public void set_place_computer_information(int index, information value)
    {
        if (index >= place_informations.Count) return;

        place_informations[index].set_auto_cacl(value);
    }

    public bool get_need_updating()
    {
        return need_updating;
    }

    public void set_need_updating(bool value) 
    {
        need_updating = value;
    }

    public void init_information()
    {
        for (int i = 0; i < suspect_informations.Count; i++)
        {
            suspect_informations[i].set_auto_cacl(information.none);
            suspect_informations[i].set_human_cacl(information.none);

            human_update.set_information_computer(i,information.none);
            human_update.set_information_human(i, information.none);
        }
        human_update.init_childs();

        for (int i = 0; i < tool_informations.Count; i++)
        {
            tool_informations[i].set_auto_cacl(information.none);
            tool_informations[i].set_human_cacl(information.none);

            tool_update.set_information_computer(i, information.none);
            tool_update.set_information_human(i, information.none);
        }
        tool_update.init_childs();

        for (int i = 0; i < place_informations.Count; i++)
        {
            place_informations[i].set_auto_cacl(information.none);
            place_informations[i].set_human_cacl(information.none);

            place_update.set_information_computer(i, information.none);
            place_update.set_information_human(i, information.none);
        }
        place_update.init_childs();
        // 모든 정보 시각화를 초기화

        guess_update.visualize(suspect.nothing, murder_tool.nothing, crime_scene.nothing, -1);

        set_need_updating(false);
    }

    public void zero_match(suspect who, murder_tool what, crime_scene where) // 인자로 받은 정보가 하나도 안 맞은 경우
    {
        for(int i =0;i<suspect_informations.Count;i++)
        {
            if(suspect_informations[i].get_suspect() == who)
            {
                if(suspect_informations[i].get_auto_cacl() != information.not_this)
                {
                    suspect_informations[i].set_auto_cacl(information.not_this);
                    human_update.set_information_computer(i, information.not_this);
                }

                break;
            }
        }

        for (int i = 0; i < tool_informations.Count; i++)
        {
            if (tool_informations[i].get_tool() == what)
            {
                if (tool_informations[i].get_auto_cacl() != information.not_this)
                {
                    tool_informations[i].set_auto_cacl(information.not_this);
                    tool_update.set_information_computer(i, information.not_this);
                } 
                break;
            }
        }

        for (int i = 0; i < place_informations.Count; i++)
        {
            if (place_informations[i].get_place() == where)
            {
                if (place_informations[i].get_auto_cacl() != information.not_this)
                {
                    place_informations[i].set_auto_cacl(information.not_this);
                    place_update.set_information_computer(i, information.not_this);
                }
                break;
            }
        }

        guess_update.visualize(who, what, where, 0);

        set_need_updating(true);
    }

    public void more_match(suspect who, murder_tool what, crime_scene where, int answer) // 인자로 받은 정보가 하나 이상 맞은 경우
    {
        for (int i = 0; i < suspect_informations.Count; i++)
        {
            if (suspect_informations[i].get_suspect() == who)
            {
                if(suspect_informations[i].get_auto_cacl()!=information.not_this)
                {
                    suspect_informations[i].set_auto_cacl(information.wondering);
                    human_update.set_information_computer(i, information.wondering);
                }
                break;
            }
        }

        for (int i = 0; i < tool_informations.Count; i++)
        {
            if (tool_informations[i].get_tool() == what)
            {
                if (tool_informations[i].get_auto_cacl() != information.not_this)
                {
                    tool_informations[i].set_auto_cacl(information.wondering);
                    tool_update.set_information_computer(i, information.wondering);
                }
                break;
            }
        }

        for (int i = 0; i < place_informations.Count; i++)
        {
            if (place_informations[i].get_place() == where)
            {
                if (place_informations[i].get_auto_cacl() != information.not_this)
                {
                    place_informations[i].set_auto_cacl(information.wondering);
                    place_update.set_information_computer(i, information.wondering);
                }
                break;
            }
        }

        guess_update.visualize(who, what, where, answer);

        set_need_updating(true);
    }

    public void set_visualize(bool value) // 정보 Note를 Animator를 이용해 보일지 안보일지 설정
    {
        if(animator!=null)
        {
            animator.SetBool("Visualize", value);
        }
    }

    public void start_animation(string value) // value 이름을 가진 애니메이션을 Animator에서 재생
    {
        if(animator!=null)
        {
            animator.Play(value, -1, 0.0f);
        }
    }
}
