using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintNumButtonController : MonoBehaviour
{
    public int num;
    public ConstraintButtonController constraintButtonController;
    private void OnMouseDown()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        constraintButtonController.EditParamNum(num);
    }
}
