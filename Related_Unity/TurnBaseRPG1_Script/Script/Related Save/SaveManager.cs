using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _saveManager;

    public static SaveManager saveManager
    {
        get
        {
            if(_saveManager==null)
            {
                _saveManager = FindObjectOfType<SaveManager>();
                if(_saveManager==null)
                {
                    Debug.LogError("Can't find SaveManager");
                }
            }
            return _saveManager;
        }
    }

    private void Awake()
    {
        if(_saveManager==null)
        {
            _saveManager = FindObjectOfType<SaveManager>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public T JsonToObject<T> (string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void SaveJsonFile(string filename, object value)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.persistentDataPath, filename), FileMode.Create);
        string jsonData = ObjectToJson(value);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public T LoadJsonFile<T>(string filename)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.persistentDataPath, filename), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        string jsonData = Encoding.UTF8.GetString(data);
        fileStream.Close();
        return JsonToObject<T>(jsonData);
    }

    public bool ExistFile(string filename)
    {
        FileInfo info = new FileInfo(string.Format("{0}/{1}.json", Application.persistentDataPath, filename));
        if(info.Exists)
        {
            return true;
        }
        return false;
    }
}
