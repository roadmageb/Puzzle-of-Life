using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Jobs;

[Serializable]
public class CellNumPair
{
    public Cell cell { get; set; }
    public int num;
    public CellNumPair(Cell cell, int num)
    {
        this.cell = cell;
        this.num = num;
    }
}

public class Level
{
    public Vector2Int size;
    public Cell[,] map;
    public bool[,] isReplaceable;
    public List<CellNumPair> palette;
    public List<Rule> rules;
    public Level(Vector2Int size)
    {
        this.size = size;
        map = new Cell[size.x, size.y];
        isReplaceable = new bool[size.x, size.y];
        palette = new List<CellNumPair>();
        rules = new List<Rule>();

        for (int i = 0; i < size.x; ++i)
        {
            for (int j = 0; j < size.y; ++j)
            {
                map[i, j] = Cell.NULL;
                isReplaceable[i, j] = false;
            }
        }
    }
    public bool SetCell(Vector2Int coord, Cell type)
    {
        if (coord.x >= size.x || coord.y >= size.y || coord.x < 0 || coord.y < 0) return false;
        map[coord.x, coord.y] = type;
        return true;
    }
    public bool SwitchReplaceability(Vector2Int coord)
    {
        if (coord.x >= size.x || coord.y >= size.y || coord.x < 0 || coord.y < 0) return false;
        isReplaceable[coord.x, coord.y] = !isReplaceable[coord.x, coord.y];
        return true;
    }
    public void AddPalette(Cell type, int num)
    {
        palette.Add(new CellNumPair(type, num));
    }
    public void RemovePalette(int index)
    {
        if (index < palette.Count)
        {
            palette.RemoveAt(index);
        }
    }
    public void AddRule(Rule rule)
    {
        rules.Add(rule);
    }
    public void RemoveRule(int index)
    {
        if (index < rules.Count)
        {
            rules.RemoveAt(index);
        }
    }
    public int RuleMatchCheck(Vector2Int coord)
    {
        int matchRuleNo = -1;
        for (int k = 0; k < rules.Count; ++k)
        {
            Dictionary<Cell, int> check = new Dictionary<Cell, int>();
            Dictionary<Cell, bool> checkBool = new Dictionary<Cell, bool>();
            Dictionary<Cell, int> count = new Dictionary<Cell, int>();
            bool constraintFlag = false;

            if (rules[k].condition[1, 1] == Cell.ANY && map[coord.x, coord.y] == Cell.NULL) continue;
            else if (rules[k].condition[1, 1] != map[coord.x, coord.y]) continue;

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if ((i == 1 && j == 1) || rules[k].condition[i, j] == Cell.NULL) continue;

                    // check, checkBool, count dict 초기화
                    if (!check.ContainsKey(rules[k].condition[i, j]))
                    {
                        check.Add(rules[k].condition[i, j], 0);
                        checkBool.Add(rules[k].condition[i, j], false);
                        count.Add(rules[k].condition[i, j], 0);
                    }
                    count[rules[k].condition[i, j]]++;

                    if (coord.x + i - 1 < 0 || coord.y + j - 1 < 0 || coord.x + i - 1 >= size.x || coord.y + j - 1 >= size.y)
                    {
                        continue;
                    }
                    // 정상적인 좌표 값이라면 map의 좌표와 rules[k]이 일치하는지 확인함
                    else if ((map[coord.x + i - 1, coord.y + j - 1] == rules[k].condition[i, j])
                        || (rules[k].condition[i, j] == Cell.ANY && map[coord.x + i - 1, coord.y + j - 1] != Cell.NULL)
                        || (rules[k].condition[i, j] == Cell.EMPTY && map[coord.x + i - 1, coord.y + j - 1] == Cell.NULL))
                    {
                        check[rules[k].condition[i, j]]++;
                    }
                }
            }

            for (int i = 0; i < rules[k].constraints.Count; ++i) // constraint도 일치하는지 확인함
            {
                int val = check[rules[k].constraints[i].target];
                if (rules[k].constraints[i].ConstraintMatches(val))
                {
                    checkBool[rules[k].constraints[i].target] = true;
                }
                else
                {
                    constraintFlag = true;
                    break;
                }
            }

            if (constraintFlag) continue;

            foreach (KeyValuePair<Cell, bool> items in checkBool)
            {
                if (!checkBool[items.Key])
                {
                    if (check[items.Key] != count[items.Key]) constraintFlag = true;
                }
            }

            if (constraintFlag) continue;
            else
            {
                matchRuleNo = k;
                break;
            }
        }
        return matchRuleNo; // 해당하는 rule이 없으면 -1을, 있으면 해당하는 rule의 번호를 반환
    }
    public void NextState()
    {
        int matchRuleNo;
        // 다음 state로 이동하기 전에 현재 state의 map을 저장함
        Cell[,] tempMap = new Cell[size.x, size.y];
        for (int i = 0; i < size.x; ++i)
        {
            for (int j = 0; j < size.y; ++j)
            {
                tempMap[i, j] = map[i, j];
            }
        }

        for (int i = 0; i < size.x; ++i)
        {
            for (int j = 0; j < size.y; ++j)
            {
                matchRuleNo = RuleMatchCheck(new Vector2Int(i, j));
                if (matchRuleNo == -1) continue;
                else
                {
                    tempMap[i, j] = rules[matchRuleNo].outcome;
                }
            }
        }

        for (int i = 0; i < size.x; ++i)
        {
            for (int j = 0; j < size.y; ++j)
            {
                map[i, j] = tempMap[i, j];
            }
        }
    }

    public bool ClearCheck()
    {
        for (int i = 0; i < size.x; ++i)
        {
            for (int j = 0; j < size.x; ++j)
            {
                if (map[i, j] == Cell.TARGET1 || map[i, j] == Cell.TARGET2 || map[i, j] == Cell.TARGET3) return false;
            }
        }
        return true;
    }
}
