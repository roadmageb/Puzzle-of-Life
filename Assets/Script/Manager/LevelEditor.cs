﻿using Newtonsoft.Json;
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
    public InputField stageName;
    public Dropdown cellPalette, constraintType;
    public Toggle constraintReplaceability;
    public Cell selectedCell;
    public bool selectedReplaceability;
    public ConstraintType selectedConstraintType;
    public Button editModeButton;
    public bool editMode { get; set; } // true = CellEdit, false = ReplacementEdit
    public void NewLevel()
    {
        Level level = new Level(new Vector2Int(3, 3));
        LevelManager.Inst.currentLevel = level;
        LevelManager.Inst.MapInstantiate();
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
        editMode = true;
        LevelManager.Inst.currentLevel = new Level(new Vector2Int(3, 3));
        LevelManager.Inst.MapInstantiate();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (LevelManager.Inst.cellUnderCursor != null)
            {
                if (editMode)
                {
                    if (LevelManager.Inst.cellUnderCursor is MapCellController)
                    {
                        if (selectedCell == Cell.EMPTY || selectedCell == Cell.ANY) return;
                        Vector2Int coord = ((MapCellController)LevelManager.Inst.cellUnderCursor).coord;
                        LevelManager.Inst.currentLevel.SetCell(coord, selectedCell);
                    }
                    else if (LevelManager.Inst.cellUnderCursor is RuleCellController)
                    {
                        int ruleNum = ((RuleCellController)LevelManager.Inst.cellUnderCursor).ruleNum;
                        Vector2Int coord = ((RuleCellController)LevelManager.Inst.cellUnderCursor).coord;
                        int constraintNum = ((RuleCellController)LevelManager.Inst.cellUnderCursor).constraintNum;

                        if (coord.x != -1 && coord.y != -1)
                        {
                            if ((coord.x == 1 && coord.y == 1) && (selectedCell == Cell.EMPTY || selectedCell == Cell.ANY)) return;
                            if (selectedCell == Cell.EMPTY || selectedCell == Cell.ANY)
                            {
                                LevelManager.Inst.currentLevel.rules[ruleNum].ChangeReplaceability(coord, false);
                            }
                            LevelManager.Inst.currentLevel.rules[ruleNum].SetConditionCell(coord, selectedCell);
                        }
                        else if (constraintNum == -1)
                        {
                            if (selectedCell == Cell.EMPTY || selectedCell == Cell.ANY) return;
                            LevelManager.Inst.currentLevel.rules[ruleNum].SetOutcome(selectedCell);
                        }
                        else
                        {
                            if (selectedCell == Cell.EMPTY || selectedCell == Cell.ANY)
                            {
                                LevelManager.Inst.currentLevel.rules[ruleNum].ChangeReplaceability(coord, false);
                            }
                            LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].target = selectedCell;
                        }
                    }
                }
                else if (!editMode)
                {
                    if (LevelManager.Inst.cellUnderCursor is MapCellController)
                    {
                        Vector2Int coord = ((MapCellController)LevelManager.Inst.cellUnderCursor).coord;
                        LevelManager.Inst.currentLevel.ChangeReplaceability(coord, selectedReplaceability);
                    }
                    else if (LevelManager.Inst.cellUnderCursor is RuleCellController)
                    {
                        int ruleNum = ((RuleCellController)LevelManager.Inst.cellUnderCursor).ruleNum;
                        Vector2Int coord = ((RuleCellController)LevelManager.Inst.cellUnderCursor).coord;
                        int constraintNum = ((RuleCellController)LevelManager.Inst.cellUnderCursor).constraintNum;

                        if (coord.x != -1 && coord.y != -1)
                        {
                            LevelManager.Inst.currentLevel.rules[ruleNum].ChangeReplaceability(coord, selectedReplaceability);
                        }
                        else if (constraintNum == -1)
                        {
                            LevelManager.Inst.currentLevel.rules[ruleNum].ChangeOutcomeReplaceability(selectedReplaceability);
                        }
                        else
                        {
                            LevelManager.Inst.currentLevel.rules[ruleNum].constraints[constraintNum].SetReplaceability(selectedReplaceability);
                        }
                    }
                }
                LevelManager.Inst.MapInstantiate();
            }
        }
}
}
