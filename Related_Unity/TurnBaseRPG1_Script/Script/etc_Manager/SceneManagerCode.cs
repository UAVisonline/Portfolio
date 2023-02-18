using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerCode : MonoBehaviour
{
    private static SceneManagerCode _sceneManagerCode;

    public static SceneManagerCode sceneManagerCode
    {
        get
        {
            if(_sceneManagerCode==null)
            {
                _sceneManagerCode = FindObjectOfType<SceneManagerCode>();
                if(_sceneManagerCode==null)
                {
                    Debug.LogError("Can't Find SceneManager Code Script");
                }
            }
            return _sceneManagerCode;
        }
    }

    private void Awake()
    {
        if(_sceneManagerCode==null)
        {
            _sceneManagerCode = FindObjectOfType<SceneManagerCode>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Scene_move(string scene_name)
    {
        string name = SceneManager.GetActiveScene().name;

        if(string.Compare(name,scene_name)!=0)
        {
            SceneManager.LoadScene(scene_name);
        }
    }
}
