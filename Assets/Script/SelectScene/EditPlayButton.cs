using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPlayButton : SelectButton
{
    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.EditPlaySelected();
        AudioManager.Inst.ButtonClicked();
    }
}
