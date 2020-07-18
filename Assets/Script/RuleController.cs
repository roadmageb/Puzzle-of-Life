using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleController : MonoBehaviour
{
    public GameObject[] rulePrefab;
    public GameObject[] ruleBorders;
    public float ruleHeight;
    public float GetSpriteHeight(GameObject g)
    {
        return g.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public void RuleInstantiate(Rule rule)
    {
        ruleBorders = new GameObject[rule.constraints.Count + 2];
        ruleBorders[0] = Instantiate(rulePrefab[0], transform);
        ruleBorders[0].transform.localPosition = new Vector2(0, 0);
        ruleHeight += GetSpriteHeight(rulePrefab[0]);
        for (int i = 0; i < rule.constraints.Count; ++i)
        {
            ruleBorders[i + 1] = Instantiate(rulePrefab[1], transform);
            ruleBorders[i + 1].transform.localPosition = new Vector2(0, -GetSpriteHeight(rulePrefab[0]) - (i * GetSpriteHeight(rulePrefab[1])));
            ruleHeight += GetSpriteHeight(rulePrefab[1]);
        }
        ruleBorders[rule.constraints.Count + 1] = Instantiate(rulePrefab[2], transform);
        ruleBorders[rule.constraints.Count + 1].transform.localPosition =
            new Vector2(0, -GetSpriteHeight(rulePrefab[0]) - (rule.constraints.Count * GetSpriteHeight(rulePrefab[1])));
        ruleHeight += GetSpriteHeight(rulePrefab[2]);
    }
}
