using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    public Cell[,] condition;
    public bool[,] isReplaceable;
    public Cell outcome;
    public bool isOutcomeReplaceable;
    public List<Constraint> constraints;
    public Rule()
    {
        condition = new Cell[3, 3];
        isReplaceable = new bool[3, 3];
        constraints = new List<Constraint>();

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                condition[i, j] = Cell.NULL;
                isReplaceable[i, j] = false;
            }
        }
        isOutcomeReplaceable = false;
    }
    public bool SetConditionCell(Vector2Int pos, Cell type)
    {
        if (pos.x >= 3 || pos.y >= 3 || pos.x < 0 || pos.y < 0) return false;
        condition[pos.x, pos.y] = type;
        return true;
    }
    public void SetOutcome(Cell type)
    {
        outcome = type;
    }
    public void AddConstraint(ConstraintType type, Cell target, int param1, int param2)
    {
        constraints.Add(new Constraint(type, target, param1, param2));
    }
    public void RemoveConstraint(int index)
    {
        constraints.RemoveAt(index);
    }
    public bool SwitchReplaceability(Vector2Int pos)
    {
        if (pos.x >= 3 || pos.y >= 3 || pos.x < 0 || pos.y < 0) return false;
        isReplaceable[pos.x, pos.y] = !isReplaceable[pos.x, pos.y];
        return true;
    }
    public void SwitchOutcomeReplaceability()
    {
        isOutcomeReplaceable = !isOutcomeReplaceable;
    }
}
