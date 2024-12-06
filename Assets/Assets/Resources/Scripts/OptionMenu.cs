using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

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

        SetMaster(masterSlider);
        SetSfx(sfxSlider);
        SetMusic(musicSlider);

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
        float decibles = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("MasterVolume", decibles);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetSfx(float volume){
        float decibles = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("SFXVolume", decibles);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void SetMusic(float volume){
        float decibles = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MusicVolume", decibles);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    
}
