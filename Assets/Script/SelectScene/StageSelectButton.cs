using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public int selected_stage;

    void OnMouseDown()
    {
        StageManager.Inst.StageSelected(selected_stage);
    }
}
