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

public class CellNumPair
{
    public Cell cell;
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
    public bool SetCell(Vector2Int pos, Cell type)
    {
        if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return false;
        map[pos.x, pos.y] = type;
        return true;
    }
    public bool SwitchReplaceability(Vector2Int pos)
    {
        if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return false;
        isReplaceable[pos.x, pos.y] = !isReplaceable[pos.x, pos.y];
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
    public int RuleMatchCheck(Vector2Int pos)
    {
        int matchRuleNo = -1;
        Dictionary<Cell, int> check = new Dictionary<Cell, int>();
        for (int k = 0; k < rules.Count; ++k)
        {
            check.Clear();
            bool conditionFlag = false;
            bool constraintFlag = false;

            if (!(rules[k].condition[1, 1] == map[pos.x, pos.y])) continue; // rule의 가운데 cell과 검사하는 좌표의 cell이 일치하는지 확인함

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    // map에서 x나 y의 값이 0 미만이나 최대 범위를 초과해서 map 밖으로 나갈 때는 rules[k]의 i, j이 Cell.NULL인지만 확인함
                    if (pos.x + i - 1 < 0 || pos.y + j - 1 < 0 || pos.x + i - 1 >= size.x || pos.y + j - 1 >= size.y)
                    {
                        if (rules[k].condition[i, j] == Cell.NULL)
                        {
                            if (!check.ContainsKey(Cell.NULL))
                                check.Add(Cell.NULL, 1);
                            else
                                check[Cell.NULL]++;
                        }
                        // 일치하지 않는다면 반복문을 탈출함
                        else
                        {
                            conditionFlag = true;
                            break;
                        }
                    }
                    // 정상적인 좌표 값이라면 map의 좌표와 rules[k]이 일치하는지 확인함
                    else if (map[pos.x + i - 1, pos.y + j - 1] == rules[k].condition[i, j])
                    {
                        if (!check.ContainsKey(rules[k].condition[i, j]))
                            check.Add(rules[k].condition[i, j], 1);
                        else
                            check[rules[k].condition[i, j]]++;
                    }
                    // 일치하지 않는다면 rules[k]에서 조건이 없는지(Cell.NULL인지) 확인함
                    else if (rules[k].condition[i, j] == Cell.NULL)
                    {
                        if (!check.ContainsKey(Cell.NULL))
                            check.Add(Cell.NULL, 1);
                        else
                            check[Cell.NULL]++;
                    }
                    // 일치하지 않는다면 반복문을 탈출함
                    else
                    {
                        conditionFlag = true;
                        break;
                    }
                }
                if (conditionFlag) break;
            }
            if (conditionFlag) continue; // condition이 일치하지 않았을 때
            else // condition이 일치했을 때
            {
                for (int i = 0; i < rules[k].constraints.Count; ++i) // constraint도 일치하는지 확인함
                {
                    int val = check[rules[k].constraints[i].target];
                    if (!rules[k].constraints[i].ConstraintMatches(val)) // constraint가 일치하지 않았을 때
                    {
                        constraintFlag = true;
                        break;
                    } 
                }
                if (constraintFlag) continue; // constraint가 일치하지 않았을 때
                else // 최종 일치
                {
                    matchRuleNo = k;
                    break;
                }
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

    public void PrintMap()
    {
        string str;
        Debug.Log("---");
        for (int j = 0; j < size.y; ++j)
        {
            str = "";
            for (int i = 0; i < size.x; ++i)
            {
                str += ((int)map[i, j]).ToString() + " ";
            }
            Debug.Log(str);
        }
        Debug.Log("---");
    }
}
