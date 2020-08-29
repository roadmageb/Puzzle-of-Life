using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : SelectButton
{
    protected override void ButtonAction()
    {
        GameManager.Inst.BackToLevelEditor();
    }
}
