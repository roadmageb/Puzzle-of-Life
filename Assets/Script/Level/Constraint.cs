using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constraint
{
    public ConstraintType type;
    public Cell target;
    public bool isReplaceable;
    public int param1, param2;
    public Constraint(ConstraintType type, Cell target, int param1, int param2)
    {
        this.type = type;
        this.target = target;
        this.param1 = param1;
        this.param2 = param2;
        isReplaceable = false;
    }
    public void SetReplaceability(bool isReplaceable)
    {
        this.isReplaceable = isReplaceable;
    }

    public bool ConstraintMatches(int val)
    {
        switch(type)
        {
            case ConstraintType.GE:
                return param1 <= val;
            case ConstraintType.LE:
                return param1 >= val;
            case ConstraintType.EQ:
                return param1 == val;
            case ConstraintType.NE:
                return param1 != val;
            case ConstraintType.BET:
                return param1 <= val && val <= param2;
            default:
                return false;
        }
    }
}
