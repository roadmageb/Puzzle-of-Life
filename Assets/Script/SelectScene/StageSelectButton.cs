using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : SelectButton
{
    private int StageToGo;

    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.StageSelected(StageToGo);
        AudioManager.Inst.ButtonClicked();
    }

    public void SetStageSelectButton(int Stage)
    {
        StageToGo = Stage;
    }
}
