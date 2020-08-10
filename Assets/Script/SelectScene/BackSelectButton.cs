using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BackSelectButton  : SelectButton
{
    protected override void ButtonAction()
    {
        SelectSceneManager.Inst.BackSelected();
    }
}
