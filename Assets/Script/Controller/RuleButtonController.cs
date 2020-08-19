using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleButtonController : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleEditButtonSprites[1];
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleEditButtonSprites[0];
    }

    private void OnMouseUpAsButton()
    {
        GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleEditButtonSprites[0];
        Rule rule = new Rule();
        Constraint constraint = new Constraint();
        constraint.SetDummy();
        rule.AddConstraint(constraint);
        LevelManager.Inst.currentLevel.AddRule(rule);
        LevelManager.Inst.MapInstantiate();
    }
}
