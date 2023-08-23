using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void SetScene(string text)
    {
        SceneManager.LoadScene(text, LoadSceneMode.Single);
    }

    void Update()
    {
        if(Input.GetKeyDown("0"))
        {
            Application.Quit();
        }
    }
}
