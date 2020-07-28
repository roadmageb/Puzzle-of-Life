using Packages.Rider.Editor.Util;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MapCellController : CellController
{
    Vector2Int coord;
    public void CellInitialize(Cell cell, bool replaceability, Vector2Int coord)
    {
        CellInitialize(cell, CellControllerType.MAP, replaceability);
        this.coord = coord;
    }
    public override void ChangeSpriteByCell(Cell cell)
    {
        cellForeground.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellSpriteDict[cell];
        LevelManager.Inst.currentLevel.map[coord.x, coord.y] = cell;
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

        if (LevelManager.Inst.cellUnderCursor != null && LevelManager.Inst.cellUnderCursor.replaceability)
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
