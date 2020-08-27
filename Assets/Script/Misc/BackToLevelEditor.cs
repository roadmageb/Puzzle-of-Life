using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToLevelEditor : MonoBehaviour
{
    public void Back()
    {
        GameManager.Inst.BackToLevelEditor();
    }
}
