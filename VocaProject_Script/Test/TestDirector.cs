using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestDirector : MonoBehaviour // Player 연출에 있어서 사용할 공용 Script
{
    private void OnEnable() // 생성 후 SetActive True -> Event Handler에 Event 할당
    {
        TestMaster.correct_problem_event += normal_attack;
        TestMaster.wrong_problem_event += Hurt;
        TestMaster.correct_end_problem_event += normal_attack_end;
        TestMaster.wrong_end_problem_event += Hurt_end;
        TestMaster.perfect_end_problem_event += Final_attack;

        //TestMaster.testmaster.start_problem();
    }

    private void OnDisable() // SetActive False -> Event 해제 후 Object 삭제
    {
        TestMaster.correct_problem_event -= normal_attack;
        TestMaster.wrong_problem_event -= Hurt;
        TestMaster.correct_end_problem_event -= normal_attack_end;
        TestMaster.wrong_end_problem_event -= Hurt_end;
        TestMaster.perfect_end_problem_event -= Final_attack;

        Destroy(this.gameObject);
    }

    public void normal_attack()
    {
        this.GetComponent<Animator>().Play("Attack", -1, 0.0f);
    }

    public void Hurt()
    {
        this.GetComponent<Animator>().Play("Hurt", -1, 0.0f);
    }

    public void normal_attack_end()
    {
        this.GetComponent<Animator>().Play("Attack_end", -1, 0.0f);
    }

    public void Hurt_end()
    {
        this.GetComponent<Animator>().Play("Hurt_end", -1, 0.0f);
    }

    public void Final_attack()
    {
        this.GetComponent<Animator>().Play("Final_Attack", -1, 0.0f);
    }

    public void test_end_event()
    {
        TestMaster.testmaster.animator_start();
    }
}
