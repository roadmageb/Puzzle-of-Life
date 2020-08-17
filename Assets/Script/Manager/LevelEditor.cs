using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    public InputField[] sizeInput, constraintInput;
    public InputField stageName;
    public Dropdown cellPalette, constraintType;
    public Toggle constraintReplaceability;
    public Cell selectedCell;
    public ConstraintType selectedConstraintType;
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
    public void DeletePaletteCell()
    {
        if (LevelManager.Inst.currentLevel.palette.Count - 1 < 0) return;
        LevelManager.Inst.currentLevel.RemovePalette(LevelManager.Inst.currentLevel.palette.Count - 1);
        LevelManager.Inst.MapInstantiate();
    }
    public void ChangeSelectedCell()
    {
        selectedCell = (Cell)System.Enum.Parse(typeof(Cell), cellPalette.options[cellPalette.value].text);
    }
    public void LoadLevelIntoJson()
    {
        try
        {
            string str = File.ReadAllText(Application.dataPath + "/Resources/" + stageName.text + ".json");
            Level level = JsonConvert.DeserializeObject<Level>(str);
            foreach (Rule rule in level.rules)
            {
                Constraint constraint = new Constraint();
                constraint.SetDummy();
                rule.AddConstraint(constraint);
            }
            LevelManager.Inst.currentLevel = level;
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e);
            return;
        }
        LevelManager.Inst.MapInstantiate();
        Debug.Log("Load complete.");
    }
    public void SaveLevelIntoJson()
    {
        Level level = LevelManager.Inst.currentLevel;
        foreach (Rule rule in level.rules)
        {
            rule.RemoveConstraint(rule.constraints.Count - 1);
        }
        string levelstr = JsonConvert.SerializeObject(level);
        File.WriteAllText(Application.dataPath + "/Resources/" + stageName.text + ".json", levelstr);
        Debug.Log("Save complete.");
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
            if (LevelManager.Inst.cellUnderCursor != null)
            {
                if (editMode == 1)
                {
                    if (LevelManager.Inst.cellUnderCursor is MapCellController)
                    {
                        Vector2Int coord = ((MapCellController)LevelManager.Inst.cellUnderCursor).coord;
                        //Debug.Log(coord);
                        LevelManager.Inst.currentLevel.SetCell(coord, selectedCell);
                    }
                    else if (LevelManager.Inst.cellUnderCursor is RuleCellController)
                    {
                        int ruleNum = ((RuleCellController)LevelManager.Inst.cellUnderCursor).ruleNum;
                        Vector2Int coord = ((RuleCellController)LevelManager.Inst.cellUnderCursor).coord;
                        int constraintNum = ((RuleCellController)LevelManager.Inst.cellUnderCursor).constraintNum;

                        if (coord.x != -1 && coord.y != -1)
                        {
                            //Debug.Log(coord);
                            LevelManager.Inst.currentLevel.rules[ruleNum].SetConditionCell(coord, selectedCell);
                        }
                        else if (constraintNum == -1)
                        {
                            LevelManager.Inst.currentLevel.rules[ruleNum].SetOutcome(selectedCell);
                        }
                        else
                        {
                            LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].target = selectedCell;
                        }
                    }
                }
                else if (editMode == 2)
                {
                    if (LevelManager.Inst.cellUnderCursor is MapCellController)
                    {
                        Vector2Int coord = ((MapCellController)LevelManager.Inst.cellUnderCursor).coord;
                        LevelManager.Inst.currentLevel.SwitchReplaceability(coord);
                    }
                    else if (LevelManager.Inst.cellUnderCursor is RuleCellController)
                    {
                        int ruleNum = ((RuleCellController)LevelManager.Inst.cellUnderCursor).ruleNum;
                        Vector2Int coord = ((RuleCellController)LevelManager.Inst.cellUnderCursor).coord;
                        int constraintNum = ((RuleCellController)LevelManager.Inst.cellUnderCursor).constraintNum;

                        if (coord.x != -1 && coord.y != -1)
                        {
                            LevelManager.Inst.currentLevel.rules[ruleNum].SwitchReplaceability(coord);
                        }
                        else if (constraintNum == -1)
                        {
                            LevelManager.Inst.currentLevel.rules[ruleNum].SwitchOutcomeReplaceability();
                        }
                        else
                        {
                            LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum]
                                .SetReplaceability(!LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].isReplaceable);
                        }
                    }
                }
                LevelManager.Inst.MapInstantiate();
            }
        }
}
}
