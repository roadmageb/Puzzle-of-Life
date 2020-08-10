using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : SelectButton
{
    public int StageToGo;

    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.StageSelected(StageToGo);
    }
}
