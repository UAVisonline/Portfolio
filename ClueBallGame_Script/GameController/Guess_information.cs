using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guess_information : MonoBehaviour // 이전 정보 유추에 대한 결과를 보여줌
{
    [SerializeField] private Text human_text;
    [SerializeField] private Text tool_text;
    [SerializeField] private Text place_text;
    [SerializeField] private Text answer_text;

    public void visualize(suspect human, murder_tool tool, crime_scene place, int value)
    {
        switch (human)
        {
            case suspect.kellog:
                human_text.text = "Kellog";
                break;
            case suspect.rinda:
                human_text.text = "Rinda";
                break;
            case suspect.android:
                human_text.text = "Android";
                break;
            case suspect.godrick:
                human_text.text = "godrick";
                break;
            case suspect.nameless:
                human_text.text = "Nameless";
                break;
            case suspect.nothing:
                human_text.text = "-----";
                break;
        }

        switch(tool)
        {
            case murder_tool.axe:
                tool_text.text = "도끼";
                break;
            case murder_tool.poision:
                tool_text.text = "독약";
                break;
            case murder_tool.hammer:
                tool_text.text = "망치";
                break;
            case murder_tool.knife:
                tool_text.text = "칼";
                break;
            case murder_tool.crowbar:
                tool_text.text = "크로우바";
                break;
            case murder_tool.shovel:
                tool_text.text = "삽";
                break;
            case murder_tool.nothing:
                tool_text.text = "-----";
                break;
        }

        switch(place)
        {
            case crime_scene.livingroom:
                place_text.text = "거실";
                break;
            case crime_scene.library:
                place_text.text = "서재";
                break;
            case crime_scene.bathroom:
                place_text.text = "욕실";
                break;
            case crime_scene.garden:
                place_text.text = "정원";
                break;
            case crime_scene.kitchen:
                place_text.text = "부엌";
                break;
            case crime_scene.warehouse:
                place_text.text = "창고";
                break;
            case crime_scene.bedroom:
                place_text.text = "침실";
                break;
            case crime_scene.dressroom:
                place_text.text = "옷방";
                break;
            case crime_scene.nothing:
                place_text.text = "-----";
                break;
        }

        switch(value)
        {
            case -1:
                answer_text.text = "-----";
                break;
            case 0:
                answer_text.text = "0";
                break;
            case 1:
                answer_text.text = "1";
                break;
            case 2:
            case 3:
                answer_text.text = "+2";
                break;
        }

    }
}
