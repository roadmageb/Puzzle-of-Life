using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public int stage_to_go;

    void OnMouseDown()
    {
        StageManager.Inst.StageSelected(stage_to_go);
    }
}
