using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LoadGame();
        }
        else if (Input.touchCount > 0)
        {
            LoadGame();
        }
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
