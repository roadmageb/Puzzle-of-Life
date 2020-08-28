using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteCellController : CellController
{
    private Transform cellPalette, cellsNum;
    public int coord { get; private set; }
    public void CellInitialize(Cell cell, int coord)
    {
        CellInitialize();
        this.coord = coord;
        ChangeCell(cell);
        ShowReplaceability(true);
    }
    public override void ChangeCell(Cell cell)
    {
        cellForeground.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellSpriteDict[cell];
        LevelManager.Inst.currentLevel.palette[coord].cell = cell;
        this.cell = cell;
    }
    public override void CellNumInitialize(int cellsNum)
    {
        this.cellsNum = transform.Find("PaletteNum");
        SetCellNumOnPalette(cellsNum);
    }
    public override void SetCellNumOnPalette(int num)
    {
        havingCellNum = num;
        SpriteRenderer units = cellsNum.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer tens = cellsNum.GetChild(1).GetComponent<SpriteRenderer>();
        units.gameObject.SetActive(true);
        tens.gameObject.SetActive(true);
        if (num == 0)
        {
            units.sprite = ImageManager.Inst.cellNumSprites[10];
            tens.gameObject.SetActive(false);
        }
        else if (num < 10)
        {
            units.sprite = ImageManager.Inst.cellNumSprites[num % 10];
            tens.gameObject.SetActive(false);
        }
        else
        {
            units.sprite = ImageManager.Inst.cellNumSprites[num % 10];
            tens.sprite = ImageManager.Inst.cellNumSprites[num / 10];
        }
    }

    protected override void OnMouseDown()
    {
        if (LevelManager.Inst.isEditorMode)
        {
            return;
        }
        if (!CheckMoveValid())
        {
            return;
        }
        if (LevelManager.Inst.GetPlayState() == PlayState.EDITTOINIT)
        {
            invalidMove = true;
            return;
        }

        cellPalette = Instantiate(cellForeground, transform).GetComponent<Transform>();
        int cellNum = havingCellNum - 1;
        invalidMove = false;
        if (cellNum < 0) // Palette에서 이 cell을 선택했을 때 남은 cell의 개수가 음수일 때
        {
            Destroy(cellPalette.gameObject);
            invalidMove = true;
            // flag를 true로 하고 return함 (이 flag를 이용하여 OnMouseDrag와 OnMouseUp event에서도 활용)
            return;
        }
        else if (cellNum == 0) // Palette에서 이 cell을 선택했을 때 남은 cell의 개수가 0일 때
        {
            cellPalette.GetComponent<SpriteRenderer>().color = Color.grey; // sprite를 어둡게 함
        }
        SetCellNumOnPalette(cellNum);

        if (!replaceability || cell == Cell.NULL || LevelManager.Inst.GetPlayState() != PlayState.EDIT)
        {
            return;
        }

        AudioManager.Inst.CellUp();
        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "SelectedCell";

        Vector3 tempVec = Input.mousePosition;
        cellForeground.position = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec));

    }
    protected override void OnMouseUp()
    {
        if (LevelManager.Inst.isEditorMode)
        {
            return;
        }
        if (!CheckMoveValid())
        {
            invalidMove = false;
            return;
        }
        Destroy(cellPalette.gameObject);

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "Cell";

        AudioManager.Inst.CellDown();
        cellForeground.localPosition = new Vector3(0, 0, 0);

        if (LevelManager.Inst.cellUnderCursor != null && LevelManager.Inst.cellUnderCursor.replaceability) // 놓을 수 있는 공간이라면
        {
            if (LevelManager.Inst.cellUnderCursor.cell != Cell.NULL) // 놓으려는 cell이 비어있지 않다면
            {
                SetCellNumOnPalette(havingCellNum + 1);
                return;
            }
            if (havingCellNum == 0)
            {
                cellForeground.GetComponent<SpriteRenderer>().color = Color.grey; // sprite를 어둡게 함
            }
            LevelManager.Inst.cellUnderCursor.ChangeCell(cell);
        }
        else // palette에서 잘못된 곳으로의 이동
        {
            SetCellNumOnPalette(havingCellNum + 1);
            return;
        }
    }

    private void OnMouseOver()
    {
        if (LevelManager.Inst.isEditorMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                havingCellNum++;
                LevelManager.Inst.currentLevel.ModifyPalette(coord, havingCellNum);
                LevelManager.Inst.MapInstantiate();
            }
            if (Input.GetMouseButtonDown(1))
            {
                havingCellNum = Mathf.Max(havingCellNum - 1, 0);
                LevelManager.Inst.currentLevel.ModifyPalette(coord, havingCellNum);
                LevelManager.Inst.MapInstantiate();
            }
        }
    }
}
