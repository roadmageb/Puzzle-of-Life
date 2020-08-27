using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditModeButton : SelectButton
{
    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.EditModeSelected();
        AudioManager.Inst.ButtonClicked();
    }
}
