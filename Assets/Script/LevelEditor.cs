using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    public InputField[] sizeInput;
    public Dropdown cellPalette;
    public Cell selectedCell;
    public Button editModeButton;
    public int editMode; // 1 = CellEdit, 2 = ReplacementEdit
    public void MapInitialize()
    {
        LevelManager.Inst.currentLevel = new Level(new Vector2Int(int.Parse(sizeInput[0].text), int.Parse(sizeInput[1].text)));
        LevelManager.Inst.MapInstantiate();
    }
    public void SwitchEditMode()
    {
        if (editMode == 1)
        {
            editMode = 2;
            editModeButton.GetComponentInChildren<Text>().text = "Replacement Edit Mode";
        }
        else if (editMode == 2)
        {
            editMode = 1;
            editModeButton.GetComponentInChildren<Text>().text = "Cell Edit Mode";
        }
    }
    public void AddPaletteCell()
    {
        bool containFlag = false;
        for (int i = 0; i < LevelManager.Inst.currentLevel.palette.Count; ++i)
        {
            if (LevelManager.Inst.currentLevel.palette[i].cell == selectedCell)
            {
                containFlag = true;
                LevelManager.Inst.currentLevel.palette[i].num++;
                break;
            }
        }
        if (!containFlag)
        {
            LevelManager.Inst.currentLevel.AddPalette(selectedCell, 1);
        }
        LevelManager.Inst.MapInstantiate();
    }
    public void ChangeSelectedCell()
    {
        //Debug.Log((Cell)System.Enum.Parse(typeof(Cell), cellPalette.options[cellPalette.value].text));
        selectedCell = (Cell)System.Enum.Parse(typeof(Cell), cellPalette.options[cellPalette.value].text);
    }
    public Vector2Int GetCoordinate()
    {
        int x = 0, y = 0;

        bool flag = false;
        for (x = 0; x < LevelManager.Inst.currentLevel.size.x; ++x)
        {
            for (y = 0; y < LevelManager.Inst.currentLevel.size.y; ++y)
            {
                if (LevelManager.Inst.cellObject[x, y].Equals(LevelManager.Inst.cellUnderCursor))
                {
                    flag = true;
                    break;
                }
            }
            if (flag) break;
        }

        if (!flag) return new Vector2Int(-1, -1);
        else return new Vector2Int(x, y);
    }
    private void Start()
    {
        editMode = 1;
        cellPalette.options.Clear();
        foreach (Cell cell in Enum.GetValues(typeof(Cell)))
        {
            cellPalette.options.Add(new Dropdown.OptionData() { text = cell.ToString() });
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (LevelManager.Inst.cellUnderCursor != null && !Equals(LevelManager.Inst.cellUnderCursor.parentName, "Palette"))
            {
                Vector2Int coord = GetCoordinate();
                if (editMode == 1)
                {
                    LevelManager.Inst.currentLevel.SetCell(new Vector2Int(coord.x, coord.y), selectedCell);
                }
                else if (editMode == 2)
                {
                    LevelManager.Inst.currentLevel.SwitchReplaceability(new Vector2Int(coord.x, coord.y));
                }
                LevelManager.Inst.MapInstantiate();
                Debug.Log(LevelManager.Inst.currentLevel.map[coord.x, coord.y]);
            }
        }
}
}
