//using JetBrains.Annotations;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[Serializable]
//public class Serialization<T>
//{
//    [SerializeField]
//    List<T> target;
//    public List<T> ToList() { return target; }

//    public Serialization(List<T> target)
//    {
//        this.target = target;
//    }
//}

//[Serializable]
//public class CellNumPairForJson
//{
//    public int cell;
//    public int num;
//    public CellNumPairForJson(int cell, int num)
//    {
//        this.cell = cell;
//        this.num = num;
//    }
//}

//[Serializable]
//public class ConstraintForJson
//{
//    public int type;
//    public int target;
//    public bool isReplaceable;
//    public int param1, param2;
//    public ConstraintForJson(Constraint constraint)
//    {
//        type = (int)constraint.type;
//        target = (int)constraint.target;
//        isReplaceable = constraint.isReplaceable;
//        param1 = constraint.param1;
//        param2 = constraint.param2;
//    }
//}

//[Serializable]
//public class RuleForJson
//{
//    public List<List<int>> condition;
//    public List<List<bool>> isReplaceable;
//    public int outcome;
//    public bool isOutcomeReplabeable;
//    public List<ConstraintForJson> constraints;
//    public RuleForJson(Rule rule)
//    {
//        condition = new List<List<int>>();
//        isReplaceable = new List<List<bool>>();
//        constraints = new List<ConstraintForJson>();
//        outcome = (int)rule.outcome;
//        isOutcomeReplabeable = rule.isOutcomeReplaceable;
//    }
//    public void InitializeCondition(Rule rule)
//    {
//        for (int i = 0; i < 3; ++i)
//        {
//            List<int> temp = new List<int>();
//            for (int j = 0; j < 3; ++j)
//            {
//                temp.Add((int)rule.condition[i, j]);
//            }
//            condition.Add(temp);
//        }
//    }
//    public void InitializeIsReplaceable(Rule rule)
//    {
//        for (int i = 0; i < 3; ++i)
//        {
//            List<bool> temp = new List<bool>();
//            for (int j = 0; j < 3; ++j)
//            {
//                temp.Add(rule.isReplaceable[i, j]);
//            }
//            isReplaceable.Add(temp);
//        }
//    }
//    public void InitializeConstraints(Rule rule)
//    {
//        for (int i = 0; i < rule.constraints.Count; ++i)
//        {
//            ConstraintForJson temp = new ConstraintForJson(rule.constraints[i]);
//            constraints.Add(temp);
//        }
//    }
//}

//[Serializable]
//public class LevelForJson
//{ 
//    public Vector2Int size;
//    public List<List<int>> map;
//    public List<List<bool>> isReplaceable;
//    public List<CellNumPairForJson> palette;
//    public List<RuleForJson> rules;
//    public LevelForJson(Level level)
//    {
//        map = new List<List<int>>();
//        isReplaceable = new List<List<bool>>();
//        palette = new List<CellNumPairForJson>();
//        rules = new List<RuleForJson>();
//        size = level.size;
//        InitializeMap(level);
//        InitializeIsReplaceable(level);
//        InitializePalette(level);
//        InitializeRules(level);
//    }
//    public void InitializeMap(Level level)
//    {
//        for (int i = 0; i < size.x; ++i)
//        {
//            List<int> temp = new List<int>();
//            for (int j = 0; j < size.y; ++j)
//            {
//                temp.Add((int)level.map[i, j]);
//            }
//            map.Add(temp);
//        }
//    }
//    public void InitializeIsReplaceable(Level level)
//    {
//        for (int i = 0; i < size.x; ++i)
//        {
//            List<bool> temp = new List<bool>();
//            for (int j = 0; j < size.y; ++j)
//            {
//                temp.Add(level.isReplaceable[i, j]);
//            }
//            isReplaceable.Add(temp);
//        }
//    }
//    public void InitializePalette(Level level)
//    {
//        for (int i = 0; i < level.palette.Count; ++i)
//        {
//            CellNumPairForJson temp = new CellNumPairForJson((int)level.palette[i].cell, level.palette[i].num);
//            palette.Add(temp);
//        }
//    }
//    public void InitializeRules(Level level)
//    {
//        for (int i = 0; i < level.rules.Count; ++i)
//        {
//            RuleForJson temp = new RuleForJson(level.rules[i]);
//            rules.Add(temp);
//        }
//    }
//}
