using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PatternCheck : MonoBehaviour
{
    [SerializeField] private string file_name;
    [ReadOnly] [SerializeField] private float time;
    [SerializeField] List<float> time_list;
    [SerializeField] List<string> string_list;

    private void Start()
    {
        this.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        time = this.GetComponent<AudioSource>().time;

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            time_check(time,"Left_Down");
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            time_check(time, "Left");
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            time_check(time, "Left_Up");
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            time_check(time, "Right_Down");
        }

        if(Input.GetKeyDown(KeyCode.Keypad5))
        {
            time_check(time, "Middle");
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            time_check(time, "Right");
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            time_check(time, "Right_Up");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            time_check(time, "Down");
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            time_check(time, "Up");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            time_check(time, "Move Left");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            time_check(time, "Move Right");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            time_check(time, "Space");
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            time_check(time, "Return");
        }
    }

    private void time_check(float value,string name)
    {
        time_list.Add(value);
        string_list.Add(name);
    }

    [Button]
    public void save_file()
    {
        StreamWriter writer = new StreamWriter(file_name + ".txt");
        for(int i =0;i<time_list.Count;i++)
        {
            writer.WriteLine(time_list[i] + "{" + string_list[i] + "}");
        }
        writer.Close();
    }
}
