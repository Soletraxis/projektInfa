using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingMenuManager : MonoBehaviour {
    //to do: add more settings

    private bool settingsDisplayed = false;

    public float audioLevel;

    [SerializeField]
    private Slider audioSlider;

    [SerializeField]
    private GameObject settingsScreen;

    [SerializeField]
    private GameObject mainScreen;

    [SerializeField]
    private AudioSource menuAudio;

    private void Awake()
    {
        mainScreen.SetActive(!settingsDisplayed);
        settingsScreen.SetActive(settingsDisplayed);
        audioLevel = PlayerPrefs.GetFloat("playerVolume");
    }

    private void Start()
    {
        audioSlider.value = audioLevel;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsScreen.active)
            {
                ToggleSettings();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ToggleSettings()
    {
        mainScreen.SetActive(settingsDisplayed);
        settingsDisplayed = !settingsDisplayed;
        settingsScreen.SetActive(settingsDisplayed);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void setVolume()
    {
        PlayerPrefs.SetFloat("audioLevel", audioSlider.value);
        menuAudio.volume = audioSlider.value;
    }
}
