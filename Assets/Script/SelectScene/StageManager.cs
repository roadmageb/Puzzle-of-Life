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
    public GameObject objLevelSelectScreen;
    public GameObject prefStageSelectButton;
    public GameObject prefLevelSelectButton;

    int selected_stage;
    int selected_level;
    List<GameObject> LevelSelectScreen_LevelSelectButton_List;

    public void Start()
    {
        for (int i = 0; i < stage_count; i++)
        {
            GameObject created_instance = Instantiate(prefStageSelectButton, objStageSelectScreen.transform);
            created_instance.GetComponent<Transform>().position += new Vector3(i * 15, -3, 0);
            created_instance.GetComponent<StageSelectButton>().stage_to_go = i + 1;
        }
    }

    public void StageSelected(int n)
    {
        selected_stage = n;
        setLevelSelectScreen();
        StartCoroutine(ScreenSlide(objStageSelectScreen, objLevelSelectScreen, 1));
    }

    public void BackSelected()
    {
        StartCoroutine(ScreenSlide(objLevelSelectScreen, objStageSelectScreen, 1));
        closeLevelSelectScreen();
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
        for (int i = 0; i < level_count[selected_stage - 1]; i++)
        {
            Destroy(LevelSelectScreen_LevelSelectButton_List[i].gameObject);
        }
    }

    public IEnumerator ScreenSlide(GameObject from, GameObject to, float animTime)
    {
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            objCamera.GetComponent<Transform>().position = Vector3.Lerp(from.GetComponent<Transform>().position, to.GetComponent<Transform>().position, animFunc(t / animTime)) + new Vector3(0, 0, -10);
            yield return null;
        }
        objCamera.GetComponent<Transform>().position = to.GetComponent<Transform>().position + new Vector3(0, 0, -10);
    }

    float animFunc(float t)
    {
        return (float)Math.Pow(Math.Sin(t * Math.PI / 2), 2.0);
    }
}
