using Packages.Rider.Editor.Util;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class CellController : MonoBehaviour
{
    public Transform cellForeground, cellReplaceable, cellSelected;
    public Sprite selectedEnabled, selectedDisabled;
    protected bool invalidMove;
    public Cell cell { get; protected set; }
    public bool replaceability { get; private set; }
    public int havingCellNum { get; protected set; }
    public void CellInitialize()
    {
        invalidMove = false;
        havingCellNum = 0;
    }
    public abstract void ChangeCell(Cell cell);
    public void ShowReplaceability(bool replaceability)
    {
        cellReplaceable.gameObject.SetActive(replaceability);
        this.replaceability = replaceability;
    }
    public void ShowSelection(bool selected)
    {
        cellSelected.gameObject.SetActive(selected);
    }
    public virtual void CellNumInitialize(int cellsNum)
    {
        return;
    }
    public virtual void SetCellNumOnPalette(int num)
    {
        return;
    }

    public void ChangeCellSelectionBorder()
    {
        if (LevelManager.Inst.GetPlayState() == PlayState.PLAY || LevelManager.Inst.GetPlayState() == PlayState.PLAYFRAME)
        {
            cellSelected.GetComponent<SpriteRenderer>().sprite = selectedDisabled;
        }
        else if (replaceability) {
            cellSelected.GetComponent<SpriteRenderer>().sprite = selectedEnabled;
        }
        else if (!replaceability) {
            cellSelected.GetComponent<SpriteRenderer>().sprite = selectedDisabled;
        }
    }

    protected bool CheckMoveValid()
    {
        if (!replaceability || cell == Cell.NULL || LevelManager.Inst.GetPlayState() != PlayState.EDIT || invalidMove)
        {
            invalidMove = true;
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnMouseEnter()
    {
        ShowSelection(true);
        ChangeCellSelectionBorder();
        LevelManager.Inst.cellUnderCursor = this;
    }
    private void OnMouseExit()
    {
        ShowSelection(false);
        LevelManager.Inst.cellUnderCursor = null;
    }
    protected abstract void OnMouseDown();
    protected abstract void OnMouseUp();
    protected void OnMouseDrag()
    {
        if (!CheckMoveValid())
        {
            return;
        }

        Vector3 tempVec = Input.mousePosition;
        cellForeground.position = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec));
    }
}
