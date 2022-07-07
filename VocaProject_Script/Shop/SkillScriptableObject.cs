using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillMode", menuName = "Scriptable Object/SkillMode")]
public class SkillScriptableObject : ScriptableObject // 플레이어 Skill 항목 Object (마지막 문제에 대한 특별연출)
{
    public string skill_name; // 스킬의 이름
    public bool purchased; // 해당 Skillmode 구입 여부
    public bool equipment; // 해당 Skillmode 장착 여부
    public int money; // 가격

    public Sprite skill_icon; // icon

    public GameObject director; // 실제 Player 스킬에 대한 연출이 적용 (GameObject를 Instantiate하는 식으로)
}
