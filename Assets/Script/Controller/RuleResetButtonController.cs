using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleResetButtonController : MonoBehaviour
{
    public bool isReset; // false = delete, true = reset;
    public bool isRule; // false = constraint, true = rule;
    public int ruleNum, constraintNum;

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        if (isReset)
        {
            if (isRule)
            {
                Rule rule = new Rule();
                Constraint constraint = new Constraint();
                constraint.SetDummy();
                rule.AddConstraint(constraint);
                LevelManager.Inst.currentLevel.rules[ruleNum] = rule;
                LevelManager.Inst.MapInstantiate();
            }
            else
            {
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum] = new Constraint();
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].SetDummy();
                LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].state = ConstraintState.SELTYPE;
                LevelManager.Inst.MapInstantiate();
            }
        }
        else
        {
            if (isRule)
            {
                LevelManager.Inst.currentLevel.RemoveRule(ruleNum);
                LevelManager.Inst.MapInstantiate();
            }
            else
            {
                LevelManager.Inst.currentLevel.rules[ruleNum].RemoveConstraint(constraintNum);
                LevelManager.Inst.MapInstantiate();
            }
        }
    }
}
