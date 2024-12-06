using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame(){
        SceneManager.LoadScene("Tutorial");

    }

    // Update is called once per frame
    public void QuitGame(){
        Application.Quit();
    }

    public void StartMenu(){
        SceneManager.LoadScene("Menu");
    }
    public void SettingsMenu(){
        SceneManager.LoadScene("Settings");
    }
}
