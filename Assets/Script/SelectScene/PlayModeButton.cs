using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeButton : SelectButton
{
    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.PlayModeSelected();
        AudioManager.Inst.ButtonClicked();
    }
}
