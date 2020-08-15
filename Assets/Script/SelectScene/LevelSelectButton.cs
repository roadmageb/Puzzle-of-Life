using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : SelectButton
{
    public int LevelToGo;

    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.LevelSelected(LevelToGo);
    }
}
