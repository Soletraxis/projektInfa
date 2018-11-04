using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour {

    #region Variables
    private bool cancelActive = false;
    private bool settingsDisplayed = false;
    private bool gamePauseCanvasActive = false;
    private float audioLevel = 0.5f;

    [SerializeField]
    private AudioSource gameAudio;

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private Slider audioSlider;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    public GameObject heldWeaponDisplay;

    [SerializeField]
    public GameObject HUDCanvas;

    [SerializeField]
    public GameObject gamePauseCanvas;

    [SerializeField]
    public GameObject gamePauseUI;

    [SerializeField]
    public GameObject gamePauseUISettings;
    #endregion

    #region Awake
    private void Awake()
    {
        gamePauseUISettings.SetActive(false);
        gamePauseCanvas.SetActive(false);
        audioSlider.value = audioLevel;
    }
#endregion

    #region Update
    void FixedUpdate () { // maybe does not have to be called all the time, just when loses or gains hp
        HealthUpdate();
    }
    #endregion

    #region Update
    private void Update()
    {
        GamePause(0);
    }
#endregion

    #region HealthUpdate
    public void HealthUpdate()
    {
        healthSlider.maxValue = player.GetComponent<PlayerController>().maxPlayerHealth;
        healthSlider.value = player.GetComponent<PlayerController>().currentPlayerHealth;
        healthSlider.GetComponentInChildren<Text>().text = player.GetComponent<PlayerController>().currentPlayerHealth + "/" + player.GetComponent<PlayerController>().maxPlayerHealth;
    }
    #endregion

    #region GamePause
    public void GamePause(int a)
    {
        if (Input.GetKeyDown(KeyCode.Escape) || a > 0.0f)
        {
            if (gamePauseUISettings.active)
            {
                SettingsTOGGLE();
            }
            else
            {
                gamePauseCanvas.SetActive(!cancelActive);
                cancelActive = !cancelActive;
                Cursor.visible = cancelActive;
                Time.timeScale = 0;
            }
            if (cancelActive == false)
            {
                Time.timeScale = 1;
            }
        }
    }
    #endregion

    #region BackToMenu
    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartingMenu");
        Destroy(player);
        Destroy(GameManager.instance.gameObject);
    }
    #endregion

    #region SettingsTOGGLE
    public void SettingsTOGGLE()
    {
        gamePauseUI.SetActive(settingsDisplayed);
        settingsDisplayed = !settingsDisplayed;
        gamePauseUISettings.SetActive(settingsDisplayed);
    }
    #endregion

    #region SetVolume
    public void setVolume()
    {
        PlayerPrefs.SetFloat("audioLevel", audioSlider.value);
        gameAudio.volume = audioSlider.value;
    }
#endregion
}

