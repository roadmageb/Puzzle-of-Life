using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArrowButton : SelectButton
{
    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.StageChange((int)GetComponent<Transform>().localScale.x);
    }
}
