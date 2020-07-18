using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleController : MonoBehaviour
{
    // 작성중
    public GameObject[] rulePrefab;
    public GameObject[] ruleBorders;
    public CellController[,] cellObjectGrid;
    public CellController outcomeCell;
    public GameObject arrow;
    public float ruleHeight;
    public Transform conditionOffset, arrowOffset, outcomeOffset;
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

        cellObjectGrid = new CellController[3, 3];
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                cellObjectGrid[i, j] = Instantiate(LevelManager.Inst.cellPrefab, ruleBorders[0].transform).GetComponent<CellController>();
                cellObjectGrid[i, j].transform.localPosition = new Vector3(i, -j) + conditionOffset.localPosition;
                cellObjectGrid[i, j].ChangeSprite(rule.condition[i, j]);
            }
        }
        arrow = Instantiate(arrow, ruleBorders[0].transform);
        arrow.transform.localPosition = arrowOffset.localPosition;
        outcomeCell = Instantiate(LevelManager.Inst.cellPrefab, ruleBorders[0].transform).GetComponent<CellController>();
        outcomeCell.transform.localPosition = outcomeOffset.localPosition;
        outcomeCell.ChangeSprite(rule.outcome);
    }
}
