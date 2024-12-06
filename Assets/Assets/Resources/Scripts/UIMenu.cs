using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    
    [SerializeField] bool closeDefault = true;

    void Awake(){
        if(closeDefault){
            CloseMenu();
        }

    }
    public void OpenMenu(){
        GetComponent<Canvas>().enabled = true;
    }
    public void CloseMenu(){
        GetComponent<Canvas>().enabled = false;
    }
}
