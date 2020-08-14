using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleButtonController : MonoBehaviour
{
    private void OnMouseDown()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        Rule rule = new Rule();
        Constraint constraint = new Constraint();
        rule.AddConstraint(constraint);
        LevelManager.Inst.currentLevel.AddRule(rule);
        LevelManager.Inst.MapInstantiate();
    }
}
