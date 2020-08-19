using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintNumButtonController : MonoBehaviour
{
    public int num;
    public ConstraintButtonController constraintButtonController;
    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.constraintNumButtonSprites[num * 2 + 1];
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.constraintNumButtonSprites[num * 2];
    }

    private void OnMouseUpAsButton()
    {
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.constraintNumButtonSprites[num * 2];
        constraintButtonController.EditParamNum(num);
    }
}
