using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public int level_to_go;

    void OnMouseDown()
    {
        Debug.Log("level selected: " + level_to_go);
    }
}
