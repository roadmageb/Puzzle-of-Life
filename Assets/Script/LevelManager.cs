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
    public Transform[][] cellBorderObject;
    public RuleController[] ruleObject;
    public PaletteController paletteObject;
    public Level currentLevel;
    public Cell[,] previousCells;
    public float wholeRuleHeight;

    public void PlayLevel()
    {
        if (isPlaymode) return;
        Debug.Log("playlevel called");
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
            Debug.Log("coroutine called");
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
                cellObject[i, j].transform.localPosition = new Vector2(i + 1, -(j + 1));
                cellObject[i, j].CellInitialize(currentLevel.map[i, j], currentLevel.isReplaceable[i, j], "Map");
            }
        }

        cellBorderObject = new Transform[currentLevel.size.x + 2][];
        for (int i = 0; i < currentLevel.size.x + 2; ++i)
        {
            if (i == 0 || i == currentLevel.size.x + 1)
            {
                cellBorderObject[i] = new Transform[currentLevel.size.y + 2];
                for (int j = 0; j < currentLevel.size.y + 2; ++j)
                {
                    cellBorderObject[i][j] = Instantiate(ImageManager.Inst.cellBorderPrefab, mapOrigin).GetComponent<Transform>();
                    cellBorderObject[i][j].localPosition = new Vector2(i, -j);
                }
            }
            else
            {
                cellBorderObject[i] = new Transform[2];
                cellBorderObject[i][0] = Instantiate(ImageManager.Inst.cellBorderPrefab, mapOrigin).GetComponent<Transform>();
                cellBorderObject[i][0].localPosition = new Vector2(i, 0);
                cellBorderObject[i][0].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellBorderSprites[1]; // Top
                cellBorderObject[i][1] = Instantiate(ImageManager.Inst.cellBorderPrefab, mapOrigin).GetComponent<Transform>();
                cellBorderObject[i][1].localPosition = new Vector2(i, -(currentLevel.size.y + 1));
                cellBorderObject[i][1].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellBorderSprites[6]; // Bottom
            }
        }

        for (int i = 1; i < currentLevel.size.y + 1; ++i)
        {
            cellBorderObject[0][i].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellBorderSprites[3]; // Left
            cellBorderObject[currentLevel.size.x + 1][i].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellBorderSprites[4]; // Right
        }

        cellBorderObject[0][0].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellBorderSprites[0]; // Top-Left
        cellBorderObject[currentLevel.size.x + 1][0].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellBorderSprites[2]; // Top-Right
        cellBorderObject[0][currentLevel.size.y + 1].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellBorderSprites[5]; // Bottom-Left
        cellBorderObject[currentLevel.size.x + 1][currentLevel.size.y + 1].GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellBorderSprites[7]; // Bottom-Right
    }

    public void CellUpdate()
    {
        for (int i = 0; i < currentLevel.size.x; ++i)
        {
            for (int j = 0; j < currentLevel.size.y; ++j)
            {
                cellObject[i, j].ChangeSpriteByCell(currentLevel.map[i, j]);
            }
        }
    }

    public void RuleInstantiate()
    {
        foreach (Transform child in ruleOrigin)
        {
            Destroy(child.gameObject);
        }

        wholeRuleHeight = 0;
        ruleObject = new RuleController[currentLevel.rules.Count];

        for (int i = 0; i < currentLevel.rules.Count; ++i)
        {
            ruleObject[i] = Instantiate(ImageManager.Inst.rulePrefab, ruleOrigin).GetComponent<RuleController>();
            ruleObject[i].RuleInstantiate(currentLevel.rules[i]);
            ruleObject[i].transform.localPosition = new Vector2(0, -wholeRuleHeight);
            wholeRuleHeight += ruleObject[i].ruleHeight + ImageManager.Inst.ruleGap;
        }
        wholeRuleHeight -= ImageManager.Inst.ruleGap;
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
    public void MapScale(int criteria)
    {
        int maxValue = Math.Max(currentLevel.size.x, currentLevel.size.y);
        if (maxValue > criteria)
        {
            mapOrigin.localScale = new Vector3((float)criteria / maxValue, (float)criteria / maxValue, 1);
        }
        else
        {
            mapOrigin.localScale = new Vector3(1, 1, 1);
        }
    }

    public void MapInstantiate()
    {
        CellInstantiate();
        RuleInstantiate();
        PaletteInstantiate();
        MapScale(5);
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        Rule currentRule;
        currentLevel = new Level(new Vector2Int(6, 3));
        currentLevel.SetCell(new Vector2Int(4, 1), Cell.TARGET1);
        currentLevel.SwitchReplaceability(new Vector2Int(1, 0));
        currentLevel.SwitchReplaceability(new Vector2Int(1, 1));
        currentLevel.SwitchReplaceability(new Vector2Int(1, 2));
        currentLevel.AddPalette(Cell.CELL1, 1);
        currentLevel.AddPalette(Cell.TARGET1, 2);
        currentRule = new Rule();
        currentRule.SetConditionCell(new Vector2Int(0, 1), Cell.CELL1);
        currentRule.SetOutcome(Cell.CELL1);
        currentRule.AddConstraint(ConstraintType.LE, Cell.CELL1, 7, 0);
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
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */
    }
}
