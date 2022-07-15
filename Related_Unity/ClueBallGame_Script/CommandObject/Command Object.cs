using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandObject : MonoBehaviour, Command
{
    abstract public void execute();

    //abstract public void de_execute();

}
