using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public int selected_level;

    void OnMouseDown()
    {
        Debug.Log("level selected: " + selected_level);
    }
}
