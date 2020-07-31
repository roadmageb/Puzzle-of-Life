using Packages.Rider.Editor.Util;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MapCellController : CellController
{
    public Vector2Int coord { get; private set; }
    public void CellInitialize(Cell cell, bool replaceability, Vector2Int coord)
    {
        CellInitialize();
        this.coord = coord;
        ChangeCell(cell);
        ShowReplaceability(replaceability);
    }
    public override void ChangeCell(Cell cell)
    {
        cellForeground.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellSpriteDict[cell];
        LevelManager.Inst.currentLevel.SetCell(coord, cell);
        this.cell = cell;
    }
    protected override void OnMouseDown()
    {
        if (!replaceability || cell == Cell.NULL || LevelManager.Inst.GetPlayState() != PlayState.EDIT)
        {
            return;
        }

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "SelectedCell";

        Vector3 tempVec = Input.mousePosition;
        cellForeground.position = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec));
    }
    protected override void OnMouseUp()
    {
        if (!replaceability || cell == Cell.NULL || LevelManager.Inst.GetPlayState() != PlayState.EDIT || invalidPaletteMove)
        {
            return;
        }

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "Cell";

        cellForeground.localPosition = new Vector3(0, 0, 0);

        if (LevelManager.Inst.cellUnderCursor != null && LevelManager.Inst.cellUnderCursor.replaceability)
        {
            if (LevelManager.Inst.cellUnderCursor is PaletteCellController) // !palette -> palette로의 이동
            {
                if (cell == LevelManager.Inst.cellUnderCursor.cell) // !palette의 cell과 palette의 cell type이 일치한다면
                {
                    LevelManager.Inst.cellUnderCursor.SetCellNumOnPalette(LevelManager.Inst.cellUnderCursor.havingCellNum + 1);
                    LevelManager.Inst.cellUnderCursor.cellForeground.GetComponent<SpriteRenderer>().color = Color.white;
                    ChangeCell(Cell.NULL);
                }
                else
                {
                    return;
                }
            }
            else // !palette -> !palette로의 이동
            {
                Cell tempCell = cell;
                ChangeCell(LevelManager.Inst.cellUnderCursor.cell);
                LevelManager.Inst.cellUnderCursor.ChangeCell(tempCell);
            }
        }
    }
}
