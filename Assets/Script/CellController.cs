using Packages.Rider.Editor.Util;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public Transform cellForeground, cellReplaceable, cellSelected;
    public Transform cellPalette;
    public Transform cellsNum;
    public int havingCellNum { get; set; }
    public bool invalidPaletteMove;
    public string parentName;
    public Cell cell;
    public bool replaceability;
    public void CellInitialize(Cell cell, string parentName)
    {
        cellForeground = transform.Find("CellForeground");
        cellSelected = transform.Find("CellSelected");
        ChangeSpriteByCell(cell);
        this.parentName = parentName;
        replaceability = true;
        invalidPaletteMove = false;
    }
    public void CellInitialize(Cell cell, bool replaceability, string parentName)
    {
        CellInitialize(cell, parentName);
        cellReplaceable = transform.Find("CellReplaceable");
        ShowReplaceability(replaceability);
    }
    public void CellNumInitialize(int cellsNum)
    {
        this.cellsNum = transform.Find("PaletteNum");
        SetCellNumOnPalette(cellsNum);
    }
    public void ChangeSpriteByCell(Cell cell)
    {
        cellForeground.GetComponent<SpriteRenderer>().sprite = ImageManager.Inst.cellSpriteDict[cell];
        this.cell = cell;
    }
    public void ShowReplaceability(bool replaceability)
    {
        cellReplaceable.gameObject.SetActive(replaceability);
        this.replaceability = replaceability;
    }
    public void ShowSelection(bool selected)
    {
        cellSelected.gameObject.SetActive(selected);
    }
    private void OnMouseEnter()
    {
        ShowSelection(true);
        LevelManager.Inst.cellUnderCursor = this;
    }
    private void OnMouseExit()
    {
        ShowSelection(false);
        LevelManager.Inst.cellUnderCursor = null;
    }
    public void SetCellNumOnPalette(int num)
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
    private void OnMouseDown()
    {
        // 현재 선택한 cell이 Palette에 속한 것인지를 먼저 검사함
        // (replaceability와 cell == Cell.NULL 조건과 충돌하지 않음)
        // 그렇게 해서 도출한 invalidPaletteMove를 통하여 이후에 valid한 OnMouseDown이 호출되었는지 검사함
        if (string.Equals(parentName, "Palette"))
        {
            cellPalette = Instantiate(cellForeground, transform).GetComponent<Transform>();
            int cellNum = havingCellNum - 1;
            invalidPaletteMove = false;
            if (cellNum < 0) // Palette에서 이 cell을 선택했을 때 남은 cell의 개수가 음수일 때
            {
                Destroy(cellPalette.gameObject);
                invalidPaletteMove = true;
                // flag를 true로 하고 return함 (이 flag를 이용하여 OnMouseDrag와 OnMouseUp event에서도 활용)
                return;
            }
            else if (cellNum == 0) // Palette에서 이 cell을 선택했을 때 남은 cell의 개수가 0일 때
            {
                cellPalette.GetComponent<SpriteRenderer>().color = Color.grey; // sprite를 어둡게 함
            }
            SetCellNumOnPalette(cellNum);
        }

        if (!replaceability || cell == Cell.NULL)
        {
            return;
        }

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "SelectedCell";

        Vector3 tempVec = Input.mousePosition;
        cellForeground.position = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec));

    }
    private void OnMouseUp()
    {
        if (!replaceability || cell == Cell.NULL || invalidPaletteMove)
        {
            return;
        }
        if (string.Equals(parentName, "Palette"))
        {
            Destroy(cellPalette.gameObject);
        }

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "Cell";

        cellForeground.localPosition = new Vector3(0, 0, 0);

        if (LevelManager.Inst.cellUnderCursor != null && LevelManager.Inst.cellUnderCursor.replaceability) // 놓을 수 있는 공간이라면
            // 지금 버그: 놓을수 없는 공간일 때 palette 개수 복구를 안해줌
        {
            if (string.Equals(parentName, "Palette")) // palette -> any로의 이동
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
                LevelManager.Inst.cellUnderCursor.ChangeSpriteByCell(cell);
            }
            else // !palette -> any로의 이동
            {
                if (string.Equals(LevelManager.Inst.cellUnderCursor.parentName, "Palette")) // !palette -> palette로의 이동
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
        else if (string.Equals(parentName, "Palette")) // palette에서 잘못된 곳으로의 이동
        {
            SetCellNumOnPalette(havingCellNum + 1);
            return;
        }
    }
    private void OnMouseDrag()
    {
        if (!replaceability || cell == Cell.NULL || invalidPaletteMove)
        {
            return;
        }

        Vector3 tempVec = Input.mousePosition;
        cellForeground.position = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec));
    }
}
