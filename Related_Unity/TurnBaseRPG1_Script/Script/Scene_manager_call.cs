using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_manager_call : MonoBehaviour
{
    public void scenemove_call(string name)
    {
        SceneManagerCode.sceneManagerCode.Scene_move(name);
    }
}
