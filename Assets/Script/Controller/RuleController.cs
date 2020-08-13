using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleController : MonoBehaviour
{
    public GameObject[] rulePrefab;
    private GameObject[] ruleBorders;
    private SpriteRenderer[] ruleNumSprite;
    public RuleCellController[,] conditionCell { get; private set; }
    public RuleCellController outcomeCell { get; private set; }
    public RuleCellController[] constraintCell { get; private set; }
    public float ruleHeight { get; set; }
    public Transform conditionOffset, outcomeOffset, constraintOffset, ruleNumOffset;
    private int ruleNum;
    public bool isEditMode { get; set; }
    private ConstraintButtonController[][] constraintButton;
    private float GetSpriteHeight(GameObject g)
    {
        return g.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public void RuleInstantiate(Rule rule, int num)
    {
        ruleNum = num;
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

        if (num + 1 < 10)
        {
            ruleNumSprite = new SpriteRenderer[2];
            ruleNumSprite[0] = Instantiate(ImageManager.Inst.symbolPrefab, transform).GetComponent<SpriteRenderer>();
            ruleNumSprite[0].sprite = ImageManager.Inst.ruleNumSprites[10];
            ruleNumSprite[0].transform.localPosition = ruleNumOffset.transform.localPosition;
            ruleNumSprite[1] = Instantiate(ImageManager.Inst.symbolPrefab, transform).GetComponent<SpriteRenderer>();
            ruleNumSprite[1].sprite = ImageManager.Inst.ruleNumSprites[(num + 1) % 10];
            ruleNumSprite[1].transform.localPosition = ruleNumOffset.transform.localPosition + new Vector3((float)7 / 32, 0, 0);
        }
        else
        {
            ruleNumSprite = new SpriteRenderer[3];
            ruleNumSprite[0] = Instantiate(ImageManager.Inst.symbolPrefab, transform).GetComponent<SpriteRenderer>();
            ruleNumSprite[0].sprite = ImageManager.Inst.ruleNumSprites[10];
            ruleNumSprite[0].transform.localPosition = ruleNumOffset.transform.localPosition;
            ruleNumSprite[1] = Instantiate(ImageManager.Inst.symbolPrefab, transform).GetComponent<SpriteRenderer>();
            ruleNumSprite[1].sprite = ImageManager.Inst.ruleNumSprites[(num + 1) / 10];
            ruleNumSprite[1].transform.localPosition = ruleNumOffset.transform.localPosition + new Vector3((float)7 / 32, 0, 0);
            ruleNumSprite[2] = Instantiate(ImageManager.Inst.symbolPrefab, transform).GetComponent<SpriteRenderer>();
            ruleNumSprite[2].sprite = ImageManager.Inst.ruleNumSprites[(num + 1) % 10];
            ruleNumSprite[2].transform.localPosition = ruleNumOffset.transform.localPosition + new Vector3((float)14 / 32, 0, 0);
        }

        conditionCell = new RuleCellController[3, 3];
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (i == 1 && j == 1)
                {
                    conditionCell[i, j] = Instantiate(ImageManager.Inst.cellPrefabInRuleIO, ruleBorders[0].transform).GetComponent<RuleCellController>();
                    conditionCell[i, j].transform.localPosition = new Vector3(i, -j) + conditionOffset.localPosition;
                    conditionCell[i, j].CellInitialize(rule.condition[i, j], rule.isReplaceable[i, j], ruleNum, new Vector2Int(i, j));
                }
                else
                {
                    conditionCell[i, j] = Instantiate(ImageManager.Inst.cellPrefabInRule, ruleBorders[0].transform).GetComponent<RuleCellController>();
                    conditionCell[i, j].transform.localPosition = new Vector3(i, -j) + conditionOffset.localPosition;
                    conditionCell[i, j].CellInitialize(rule.condition[i, j], rule.isReplaceable[i, j], ruleNum, new Vector2Int(i, j));
                }
            }
        }
        outcomeCell = Instantiate(ImageManager.Inst.cellPrefabInRuleIO, ruleBorders[0].transform).GetComponent<RuleCellController>();
        outcomeCell.transform.localPosition = outcomeOffset.localPosition;
        outcomeCell.CellInitialize(rule.outcome, rule.isOutcomeReplaceable, ruleNum);

        ConstraintInstantiate(rule.constraints);
    }

    private void ConstraintInstantiate(List<Constraint> constraints)
    {
        constraintCell = new RuleCellController[constraints.Count];
        constraintButton = new ConstraintButtonController[constraints.Count][];

        for (int i = 0; i < constraints.Count; ++i)
        {
            if (constraints[i].state == ConstraintState.DUMMY)
            {
                constraintButton[i] = new ConstraintButtonController[1];
                constraintButton[i][0] = Instantiate(ImageManager.Inst.constraintButtonPrefab, ruleBorders[i + 1].transform).GetComponent<ConstraintButtonController>();
                constraintButton[i][0].transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
                constraintButton[i][0].ruleNum = ruleNum;
                constraintButton[i][0].constraintNum = i;
                constraintButton[i][0].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleEditButtonSprites[2];
                constraintButton[i][0].editorButtonType = EditorButtonType.ADD;
            }
            else if (constraints[i].state == ConstraintState.SELTYPE)
            {
                constraintButton[i] = new ConstraintButtonController[5];
                for (int j = 0; j < 5; ++j)
                {
                    constraintButton[i][j] = Instantiate(ImageManager.Inst.constraintButtonPrefab, ruleBorders[i + 1].transform).GetComponent<ConstraintButtonController>();
                    constraintButton[i][j].transform.localPosition = new Vector3(j - 1, 0, 0) + constraintOffset.localPosition;
                    constraintButton[i][j].ruleNum = ruleNum;
                    constraintButton[i][j].constraintNum = i;
                    constraintButton[i][j].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleEditButtonSprites[4 + j * 2];
                    constraintButton[i][j].editorButtonType = EditorButtonType.CONSTTYPE;
                }
                constraintButton[i][0].constraintType = ConstraintType.LE;
                constraintButton[i][1].constraintType = ConstraintType.GE;
                constraintButton[i][2].constraintType = ConstraintType.EQ;
                constraintButton[i][3].constraintType = ConstraintType.NE;
                constraintButton[i][4].constraintType = ConstraintType.BET;
            }
            else if (constraints[i].state == ConstraintState.SELNUM)
            {
                if (constraints[i].type != ConstraintType.BET)
                {
                    GameObject tmp;
                    tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform); // prefab 없이 instantiate 하는 방법이 있는지?
                    tmp.transform.localPosition = new Vector3(0, 0, 0) + constraintOffset.localPosition;
                    tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleCellSprite;
                    tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                    tmp.transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
                    tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[constraints[i].type];
                    constraintButton[i] = new ConstraintButtonController[1];
                    constraintButton[i][0] = Instantiate(ImageManager.Inst.constraintNumButtonPrefab, ruleBorders[i + 1].transform).GetComponent<ConstraintButtonController>();
                    constraintButton[i][0].transform.localPosition = new Vector3(2, 0, 0) + constraintOffset.localPosition;
                    constraintButton[i][0].ruleNum = ruleNum;
                    constraintButton[i][0].constraintNum = i;
                    constraintButton[i][0].editorButtonType = EditorButtonType.CONSTNUM;
                }
                else
                {
                    GameObject tmp;
                    tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                    tmp.transform.localPosition = new Vector3(0, 0, 0) + constraintOffset.localPosition;
                    tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
                    tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                    tmp.transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
                    tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleCellSprite;
                    tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                    tmp.transform.localPosition = new Vector3(2, 0, 0) + constraintOffset.localPosition;
                    tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
                    constraintButton[i] = new ConstraintButtonController[2];
                    if (constraints[i].param1 == -1)
                    {
                        constraintButton[i][0] = Instantiate(ImageManager.Inst.constraintNumButtonPrefab, ruleBorders[i + 1].transform).GetComponent<ConstraintButtonController>();
                        constraintButton[i][0].transform.localPosition = new Vector3(-1, 0, 0) + constraintOffset.localPosition;
                        constraintButton[i][0].ruleNum = ruleNum;
                        constraintButton[i][0].constraintNum = i;
                        constraintButton[i][0].paramNum = 0;
                        constraintButton[i][0].editorButtonType = EditorButtonType.CONSTNUM;
                    }
                    else
                    {
                        tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                        tmp.transform.localPosition = new Vector3(-1, 0, 0) + constraintOffset.localPosition;
                        tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraints[i].param1];
                    }
                    if (constraints[i].param2 == -1)
                    {
                        constraintButton[i][1] = Instantiate(ImageManager.Inst.constraintNumButtonPrefab, ruleBorders[i + 1].transform).GetComponent<ConstraintButtonController>();
                        constraintButton[i][1].transform.localPosition = new Vector3(3, 0, 0) + constraintOffset.localPosition;
                        constraintButton[i][1].ruleNum = ruleNum;
                        constraintButton[i][1].constraintNum = i;
                        constraintButton[i][1].paramNum = 1;
                        constraintButton[i][1].editorButtonType = EditorButtonType.CONSTNUM;
                    }
                    else
                    {
                        tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                        tmp.transform.localPosition = new Vector3(3, 0, 0) + constraintOffset.localPosition;
                        tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraints[i].param2];
                    }
                }
            }
            else if (constraints[i].type != ConstraintType.BET)
            {
                constraintCell[i] = Instantiate(ImageManager.Inst.cellPrefabInRule, ruleBorders[i + 1].transform).GetComponent<RuleCellController>();
                constraintCell[i].transform.localPosition = new Vector3(0, 0, 0) + constraintOffset.localPosition;
                constraintCell[i].CellInitialize(constraints[i].target, constraints[i].isReplaceable, ruleNum, i);
                GameObject tmp;
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[constraints[i].type];
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector3(2, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraints[i].param1];
            }
            else
            {
                GameObject tmp;
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector3(-1, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraints[i].param1];
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector3(0, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
                constraintCell[i] = Instantiate(ImageManager.Inst.cellPrefabInRule, ruleBorders[i + 1].transform).GetComponent<RuleCellController>();
                constraintCell[i].transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
                constraintCell[i].CellInitialize(constraints[i].target, constraints[i].isReplaceable, ruleNum, i);
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector3(2, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, ruleBorders[i + 1].transform);
                tmp.transform.localPosition = new Vector3(3, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraints[i].param2];
            }
        }
    }
}
