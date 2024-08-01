using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;

    private string newGameScene = "SampleScene";


    // Start is called before the first frame update
    void Start()
    {
        //设置最高分文本
        int highScore = SaveLoadManager.Instance.LoadHighScoreKey();
        highScoreUI.text = $"Top Wave Survived: {highScore}";

    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif

    }

}
