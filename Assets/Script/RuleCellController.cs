using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleCellController : CellController
{
    int ruleNum, constraintNum;
    Vector2Int coord;
    public void CellInitialize(Cell cell, bool replaceability, int ruleNum) // for outcome cell
    {
        CellInitialize(cell, CellControllerType.RULE, replaceability);
        this.ruleNum = ruleNum;
        constraintNum = -1;
        coord = new Vector2Int(-1, -1);
    }
    public void CellInitialize(Cell cell, bool replaceability, int ruleNum, Vector2Int coord) // for condition cell
    {
        CellInitialize(cell, replaceability, ruleNum);
        this.coord = coord;
    }
    public void CellInitialize(Cell cell, bool replaceability, int ruleNum, int constraintNum) // for constraint cell
    {
        CellInitialize(cell, replaceability, ruleNum);
        this.constraintNum = constraintNum;
    }
    public override void ChangeSpriteByCell(Cell cell)
    {
        cellForeground.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellSpriteDict[cell];
        if (coord.x != -1 && coord.y != -1) {
            LevelManager.Inst.currentLevel.rules[ruleNum].condition[coord.x, coord.y] = cell;
        }
        else if (constraintNum == -1)
        {
            LevelManager.Inst.currentLevel.rules[ruleNum].outcome = cell;
        }
        else
        {
            LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].target = cell;
        }
        this.cell = cell;
    }
    protected override void OnMouseDown()
    {
        if (!replaceability || cell == Cell.NULL)
        {
            return;
        }

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "SelectedCell";

        Vector3 tempVec = Input.mousePosition;
        cellForeground.position = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec));
    }
    protected override void OnMouseUp()
    {
        if (!replaceability || cell == Cell.NULL || invalidPaletteMove)
        {
            return;
        }

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "Cell";

        cellForeground.localPosition = new Vector3(0, 0, 0);

        if (LevelManager.Inst.cellUnderCursor != null && LevelManager.Inst.cellUnderCursor.replaceability) // 놓을 수 있는 공간이라면
        {
            if (LevelManager.Inst.cellUnderCursor.cellControllerType == CellControllerType.PALETTE) // !palette -> palette로의 이동
            {
                if (cell == LevelManager.Inst.cellUnderCursor.cell) // !palette의 cell과 palette의 cell type이 일치한다면
                {
                    LevelManager.Inst.cellUnderCursor.SetCellNumOnPalette(LevelManager.Inst.cellUnderCursor.havingCellNum + 1);
                    LevelManager.Inst.cellUnderCursor.cellForeground.GetComponent<SpriteRenderer>().color = Color.white;
                    ChangeSpriteByCell(Cell.NULL);
                }
                else
                {
                    return;
                }
            }
            else // !palette -> !palette로의 이동
            {
                Cell tempCell = cell;
                ChangeSpriteByCell(LevelManager.Inst.cellUnderCursor.cell);
                LevelManager.Inst.cellUnderCursor.ChangeSpriteByCell(tempCell);
            }
        }
    }
}
