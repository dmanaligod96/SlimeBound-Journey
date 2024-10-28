using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] string winSceneName = "GameOverWinScene";

    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.gameObject.CompareTag("Player")){
            LoadWinScene();
        }

    }
    private void LoadWinScene(){
        SceneManager.LoadScene(winSceneName);
    }
}
