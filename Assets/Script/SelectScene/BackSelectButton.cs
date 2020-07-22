using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSelectButton  : MonoBehaviour
{
    void OnMouseDown()
    {
        StageManager.Inst.BackSelected();
    }
}
