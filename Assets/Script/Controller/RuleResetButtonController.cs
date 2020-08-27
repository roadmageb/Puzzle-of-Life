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
        if (isReset)
        {
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleResetButtonSprites[3];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleResetButtonSprites[1];
        }
    }

    private void OnMouseExit()
    {
        if (isReset)
        {
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleResetButtonSprites[2];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleResetButtonSprites[0];
        }
    }

    private void OnMouseUpAsButton()
    {
        if (isReset)
        {
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleResetButtonSprites[2];
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
            GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleResetButtonSprites[0];
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
