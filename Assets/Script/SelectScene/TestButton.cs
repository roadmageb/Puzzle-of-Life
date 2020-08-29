using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestButton : SelectButton
{
    protected override void ButtonAction()
    {
        Level level = LevelManager.Inst.currentLevel;
        foreach (Rule rule in level.rules)
        {
            rule.RemoveConstraint(rule.constraints.Count - 1);
        }
        List<CellNumPair> palette = new List<CellNumPair>();
        foreach (CellNumPair pair in level.palette)
        {
            if (pair.num == 0)
            {
                continue;
            }
            else
            {
                palette.Add(pair);
            }
        }
        LevelManager.Inst.currentLevel.palette = palette;
        string levelstr = JsonConvert.SerializeObject(level);
        File.WriteAllText(Application.dataPath + "/Resources/test.json", levelstr);

        GameManager.Inst.TestLevel();
    }
}
