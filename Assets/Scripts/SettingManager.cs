using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SettingData
{
    public bool fullScreen;
    public int resolutionIndex;
    public int windowWidth;
    public int windowHeight;
    public float musicVolume;
    public float SFXVolume;
}

public class SettingManager : MonoBehaviour
{
    public static SettingManager settingManager;
    private AudioSource backgroundMusic;
    private AudioSource playerInteraction;
    public AudioClip uiSFX;
    public AudioClip gameSFX;
    public AudioClip goalSFX;
    public AudioClip winSFX;
    public AudioClip loseSFX;

    private string savedDataPath;
    private int resolutionIndex, width, height;
    public bool playerActive;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        settingManager = gameObject.GetComponent<SettingManager>();
        backgroundMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        playerInteraction = gameObject.GetComponent<AudioSource>();
        savedDataPath = Application.dataPath + "/Setting.dat";
        playerActive = false;

        LoadSettingData();
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
    /// <param name="width">Window's width (pixel)</param>
    /// <param name="height">Window's height (pixel)</param>
    public void ChangeResolution(int resolutionIndex, int width, int height)
    {
        this.resolutionIndex = resolutionIndex;
        this.width = width;
        this.height = height;
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    //public void ChangeMouseSensitive(float mouseSensitive)
    //{
    //    PlayerPrefs.SetFloat("MouseSensitive", mouseSensitive);
    //    Debug.Log("Saved mouse sensitive: " + mouseSensitive);
    //}

    /// <summary>
    /// Change the background music volume
    /// </summary>
    /// <param name="musicVolume">The volume of background music</param>
    public void ChangeMusicVolume(float musicVolume)
    {
        backgroundMusic.volume = musicVolume;
    }

    /// <summary>
    /// Change the sound effect volume
    /// </summary>
    /// <param name="SFXVolume">The volume of sound effect</param>
    public void ChangeSFXVolume(float SFXVolume)
    {
        playerInteraction.volume = SFXVolume;
    }

    /// <summary>
    /// Play SFX when player click a button
    /// </summary>
    public void playUISFX()
    {
        playerInteraction.PlayOneShot(uiSFX);
        Debug.Log("Play UI SFX");
    }

    /// <summary>
    /// Play SFX in gameplay
    /// </summary>
    public void playGameSFX()
    {
        playerInteraction.PlayOneShot(gameSFX);
        Debug.Log("Play gameplay SFX");
    }

    /// <summary>
    /// Play goal sound effect
    /// </summary>
    public void playGoalSFX()
    {
        playerInteraction.PlayOneShot(goalSFX);
        Debug.Log("Play goal SFX");
    }

    /// <summary>
    /// Play endgame sound effect
    /// </summary>
    /// <param name="isWinner">Play win or lose sound effect</param>
    public void playEndgameSFX(bool isWinner)
    {
        if (isWinner)
        {
            playerInteraction.PlayOneShot(winSFX);
            Debug.Log("Play win SFX");
        }
        else
        {
            playerInteraction.PlayOneShot(loseSFX);
            Debug.Log("Play lose SFX");
        }
    }

    /// <summary>
    /// Save setting data to a file
    /// </summary>
    /// <param name="isCurrentData">Set to true will save the current data and false will save default data</param>
    public void SaveSettingData(bool isCurrentData)
    {
        BinaryFormatter binFormatter = new BinaryFormatter();
        FileStream file;
        if (!File.Exists(savedDataPath))
        {
            file = File.Create(savedDataPath);
            Debug.Log("Setting file created");
        }
        else
        {
            file = File.Open(savedDataPath, FileMode.Create);
            Debug.Log("Setting file opened");
        }
        SettingData data = new SettingData();
        if (isCurrentData)
        {
            data.fullScreen = Screen.fullScreen;
            data.resolutionIndex = resolutionIndex;
            data.windowWidth = width;
            data.windowHeight = height;
            data.musicVolume = backgroundMusic.volume;
            data.SFXVolume = playerInteraction.volume;
        }
        else
        {
            data.fullScreen = true;
            data.resolutionIndex = 0;
            data.windowWidth = 1920;
            data.windowHeight = 1080;
            data.musicVolume = 1;
            data.SFXVolume = 1;
        }
        binFormatter.Serialize(file, data);
        file.Close();
        Debug.Log("Setting saved");
    }

    /// <summary>
    /// Load setting data from file
    /// </summary>
    public void LoadSettingData()
    {
        if (!File.Exists(savedDataPath))
        {
            SaveSettingData(false);
            Debug.Log("No saved data found. New setting data file is created");
        }
        BinaryFormatter binFormatter = new BinaryFormatter();
        FileStream file = File.Open(savedDataPath, FileMode.Open);
        SettingData data = (SettingData)binFormatter.Deserialize(file);
        file.Close();
        Debug.Log("Load setting data:\n Screen mode: " + data.fullScreen + "\nResolution index: " + data.resolutionIndex + "\nResolution: " + data.windowWidth + " " + data.windowHeight + "\nMusic volume: " + data.musicVolume + "\nSFX volume: " + data.SFXVolume);
        backgroundMusic.volume = data.musicVolume;
        playerInteraction.volume = data.SFXVolume;
        resolutionIndex = data.resolutionIndex;
    }

    /// <summary>
    /// Return saved data
    /// </summary>
    /// <returns></returns>
    public SettingData GetSavedData()
    {
        SettingData setting = new SettingData();
        setting.fullScreen = Screen.fullScreen;
        setting.resolutionIndex = resolutionIndex;
        setting.musicVolume = backgroundMusic.volume;
        setting.SFXVolume = playerInteraction.volume;
        return setting;
    }
}
