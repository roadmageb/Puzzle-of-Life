using Packages.Rider.Editor.Util;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class CellController : MonoBehaviour
{
    public Transform cellForeground, cellReplaceable, cellSelected;
    protected bool invalidPaletteMove;
    public Cell cell { get; protected set; }
    public bool replaceability { get; private set; }
    public int havingCellNum { get; protected set; }
    public void CellInitialize()
    {
        invalidPaletteMove = false;
        havingCellNum = 0;
    }
    public abstract void ChangeSpriteByCell(Cell cell);
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
    protected abstract void OnMouseDown();
    protected abstract void OnMouseUp();
    protected void OnMouseDrag()
    {
        if (!replaceability || cell == Cell.NULL || LevelManager.Inst.playState != PlayState.EDIT || invalidPaletteMove)
        {
            return;
        }

        Vector3 tempVec = Input.mousePosition;
        cellForeground.position = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec));
    }
}
