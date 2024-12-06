using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        foreach (var resolution in resolutions){
            options.Add($"{resolution.width}x{resolution.height}");
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        float masterSlider = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float sfxSlider = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float musicSlider = PlayerPrefs.GetFloat("MusicVolume", 1f);

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

    
}
