using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    private Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        foreach (var resolution in resolutions){
            options.Add($"{resolution.width}x{resolution.height}");
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        masterSlider.onValueChanged.AddListener(SetMaster);
        sfxSlider.onValueChanged.AddListener(SetSfx);
        musicSlider.onValueChanged.AddListener(SetMusic);

    }
    private int GetCurrentResolutionIndex(){
        for (int i = 0; i < resolutions.Length; i++){
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height){
                return i;
            }
        }
        return 0;
    }

    public void SetResolution(int index){
        
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetMaster(float value){
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetSfx(float value){
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    public void SetMusic(float value){
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    
}
