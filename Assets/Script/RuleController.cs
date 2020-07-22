using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleController : MonoBehaviour
{
    // 작성중
    public GameObject[] rulePrefab;
    public GameObject[] ruleBorders;
    public CellController[,] conditionCell;
    public CellController outcomeCell;
    public GameObject arrow;
    public CellController[] constraintCell;
    public float ruleHeight;
    public Transform conditionOffset, arrowOffset, outcomeOffset;
    public Vector2 constraintOffset;
    public float GetSpriteHeight(GameObject g)
    {
        return g.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public void RuleInstantiate(Rule rule)
    {
        ruleBorders = new GameObject[rule.constraints.Count + 2];
        ruleBorders[0] = Instantiate(rulePrefab[0], transform);
        ruleBorders[0].transform.localPosition = new Vector2(0, 0);
        ruleHeight += GetSpriteHeight(rulePrefab[0]);
        for (int i = 0; i < rule.constraints.Count; ++i)
        {
            ruleBorders[i + 1] = Instantiate(rulePrefab[1], transform);
            ruleBorders[i + 1].transform.localPosition = new Vector2(0, -GetSpriteHeight(rulePrefab[0]) - (i * GetSpriteHeight(rulePrefab[1])));
            ruleHeight += GetSpriteHeight(rulePrefab[1]);
        }
        ruleBorders[rule.constraints.Count + 1] = Instantiate(rulePrefab[2], transform);
        ruleBorders[rule.constraints.Count + 1].transform.localPosition =
            new Vector2(0, -GetSpriteHeight(rulePrefab[0]) - (rule.constraints.Count * GetSpriteHeight(rulePrefab[1])));
        ruleHeight += GetSpriteHeight(rulePrefab[2]);

        conditionCell = new CellController[3, 3];
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                conditionCell[i, j] = Instantiate(ImageManager.Inst.cellPrefabInRule, ruleBorders[0].transform).GetComponent<CellController>();
                conditionCell[i, j].transform.localPosition = new Vector3(i, -j) + conditionOffset.localPosition;
                conditionCell[i, j].CellInitialize(rule.condition[i, j], rule.isReplaceable[i, j], "Rule");
            }
        }
        arrow = Instantiate(arrow, ruleBorders[0].transform);
        arrow.transform.localPosition = arrowOffset.localPosition;
        outcomeCell = Instantiate(ImageManager.Inst.cellPrefabInRule, ruleBorders[0].transform).GetComponent<CellController>();
        outcomeCell.transform.localPosition = outcomeOffset.localPosition;
        outcomeCell.CellInitialize(rule.outcome, rule.isOutcomeReplaceable, "Rule");

        ConstraintInstantiate(rule.constraints);
    }

    public void ConstraintInstantiate(List<Constraint> constraints)
    {
        constraintCell = new CellController[constraints.Count];
        for (int i = 0; i < constraints.Count; ++i)
        {
            if (constraints[i].type != ConstraintType.BET)
            {
                constraintCell[i] = Instantiate(ImageManager.Inst.cellPrefabInRule, ruleBorders[i + 1].transform).GetComponent<CellController>();
                constraintCell[i].transform.localPosition = new Vector2(0, 0) + constraintOffset;
                constraintCell[i].CellInitialize(constraints[i].target, constraints[i].isReplaceable, "Rule");
                GameObject tmp;
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector2(1, 0) + constraintOffset;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[constraints[i].type];
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector2(2, 0) + constraintOffset;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraints[i].param1];
            }
            else
            {
                GameObject tmp;
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector2(-1, 0) + constraintOffset;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraints[i].param1];
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector2(0, 0) + constraintOffset;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
                constraintCell[i] = Instantiate(ImageManager.Inst.cellPrefabInRule, ruleBorders[i + 1].transform).GetComponent<CellController>();
                constraintCell[i].transform.localPosition = new Vector2(1, 0) + constraintOffset;
                constraintCell[i].CellInitialize(constraints[i].target, constraints[i].isReplaceable, "Rule");
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector2(2, 0) + constraintOffset;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector2(3, 0) + constraintOffset;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraints[i].param2];
            }
        }
    }
}
