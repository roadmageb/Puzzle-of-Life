using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditEditButton : SelectButton
{
    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.EditEditSelected();
        AudioManager.Inst.ButtonClicked();
    }
}
