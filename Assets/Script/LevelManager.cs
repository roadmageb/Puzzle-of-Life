using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool isPlaymode = false;
    public Level currentLevel;
    public Cell[,] previousCells;

    public void PlayLevel()
    {
        previousCells = new Cell[currentLevel.size.x, currentLevel.size.y];
        for (int i = 0; i < currentLevel.size.x; ++i)
            for (int j = 0; j < currentLevel.size.y; ++j)
                previousCells[i, j] = currentLevel.map[i, j];
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
        currentLevel.AddRule(currentRule);

        currentLevel.PrintMap();

        currentLevel.NextState();

        currentLevel.PrintMap();

        currentLevel.NextState();

        currentLevel.PrintMap();

        currentLevel.NextState();

        currentLevel.PrintMap();

        currentLevel.NextState();

        currentLevel.PrintMap();

        currentLevel.NextState();

        currentLevel.PrintMap();

        currentLevel.NextState();

        currentLevel.PrintMap();

        currentLevel.NextState();

        currentLevel.PrintMap();

        currentLevel.NextState();

        currentLevel.PrintMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
