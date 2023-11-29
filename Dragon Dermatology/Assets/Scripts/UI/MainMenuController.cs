using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject howtoMenu;
 
 
    public void playGame() {
        SceneManager.LoadScene("Gameplay");
    }
 
    public void howTo() {
        mainMenu.SetActive(false);
        howtoMenu.SetActive(true);
    }
 
    public void back() {
        mainMenu.SetActive(true);
        howtoMenu.SetActive(false);
    }
 
    public void exitGame() {
        Application.Quit();
    }
}