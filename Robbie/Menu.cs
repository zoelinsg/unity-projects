using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //加載當前場景的下個場景
    }
    public void QuitGame() 
    {
        Application.Quit();  //離開遊戲
    }

}
