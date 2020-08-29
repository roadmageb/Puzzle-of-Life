using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RuleScroller : MonoBehaviour
{
    public Transform rule;
    private float ruleMaxY, ruleMinY;
    private Vector2 mouseCoord;
    private float ruleOffset, wheelOffset;

    private void Scroll(float diff)
    {
        if (rule.position.y - diff < ruleMinY)
        {
            rule.position = new Vector3(rule.position.x, ruleMinY, rule.position.z);
        }
        else if (rule.position.y - diff > ruleMaxY)
        {
            rule.position = new Vector3(rule.position.x, ruleMaxY, rule.position.z);
        }
        else
        {
            rule.position -= new Vector3(0, diff, 0);
        }
    }

    private void Start()
    {
        ruleMinY = rule.position.y;
        ruleMaxY = rule.position.y + LevelManager.Inst.wholeRuleHeight - GetComponent<BoxCollider2D>().size.y * transform.localScale.y + ruleOffset;
        wheelOffset = 1.25f;
        ruleOffset = 0.5f;
    }


    private void OnMouseDown()
    {
        ruleMaxY = ruleMinY + LevelManager.Inst.wholeRuleHeight - GetComponent<BoxCollider2D>().size.y * transform.localScale.y + ruleOffset;
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
        Scroll(mouseCoord.y - currentMouseCoord.y);
        mouseCoord = currentMouseCoord;
    }

    private void OnMouseOver()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta != 0.0f)
        {
            ruleMaxY = ruleMinY + LevelManager.Inst.wholeRuleHeight - GetComponent<BoxCollider2D>().size.y * transform.localScale.y + ruleOffset;
            if (LevelManager.Inst.isEditorMode)
            {
                ruleMaxY += 1.5f;
            }
            if (ruleMaxY <= ruleMinY) return;

            if (scrollDelta > 0.0f) // scroll up
            {
                Scroll(wheelOffset);
            }
            if (scrollDelta < 0.0f) // scroll down
            {
                Scroll(-wheelOffset);
            }
        }
    }
}
