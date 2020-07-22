using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    public string go_to_scene;

    void OnMouseDown()
    {
        Debug.Log("Scene has been changed to " + go_to_scene);
        SceneManager.LoadScene(go_to_scene);
    }
}
