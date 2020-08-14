using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintController : MonoBehaviour
{
    public RuleCellController constraintCell { get; private set; }
    public RuleResetButtonController[] ruleResetButtonControllers;
    private ConstraintButtonController[] constraintButton;
    private int ruleNum, constraintNum;

    public void ConstraintInstantiate(int ruleNum, int constraintNum, Constraint constraint, Transform constraintOffset)
    {
        this.ruleNum = ruleNum;
        this.constraintNum = constraintNum;
        foreach (RuleResetButtonController rrbc in ruleResetButtonControllers)
        {
            rrbc.ruleNum = ruleNum;
            rrbc.constraintNum = constraintNum;
        }

        if (constraint.state == ConstraintState.DUMMY)
        {
            constraintButton = new ConstraintButtonController[1];
            constraintButton[0] = Instantiate(ImageManager.Inst.constraintButtonPrefab, transform).GetComponent<ConstraintButtonController>();
            constraintButton[0].transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
            constraintButton[0].ruleNum = ruleNum;
            constraintButton[0].constraintNum = constraintNum;
            constraintButton[0].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleEditButtonSprites[2];
            constraintButton[0].editorButtonType = EditorButtonType.ADD;
            foreach (RuleResetButtonController rrbc in ruleResetButtonControllers)
            {
                rrbc.gameObject.SetActive(false);
            }
        }
        else if (constraint.state == ConstraintState.SELTYPE)
        {
            constraintButton = new ConstraintButtonController[5];
            for (int i = 0; i < 5; ++i)
            {
                constraintButton[i] = Instantiate(ImageManager.Inst.constraintButtonPrefab, transform).GetComponent<ConstraintButtonController>();
                constraintButton[i].transform.localPosition = new Vector3(i - 1, 0, 0) + constraintOffset.localPosition;
                constraintButton[i].ruleNum = ruleNum;
                constraintButton[i].constraintNum = constraintNum;
                constraintButton[i].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleEditButtonSprites[4 + i * 2];
                constraintButton[i].editorButtonType = EditorButtonType.CONSTTYPE;
            }
            constraintButton[0].constraintType = ConstraintType.LE;
            constraintButton[1].constraintType = ConstraintType.GE;
            constraintButton[2].constraintType = ConstraintType.EQ;
            constraintButton[3].constraintType = ConstraintType.NE;
            constraintButton[4].constraintType = ConstraintType.BET;
        }
        else if (constraint.state == ConstraintState.SELNUM)
        {
            if (constraint.type != ConstraintType.BET)
            {
                GameObject tmp;
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform); // prefab 없이 instantiate 하는 방법이 있는지?
                tmp.transform.localPosition = new Vector3(0, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleCellSprite;
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
                tmp.transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[constraint.type];
                constraintButton = new ConstraintButtonController[1];
                constraintButton[0] = Instantiate(ImageManager.Inst.constraintNumButtonPrefab, transform).GetComponent<ConstraintButtonController>();
                constraintButton[0].transform.localPosition = new Vector3(2, 0, 0) + constraintOffset.localPosition;
                constraintButton[0].ruleNum = ruleNum;
                constraintButton[0].constraintNum = constraintNum;
                constraintButton[0].editorButtonType = EditorButtonType.CONSTNUM;
            }
            else
            {
                GameObject tmp;
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
                tmp.transform.localPosition = new Vector3(0, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
                tmp.transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.ruleCellSprite;
                tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
                tmp.transform.localPosition = new Vector3(2, 0, 0) + constraintOffset.localPosition;
                tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
                constraintButton = new ConstraintButtonController[2];
                if (constraint.param1 == -1)
                {
                    constraintButton[0] = Instantiate(ImageManager.Inst.constraintNumButtonPrefab, transform).GetComponent<ConstraintButtonController>();
                    constraintButton[0].transform.localPosition = new Vector3(-1, 0, 0) + constraintOffset.localPosition;
                    constraintButton[0].ruleNum = ruleNum;
                    constraintButton[0].constraintNum = constraintNum;
                    constraintButton[0].paramNum = 0;
                    constraintButton[0].editorButtonType = EditorButtonType.CONSTNUM;
                }
                else
                {
                    tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
                    tmp.transform.localPosition = new Vector3(-1, 0, 0) + constraintOffset.localPosition;
                    tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraint.param1];
                }
                if (constraint.param2 == -1)
                {
                    constraintButton[1] = Instantiate(ImageManager.Inst.constraintNumButtonPrefab, transform).GetComponent<ConstraintButtonController>();
                    constraintButton[1].transform.localPosition = new Vector3(3, 0, 0) + constraintOffset.localPosition;
                    constraintButton[1].ruleNum = ruleNum;
                    constraintButton[1].constraintNum = constraintNum;
                    constraintButton[1].paramNum = 1;
                    constraintButton[1].editorButtonType = EditorButtonType.CONSTNUM;
                }
                else
                {
                    tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
                    tmp.transform.localPosition = new Vector3(3, 0, 0) + constraintOffset.localPosition;
                    tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraint.param2];
                }
            }
        }
        else if (constraint.type != ConstraintType.BET)
        {
            constraintCell = Instantiate(ImageManager.Inst.cellPrefabInRule, transform).GetComponent<RuleCellController>();
            constraintCell.transform.localPosition = new Vector3(0, 0, 0) + constraintOffset.localPosition;
            constraintCell.CellInitialize(constraint.target, constraint.isReplaceable, ruleNum, constraintNum);
            GameObject tmp;
            tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
            tmp.transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
            tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[constraint.type];
            tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
            tmp.transform.localPosition = new Vector3(2, 0, 0) + constraintOffset.localPosition;
            tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraint.param1];
        }
        else
        {
            GameObject tmp;
            tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
            tmp.transform.localPosition = new Vector3(-1, 0, 0) + constraintOffset.localPosition;
            tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraint.param1];
            tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
            tmp.transform.localPosition = new Vector3(0, 0, 0) + constraintOffset.localPosition;
            tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
            constraintCell = Instantiate(ImageManager.Inst.cellPrefabInRule, transform).GetComponent<RuleCellController>();
            constraintCell.transform.localPosition = new Vector3(1, 0, 0) + constraintOffset.localPosition;
            constraintCell.CellInitialize(constraint.target, constraint.isReplaceable, ruleNum, constraintNum);
            tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
            tmp.transform.localPosition = new Vector3(2, 0, 0) + constraintOffset.localPosition;
            tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.symbolSpriteDict[ConstraintType.LE];
            tmp = Instantiate(ImageManager.Inst.symbolPrefab, transform);
            tmp.transform.localPosition = new Vector3(3, 0, 0) + constraintOffset.localPosition;
            tmp.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.numberSprites[constraint.param2];
        }
    }
}
