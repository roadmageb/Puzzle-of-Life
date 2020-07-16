using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
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
            for (int j = 0; j < size.y; ++j)
            {
                map[i, j] = Cell.NULL;
                isReplaceable[i, j] = false;
            }
    }
    public bool SetCell(Vector2Int pos, Cell type)
    {
        if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0)
            return false;
        map[pos.x, pos.y] = type;
        return true;
    }
    public bool SwitchReplaceability(Vector2Int pos)
    {
        if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0)
            return false;
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
            palette.RemoveAt(index);
    }
    public void AddRule(Rule rule)
    {
        rules.Add(rule);
    }
    public void RemoveRule(int index)
    {
        if (index < rules.Count)
            rules.RemoveAt(index);
    }
    public bool RuleMatchCheck(Vector2Int pos)
    {
        Dictionary<Cell, int> check = new Dictionary<Cell, int>();
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
            {
                if (pos.x + i - 1 < 0 || pos.y + j - 1 < 0)
                    continue;

                //for (int k = 0; k < rules.)

                //if (map[pos.x + i - 1] == 
            }
        return false;
    }
    public void NextState()
    {
        for (int i = 0; i < size.x; ++i)
            for (int j = 0; j < size.y; ++j)
            {

            }
    }
}
