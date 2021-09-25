using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager settingManager;
    private AudioSource backgroundMusic;
    private AudioSource playerInteraction;
    public AudioClip uiSFX;
    public AudioClip gameSFX;
    private Scene currentScene;

    // Start is called before the first frame update
    void Start()
    {
        settingManager = gameObject.GetComponent<SettingManager>();
        backgroundMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        playerInteraction = gameObject.GetComponent<AudioSource>();
        currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "TitleScreen")
        {
            LoadSettingData();
        }
    }

    /// <summary>
    /// Switch between fullscreen and windowed mode
    /// </summary>
    public void TongleScreenMode()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    /// <summary>
    /// Change the resolution of the game window
    /// </summary>
    /// <param name="width"> Window's width (pixel)</param>
    /// <param name="height"> Window's height (pixel)</param>
    public void ChangeResolution(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    /// <summary>
    /// Save the player movement speed value
    /// </summary>
    /// <param name="mouseSensitive"> The value that is saved</param>
    public void ChangeMouseSensitive(float mouseSensitive)
    {
        PlayerPrefs.SetFloat("MouseSensitive", mouseSensitive);
        Debug.Log("Saved mouse sensitive: " + mouseSensitive);
    }

    /// <summary>
    /// Save the music volume value
    /// </summary>
    /// <param name="musicVolume"> The value that is saved</param>
    public void ChangeMusicVolume(float musicVolume)
    {
        backgroundMusic.volume = musicVolume;
        PlayerPrefs.SetFloat("BackgroundMusicVolume", musicVolume);
        Debug.Log("Saved music volume: " + musicVolume);
    }

    /// <summary>
    /// Save the sound effect value
    /// </summary>
    /// <param name="SFXVolume"> The value that is saved</param>
    public void ChangeSFXVolume(float SFXVolume)
    {
        playerInteraction.volume = SFXVolume;
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        Debug.Log("Saved sfx volume: " + SFXVolume);
    }

    /// <summary>
    /// Get saved setting data and load to the game UI
    /// </summary>
    private void LoadSettingData() 
    {
        float savedMouseSensitive = PlayerPrefs.GetFloat("MouseSensitive");
        float savedBackgroundMusic = PlayerPrefs.GetFloat("BackgroundMusicVolume");
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume");
        Debug.Log("Mouse: " + savedMouseSensitive + "\nMusic: " + savedBackgroundMusic + "\nSFX: " + savedSFX);

        backgroundMusic.volume = savedBackgroundMusic;
        playerInteraction.volume = savedSFX;
        //GameUI.gameUI.SetUISliderValue(savedMouseSensitive, savedBackgroundMusic, savedSFX);
    }

    /// <summary>
    /// Play SFX when player click a button
    /// </summary>
    public void playUISFX()
    {
        playerInteraction.PlayOneShot(uiSFX);
    }

    /// <summary>
    /// Play SFX in gameplay
    /// </summary>
    public void playGameSFX()
    {
        playerInteraction.PlayOneShot(gameSFX);
    }
}
