using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMenu : MonoBehaviour
{

    public GameObject ButtonsPanel;
    public GameObject NewGameMenu;
    public GameObject SettingsMenu;

    public GameObject SpecSelector;
    public GameObject AvatarSelector;


    // Start is called before the first frame update
    void Start()
    {
        ButtonsPanel.SetActive(true);
        NewGameMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SpecSelector.SetActive(true);
        AvatarSelector.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenNewGame()
    {
        ButtonsPanel.SetActive(false);
        NewGameMenu.SetActive(true);
    }

    public void ExitNewGame()
    {
        ButtonsPanel.SetActive(true);
        NewGameMenu.SetActive(false);
        SpecSelector.SetActive(true);
        AvatarSelector.SetActive(false);
    }

    public void OpenSettings()
    {
        ButtonsPanel.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    
    public void ExitSettings()
    {
        ButtonsPanel.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenAvatarSelector()
    {
        SpecSelector.SetActive(false);
        AvatarSelector.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
