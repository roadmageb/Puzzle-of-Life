using Packages.Rider.Editor.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public Transform cellForeground, cellReplaceable, cellSelected;
    public string parentName;
    public Vector3 myOrigin, parentOrigin;
    public Cell cell;
    public bool replaceability;
    public void CellInitialize(Cell cell, bool replaceability, string parentName)
    {
        cellForeground = transform.GetChild(0);
        cellReplaceable = transform.GetChild(1);
        cellSelected = transform.GetChild(2);
        ChangeSprite(cell);
        ShowReplaceability(replaceability);
        myOrigin = transform.localPosition;
        parentOrigin = transform.parent.position;
        this.parentName = parentName;
    }
    public void ChangeSprite(Cell cell)
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
        if (replaceability)
        {
            LevelManager.Inst.cellUnderCursor = this;
        }
    }
    private void OnMouseExit()
    {
        ShowSelection(false);
        LevelManager.Inst.cellUnderCursor = null;
    }
    private void OnMouseDown()
    {
        if (!replaceability || cell == Cell.NULL)
        {
            return;
        }

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "SelectedCell";

        Vector3 tempVec = Input.mousePosition;
        cellForeground.localPosition = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec) - parentOrigin - myOrigin);

    }
    private void OnMouseUp()
    {
        if (!replaceability || cell == Cell.NULL)
        {
            return;
        }

        cellForeground.GetComponent<SpriteRenderer>().sortingLayerName = "Cell";

        cellForeground.localPosition = new Vector3(0, 0, 0);
        if (LevelManager.Inst.cellUnderCursor != null && replaceability) // 놓을 수 있는 공간이라면
        {
            Cell tempCell = cell;
            ChangeSprite(LevelManager.Inst.cellUnderCursor.cell);
            LevelManager.Inst.cellUnderCursor.ChangeSprite(tempCell);
        }
    }
    private void OnMouseDrag()
    {
        if (!replaceability || cell == Cell.NULL)
        {
            return;
        }
        else
        {
            Vector3 tempVec = Input.mousePosition;
            cellForeground.localPosition = (Vector2)(Camera.main.ScreenToWorldPoint(tempVec) - parentOrigin - myOrigin);
        }
    }
}
