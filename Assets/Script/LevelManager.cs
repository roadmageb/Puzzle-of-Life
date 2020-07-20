using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public bool isPlaymode = false;
    public CellController cellUnderCursor { get; set; }
    public Transform mapOrigin;
    public Transform ruleOrigin;
    public Transform paletteOrigin;
    public CellController[,] cellObject;
    public RuleController[] ruleObject;
    public PaletteController paletteObject;
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
        foreach (Transform child in mapOrigin)
        {
            Destroy(child.gameObject);
        }

        cellObject = new CellController[currentLevel.size.x, currentLevel.size.y];
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                cellObject[i, j] = Instantiate(ImageManager.Inst.cellPrefab, mapOrigin).GetComponent<CellController>();
                cellObject[i, j].transform.localPosition = new Vector2(i, -j);
                cellObject[i, j].CellInitialize(currentLevel.map[i, j], currentLevel.isReplaceable[i, j], "Map");
            }
        }
    }

    public void CellUpdate()
    {
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                cellObject[i, j].ChangeSprite(currentLevel.map[i, j]);
            }
        }
    }

    public void RuleInstantiate()
    {
        foreach (Transform child in ruleOrigin)
        {
            Destroy(child.gameObject);
        }

        float wholeRuleHeight = 0;
        ruleObject = new RuleController[currentLevel.rules.Count];

        for (int i = 0; i < currentLevel.rules.Count; ++i)
        {
            ruleObject[i] = Instantiate(ImageManager.Inst.rulePrefab, ruleOrigin).GetComponent<RuleController>();
            ruleObject[i].RuleInstantiate(currentLevel.rules[i]);
            ruleObject[i].transform.localPosition = new Vector2(0, -wholeRuleHeight);
            wholeRuleHeight += ruleObject[i].ruleHeight + ImageManager.Inst.ruleGap;
        }
    }

    public void PaletteInstantiate()
    {
        foreach (Transform child in paletteOrigin)
        {
            Destroy(child.gameObject);
        }

        paletteObject = Instantiate(ImageManager.Inst.palettePrefab, paletteOrigin).GetComponent<PaletteController>();
        paletteObject.PaletteInstantiate(currentLevel.palette);
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
        currentRule.AddConstraint(ConstraintType.NE, Cell.CELL1, 7, 0);
        currentRule.AddConstraint(ConstraintType.BET, Cell.CELL1, 2, 5);
        currentLevel.AddRule(currentRule);
        currentRule = new Rule();
        currentRule.SetConditionCell(new Vector2Int(0, 1), Cell.CELL1);
        currentRule.SetConditionCell(new Vector2Int(1, 1), Cell.TARGET1);
        currentRule.SetOutcome(Cell.NULL);
        currentLevel.SetCell(new Vector2Int(0, 1), Cell.CELL1);
        currentLevel.SetCell(new Vector2Int(1, 1), Cell.CELL1);
        currentLevel.AddRule(currentRule);
        CellInstantiate();
        RuleInstantiate();
        PaletteInstantiate();
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

        if (Input.GetKeyDown("q"))
        {
            Debug.Log("map reset");
            CellInstantiate();
            RuleInstantiate();
            PaletteInstantiate();
        }
    }
}
