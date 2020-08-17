using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RuleScroller : MonoBehaviour
{
    public Transform rule;
    private float ruleMaxY, ruleMinY;
    private Vector2 mouseCoord;
    private void Start()
    {
        ruleMinY = rule.position.y;
        ruleMaxY = rule.position.y + LevelManager.Inst.wholeRuleHeight - GetComponent<BoxCollider2D>().size.y * transform.localScale.y;
    }
    private void OnMouseDown()
    {
        ruleMaxY = ruleMinY + LevelManager.Inst.wholeRuleHeight - GetComponent<BoxCollider2D>().size.y * transform.localScale.y;
        if (LevelManager.Inst.isEditorMode)
        {
            ruleMaxY += 1.5f;
        }
        mouseCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDrag()
    {
        if (ruleMaxY <= ruleMinY) return;
        Vector2 currentMouseCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (rule.position.y - (mouseCoord.y - currentMouseCoord.y) < ruleMinY)
        {
            rule.position = new Vector3(rule.position.x, ruleMinY, rule.position.z);
        }
        else if (rule.position.y - (mouseCoord.y - currentMouseCoord.y) > ruleMaxY)
        {
            rule.position = new Vector3(rule.position.x, ruleMaxY, rule.position.z);
        }
        else
        {
            rule.position -= new Vector3(0, mouseCoord.y - currentMouseCoord.y, 0);
        }
        mouseCoord = currentMouseCoord;
    }
}
