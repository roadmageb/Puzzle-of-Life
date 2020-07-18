using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public bool isPlaymode = false;
    public Transform mapOrigin;
    public Transform ruleOrigin;
    public GameObject cellPrefab;
    public GameObject rulePrefab;
    public CellController[,] cellObjectGrid;
    public RuleController[] ruleObjectGrid;
    public Level currentLevel;
    public Cell[,] previousCells;

    public void PlayLevel()
    {
        if (isPlaymode) return;
        isPlaymode = true;
        previousCells = new Cell[currentLevel.size.x, currentLevel.size.y];
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                previousCells[i, j] = currentLevel.map[i, j];
            }
        }

        StartCoroutine("CellCoroutine");
    }

    public void StopLevel()
    {
        isPlaymode = false;
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                currentLevel.SetCell(new Vector2Int(i, j), previousCells[i, j]);
            }
        }

        CellUpdate();
        StopCoroutine("CellCoroutine");
    }

    public void PlayFrame()
    {
        currentLevel.NextState();
        CellUpdate();
    }

    public IEnumerator CellCoroutine()
    {
        while (true)
        {
            currentLevel.NextState();
            CellUpdate();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void CellInstantiate()
    {
        while (mapOrigin.childCount > 0)
        {
            Destroy(mapOrigin.GetChild(0));
        }

        cellObjectGrid = new CellController[currentLevel.size.x, currentLevel.size.y];
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                cellObjectGrid[i, j] = Instantiate(cellPrefab, mapOrigin).GetComponent<CellController>();
                cellObjectGrid[i, j].transform.localPosition = new Vector2(i, -j);
                cellObjectGrid[i, j].ChangeSprite(currentLevel.map[i, j]);
            }
        }
    }

    public void CellUpdate()
    {
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                cellObjectGrid[i, j].ChangeSprite(currentLevel.map[i, j]);
            }
        }
    }

    public void RuleInstantiate()
    {
        float wholeRuleHeight = 0;
        ruleObjectGrid = new RuleController[currentLevel.rules.Count];

        for (int i = 0; i < currentLevel.rules.Count; ++i)
        {
            ruleObjectGrid[i] = Instantiate(rulePrefab, ruleOrigin).GetComponent<RuleController>();
            ruleObjectGrid[i].RuleInstantiate(currentLevel.rules[i]);
            ruleObjectGrid[i].transform.localPosition = new Vector2(0, -wholeRuleHeight);
            wholeRuleHeight = ruleObjectGrid[i].ruleHeight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Rule currentRule;
        currentLevel = new Level(new Vector2Int(6, 3));
        currentLevel.SetCell(new Vector2Int(4, 1), Cell.TARGET1);
        currentLevel.SwitchReplaceability(new Vector2Int(1, 0));
        currentLevel.SwitchReplaceability(new Vector2Int(1, 1));
        currentLevel.SwitchReplaceability(new Vector2Int(1, 2));
        currentLevel.AddPalette(Cell.CELL1, 1);
        currentRule = new Rule();
        currentRule.SetConditionCell(new Vector2Int(0, 1), Cell.CELL1);
        currentRule.SetOutcome(Cell.CELL1);
        currentLevel.AddRule(currentRule);
        currentRule = new Rule();
        currentRule.SetConditionCell(new Vector2Int(0, 1), Cell.CELL1);
        currentRule.SetConditionCell(new Vector2Int(1, 1), Cell.TARGET1);
        currentRule.SetOutcome(Cell.NULL);
        currentLevel.SetCell(new Vector2Int(1, 1), Cell.CELL1);
        currentRule.AddConstraint(ConstraintType.GE, Cell.CELL1, 1, 0);
        currentLevel.AddRule(currentRule);
        CellInstantiate();
        RuleInstantiate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            PlayLevel();
        }

        if (Input.GetKeyDown("d"))
        {
            StopLevel();
        }

        if (Input.GetKeyDown("f"))
        {
            PlayFrame();
        }
    }
}
