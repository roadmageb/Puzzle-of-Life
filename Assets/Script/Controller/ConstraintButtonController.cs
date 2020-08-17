using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintButtonController : MonoBehaviour
{
    public EditorButtonType editorButtonType { get; set; }
    public ConstraintType constraintType { get; set; }
    public LevelEditor levelEditor;
    public int ruleNum, constraintNum, paramNum;
    private void OnMouseDown()
    {
    }

    private void OnMouseUpAsButton()
    {
        switch (editorButtonType)
        {
            case EditorButtonType.ADD:
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].state = ConstraintState.SELTYPE;
                Constraint constraint = new Constraint();
                constraint.SetDummy();
                LevelManager.Inst.currentLevel.rules[ruleNum].AddConstraint(constraint);
                LevelManager.Inst.MapInstantiate();
                break;
            case EditorButtonType.CONSTTYPE:
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].state = ConstraintState.SELNUM;
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].type = constraintType;
                LevelManager.Inst.MapInstantiate();
                break;
        }
    }

    public void EditParamNum(int num)
    {
        if (editorButtonType == EditorButtonType.CONSTNUM)
        {
            if (paramNum == 0)
            {
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].param1 = num;
            }
            else if (paramNum == 1)
            {
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].param2 = num;
            }
            if (LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].type != ConstraintType.BET ||
                (LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].param1 != -1 &&
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].param2 != -1)) // BET가 아니면 바로 pass, BET면 param1, 2 모두 체크
            {
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].state = ConstraintState.NORMAL;
            }
            LevelManager.Inst.MapInstantiate();
        }
    }
}
