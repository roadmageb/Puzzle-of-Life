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
    private ConstraintController[] constraintControllers;
    public RuleResetButtonController[] ruleResetButtonControllers;
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
            ruleBorders[i + 1].GetComponent<ConstraintController>().
                ConstraintInstantiate(ruleNum, i, rule.constraints[i], constraintOffset);
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

        foreach (RuleResetButtonController rrbc in ruleResetButtonControllers)
        {
            if (!LevelManager.Inst.isEditorMode)
            {
                rrbc.gameObject.SetActive(false);
                continue;
            }
            rrbc.ruleNum = ruleNum;
            rrbc.constraintNum = -1;
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
    }
}
