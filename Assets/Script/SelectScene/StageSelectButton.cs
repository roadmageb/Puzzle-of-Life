using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : SelectButton
{
    private int StageToGo;

    public AudioSource ClickSound;

    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.StageSelected(StageToGo);
        ClickSound.Play();
    }

    public void SetStageSelectButton(int Stage)
    {
        StageToGo = Stage;
    }
}
