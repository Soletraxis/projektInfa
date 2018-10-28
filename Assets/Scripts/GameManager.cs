using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    //to do: add loading screens and move player to starting position on next levels

    #region Variables
    public int currentLevel = 1;
    public int lastLevel = 1000;

    [SerializeField]
    public HUDManager hudManager;

    public static GameManager instance;
#endregion

    #region Awake
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            instance.hudManager = FindObjectOfType<HUDManager>();
        }

        DontDestroyOnLoad(gameObject);
        hudManager = FindObjectOfType<HUDManager>();
    }
    #endregion

    #region ResetGame
    public void ResetGame()
    {
        Time.timeScale = 1;
        Destroy(hudManager.player);
        Destroy(gameObject);
        currentLevel = 1;
        SceneManager.LoadScene("Level" + currentLevel);
    }
    #endregion

    #region NextLevel
    public void NextLevel()
    {
        if (currentLevel < lastLevel)
        {
            currentLevel++;
        }
        else if (currentLevel == lastLevel)
        {
            currentLevel = 1;
        }
        SceneManager.LoadScene("Level" + currentLevel);
        hudManager.player.gameObject.SetActive(false);
        //ADD LOADING SCREEN, MOVE PLAYER TO DESIRED POSITION
        hudManager.player.gameObject.SetActive(true);
    }
    #endregion

    #region GameOver
    public void GameOver()
    {
        //show game over screen
    }
    #endregion
}
