using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditBackButton : SelectButton
{
    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.EditBackSelected();
        AudioManager.Inst.ButtonClicked();
    }
}
