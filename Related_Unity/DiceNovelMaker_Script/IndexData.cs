using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "IndexTemplate", menuName = "Scriptable Object/IndexDataTemplate")]
public class IndexData : ScriptableObject
{
    [SerializeField] private string title;

    [BoxGroup("Setting")] private List<string> setting;
    [BoxGroup("Chapter")] private List<string> chapter; // 해당 데이터 내부에는 List값이 저장되어 있음
    [BoxGroup("Chapter_Content_index")] private List<int> chapter_index;

    public void delete_all_content()
    {
        for(int i =0;i<setting.Count;i++)
        {
            ES3.DeleteKey(title + "_Setting_" + setting[i]);
        }
        setting.Clear();

        ES3.DeleteKey(title);
    }
}
