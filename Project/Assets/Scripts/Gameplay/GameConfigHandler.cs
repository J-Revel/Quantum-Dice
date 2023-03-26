using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConfigHandler : MonoBehaviour
{
    public static GameConfigHandler instance;
    public LevelConfig[] levelList;
    public LevelConfig currentLevel {get { return levelList[currentLevelIndex]; } }
    public string gameSceneName;
    public string endingSceneName;

    public int currentLevelIndex;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            currentLevelIndex = 0;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadFirstLevel()
    {
        currentLevelIndex = 0;
        SceneManager.LoadScene(gameSceneName);
    }

    public void GotoNextLevel()
    {
        currentLevelIndex++;
        if(currentLevelIndex >= levelList.Length)
        {
            SceneManager.LoadScene(endingSceneName);
        }
        else
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(endingSceneName);
    }
}
