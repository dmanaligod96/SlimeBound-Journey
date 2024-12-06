using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System;

public class OptionMenu : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer audioMixer;

    private Resolution[] resolutions;
    

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;
        int currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutions.Length -1);
        for(int i = 0; i < resolutions.Length; i++ ){
            string resolutionsString = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionsString));
        }
        currentResolutionIndex = Math.Min(currentResolutionIndex, resolutions.Length -1);
        resolutionDropdown.value = currentResolutionIndex;
        SetResolution();

        float masterSlider = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float sfxSlider = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float musicSlider = PlayerPrefs.GetFloat("MusicVolume", 1f);

    }
    

    public void SetResolution(){
        
        int rezIndex = resolutionDropdown.value;
        Screen.SetResolution(resolutions[rezIndex].width, resolutions[rezIndex].height, true);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
    }
    public void SetMaster(){
        SetVolume("MasterVolume", masterSlider.value);
        
    }

    public void SetSfx(){
        SetVolume("SFXVolume", sfxSlider.value);
        
    }
    public void SetMusic(){
        SetVolume("MusicVolume", musicSlider.value);
        
    }
    void SetVolume(string groupName, float value){
        float decibles = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(groupName, decibles);
    }
    
    public void QuitGame(){
        Application.Quit();
    }
    
    public void MainMenu(){

        SceneManager.LoadScene("Menu");
    }

    
}
