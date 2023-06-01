using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ResetSimulation()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
