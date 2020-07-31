using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public int stage_count;
    public int[] level_count;
    public GameObject objCamera;
    public GameObject objStageSelectScreen;
    public GameObject objStageSelectButtonsScreen;
    public GameObject objLevelSelectScreen;
    public GameObject prefStageSelectButton;
    public GameObject prefLevelSelectButton;

    int selected_stage = 0;
    int selected_level;
    int now_stage = 1;
    List<GameObject> LevelSelectScreen_LevelSelectButton_List;

    public void Start()
    {
        for (int i = 0; i < stage_count; i++)
        {
            GameObject created_instance = Instantiate(prefStageSelectButton, objStageSelectButtonsScreen.transform);
            created_instance.GetComponent<Transform>().position += new Vector3(i * 15, 0, 0);
            created_instance.GetComponent<StageSelectButton>().stage_to_go = i + 1;
        }
    }

    public void StageChange(int i)
    {
        int last_stage = now_stage;
        now_stage += i;
        if (now_stage < 1)
        {
            now_stage = stage_count;
        }
        if (now_stage > stage_count)
        {
            now_stage = 1;
        }
        StartCoroutine(ScreenSlide(objStageSelectButtonsScreen, new Vector3(-15 * (last_stage - 1), -3, 0), new Vector3(-15 * (now_stage - 1), -3, 0), 0.5f));
    }

    public void StageSelected(int n)
    {
        closeLevelSelectScreen();
        selected_stage = n;
        setLevelSelectScreen();
        StartCoroutine(ScreenSlide(objCamera, objStageSelectScreen.GetComponent<Transform>().position + new Vector3(0, 0, -10), objLevelSelectScreen.GetComponent<Transform>().position + new Vector3(0, 0, -10), 1));
    }

    public void BackSelected()
    {
        StartCoroutine(ScreenSlide(objCamera, objLevelSelectScreen.GetComponent<Transform>().position + new Vector3(0, 0, -10), objStageSelectScreen.GetComponent<Transform>().position + new Vector3(0, 0, -10), 1));
    }

    void setLevelSelectScreen()
    {
        LevelSelectScreen_LevelSelectButton_List = new List<GameObject>();
        for (int i = 0; i < level_count[selected_stage - 1]; i++)
        {
            GameObject created_instance = Instantiate(prefLevelSelectButton, objLevelSelectScreen.transform);
            created_instance.GetComponent<Transform>().position += new Vector3(-3 + (i % 5) * 1.5f, 3 - (i / 5) * 1.5f, 0);
            created_instance.GetComponent<LevelSelectButton>().level_to_go = i + 1;
            LevelSelectScreen_LevelSelectButton_List.Add(created_instance);
        }
    }

    void closeLevelSelectScreen()
    {
        if (selected_stage == 0)
        {
            return;
        }
        for (int i = 0; i < level_count[selected_stage - 1]; i++)
        {
            Destroy(LevelSelectScreen_LevelSelectButton_List[i].gameObject);
        }
    }

    public IEnumerator ScreenSlide(GameObject target, Vector3 from, Vector3 to, float animTime)
    {
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            target.GetComponent<Transform>().position = Vector3.Lerp(from, to, animFunc(t / animTime));
            yield return null;
        }
        target.GetComponent<Transform>().position = to;
    }

    float animFunc(float t)
    {
        return (float)Math.Pow(Math.Sin(t * Math.PI / 2), 2.0);
    }
}
