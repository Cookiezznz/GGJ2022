using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour
{
    /// <summary>
    /// Scene Manager
    /// Scene 0: Menu
    /// Scene 1: Game
    /// </summary>

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
