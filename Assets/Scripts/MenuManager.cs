using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public GameObject _mainMenu;
    public GameObject _optionsMenu;
    public GameObject _instructionsMenu;
    public GameObject _creditsMenu;

    void Start()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _instructionsMenu.SetActive(false);
        _creditsMenu.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
