﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    public Cell selectedCell;
    public bool selectedReplaceability;
    public bool editMode { get; set; } // true = CellEdit, false = ReplacementEdit
    public void NewLevel()
    {
        Level level = new Level(new Vector2Int(3, 3));
        LevelManager.Inst.currentLevel = level;
        LevelManager.Inst.MapInstantiate();
    }
    
    public void LoadLevelIntoJson()
    {
        try
        {
            string str = File.ReadAllText(Application.persistentDataPath + "/CustomStage/" + GameManager.Inst.editNum.ToString() + ".json");
            Level level = JsonConvert.DeserializeObject<Level>(str);
            foreach (Rule rule in level.rules)
            {
                Constraint constraint = new Constraint();
                constraint.SetDummy();
                rule.AddConstraint(constraint);
            }
            List<CellNumPair> palette = level.palette;
            level.ResetPalette();
            foreach (CellNumPair pair in palette)
            {
                level.palette[(int)pair.cell - 3] = pair;
            }
            LevelManager.Inst.currentLevel = level;
        }
        catch (DirectoryNotFoundException e)
        {
            //Debug.Log(e);
            NewLevel();
        }
        catch (FileNotFoundException e)
        {
            //Debug.Log(e);
            NewLevel();
            //return;
        }
        LevelManager.Inst.MapInstantiate();
    }
    private void Start()
    {
        editMode = true;

        if (GameManager.Inst.back)
        {
            try
            {
                string str = File.ReadAllText(Application.persistentDataPath + "/CustomStage/test.json");
                Level level = JsonConvert.DeserializeObject<Level>(str);
                foreach (Rule rule in level.rules)
                {
                    Constraint constraint = new Constraint();
                    constraint.SetDummy();
                    rule.AddConstraint(constraint);
                }
                List<CellNumPair> palette = level.palette;
                level.ResetPalette();
                foreach (CellNumPair pair in palette)
                {
                    level.palette[(int)pair.cell - 3] = pair;
                }
                LevelManager.Inst.currentLevel = level;
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e);
                return;
            }
            LevelManager.Inst.MapInstantiate();
            //Debug.Log("Load complete.");

            GameManager.Inst.back = false;
        }
        else
        {
            LoadLevelIntoJson();
        }
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
