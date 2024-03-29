﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SelectSceneManager : Singleton<SelectSceneManager>
{
    public GameObject objCamera;
    public GameObject objStageSelectScreen;
    public GameObject objStageSelectButtonsScreen;
    public GameObject objLevelSelectScreen;
    public GameObject objEditModeScreen;
    public GameObject prefStageSelectButton;
    public GameObject prefLevelSelectButton;
    public GameObject prefEditLevelSelectButton;

    int SelectedStage = 0;
    int NowStage = 1;
    List<GameObject> LevelSelectScreenLevelSelectButtonList;
    List<GameObject> EditModeScreenEditLevelSelectButtonList;


    Vector3 CameraZPosition = new Vector3(0, 0, -10);

    public void Start()
    {
        //Create stage select buttons
        for (int i = 0; i < GameManager.Inst.StageCount; i++)
        {
            GameObject created_instance = Instantiate(prefStageSelectButton, objStageSelectButtonsScreen.transform);
            created_instance.transform.position += new Vector3(i * 15, 0, 0);
            created_instance.GetComponent<StageSelectButton>().SetStageSelectButton(i + 1);
        }

        //Create edit level select buttons
        EditModeScreenEditLevelSelectButtonList = new List<GameObject>();
        for (int i = 0; i < 54; i++)
        {
            GameObject created_instance = Instantiate(prefEditLevelSelectButton, objEditModeScreen.transform);
            created_instance.transform.position += new Vector3(-6 + (i % 9) * 1.5f, 4 - (i / 9) * 1.5f, 0);
            bool ip = false;
            if (File.Exists(Path.Combine(Application.persistentDataPath + "/CustomStage/" + (i + 1) + ".json")))
            {
                ip = true;
            }
            created_instance.GetComponent<EditLevelSelectButton>().SetEditLevelSelectButton(i + 1, ip);
            EditModeScreenEditLevelSelectButtonList.Add(created_instance);
        }

        if(GameManager.Inst.stage > 0)
        {
            CloseLevelSelectScreen();
            SelectedStage = GameManager.Inst.stage;
            SetLevelSelectScreen();
            objCamera.transform.position = objLevelSelectScreen.transform.position + CameraZPosition;
            objStageSelectButtonsScreen.transform.position = new Vector3(-15 * (SelectedStage - 1), -3, 0);
        }
        else if(GameManager.Inst.stage == -1)
        {
            objCamera.transform.position = objEditModeScreen.transform.position + CameraZPosition;
        }
    }

    public void StageChange(int i)
    {
        int last_stage = NowStage;
        NowStage += i;
        if (NowStage < 1)
        {
            NowStage = GameManager.Inst.StageCount;
        }
        if (NowStage > GameManager.Inst.StageCount)
        {
            NowStage = 1;
        }
        StartCoroutine(ScreenSlide(objStageSelectButtonsScreen, new Vector3(-15 * (last_stage - 1), -3, 0), new Vector3(-15 * (NowStage - 1), -3, 0), 0.5f));
    }

    public void StageSelected(int n)
    {
        CloseLevelSelectScreen();
        SelectedStage = n;
        SetLevelSelectScreen();
        StartCoroutine(ScreenSlide(objCamera, objStageSelectScreen.transform.position + CameraZPosition, objLevelSelectScreen.transform.position + CameraZPosition, 1));
    }

    public void LevelSelected(int SelectedLevel)
    {
        if (GameManager.Inst.IsPlayable(SelectedStage, SelectedLevel))
        {
            GameManager.Inst.LoadPuzzle(SelectedStage, SelectedLevel);
        }
    }

    public void BackSelected()
    {
        StartCoroutine(ScreenSlide(objCamera, objLevelSelectScreen.transform.position + CameraZPosition, objStageSelectScreen.transform.position + CameraZPosition, 1));
    }

    void SetLevelSelectScreen()
    {
        LevelSelectScreenLevelSelectButtonList = new List<GameObject>();
        for (int i = 0; i < GameManager.Inst.LevelCount[SelectedStage - 1]; i++)
        {
            GameObject created_instance = Instantiate(prefLevelSelectButton, objLevelSelectScreen.transform);
            created_instance.transform.position += new Vector3(-3 + (i % 5) * 1.5f, 1 - (i / 5) * 1.5f, 0);
            created_instance.GetComponent<LevelSelectButton>().SetLevelSelectButton(i + 1, GameManager.Inst.IsCleared(SelectedStage, i + 1), GameManager.Inst.IsPlayable(SelectedStage, i + 1));
            LevelSelectScreenLevelSelectButtonList.Add(created_instance);
        }
    }

    void CloseLevelSelectScreen()
    {
        if (SelectedStage == 0)
        {
            return;
        }
        for (int i = 0; i < GameManager.Inst.LevelCount[SelectedStage - 1]; i++)
        {
            Destroy(LevelSelectScreenLevelSelectButtonList[i].gameObject);
        }
    }

    public void EditModeSelected()
    {
        StartCoroutine(ScreenSlide(objCamera, objStageSelectScreen.transform.position + CameraZPosition, objEditModeScreen.transform.position + CameraZPosition, 1));
    }

    public void PlayModeSelected()
    {
        StartCoroutine(ScreenSlide(objCamera, objEditModeScreen.transform.position + CameraZPosition, objStageSelectScreen.transform.position + CameraZPosition, 1));
    }

    public void EditModeChange(bool edit)
    {
        for (int i = 0; i < 54; i++)
        {
            EditModeScreenEditLevelSelectButtonList[i].gameObject.GetComponent<EditLevelSelectButton>().EditModeChange(edit);
        }
    }

    public IEnumerator ScreenSlide(GameObject target, Vector3 from, Vector3 to, float animTime)
    {
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            target.transform.position = Vector3.Lerp(from, to, AnimFunc(t / animTime));
            yield return null;
        }
        target.transform.position = to;
    }

    float AnimFunc(float t)
    {
        return (float)Math.Pow(Math.Sin(t * Math.PI / 2), 2.0);
    }
}
