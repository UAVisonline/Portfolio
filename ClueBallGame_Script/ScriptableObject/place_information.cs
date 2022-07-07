using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "place_information", menuName = "ScriptableObject/place_information")]
public class place_information : ScriptableObject
{
    [SerializeField] private crime_scene place; // 
    [SerializeField] private information auto_cacl; // 시스템 상 상태 설정 (맞음, 틀림, 모르겠음)
    [SerializeField] private information human_cacl; // 사람이 직접 상태 설정

    public crime_scene get_place()
    {
        return place;
    }

    public information get_auto_cacl() 
    {
        return auto_cacl;
    }

    public information get_human_cacl()
    {
        return human_cacl;
    }

    public void set_auto_cacl(information value)
    {
        auto_cacl = value;
    }

    public void set_human_cacl(information value)
    {
        human_cacl = value;
    }
}
