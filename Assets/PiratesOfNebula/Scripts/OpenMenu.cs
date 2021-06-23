using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OpenMenu : MonoBehaviour
{

    public GameObject ButtonsPanel;
    public GameObject NewGameMenu;
    public GameObject SettingsMenu;

    public GameObject SpecSelector;
    public GameObject AvatarSelector;

    public GameObject SaveSlots;
    public GameObject LoadSlots;

    public GameObject[] Saves;

    

    public bool DeleteAllSaves; // Deletes all Saves if Turns on. So Keep Off

    


    // Start is called before the first frame update
    

    void Start()
    {
        ButtonsPanel.SetActive(true);
        NewGameMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SpecSelector.SetActive(true);
        AvatarSelector.SetActive(false);

        for (int i = 1; i < Saves.Length + 1; i++)
        {
            string path = Application.persistentDataPath + "/player.SaveFile" + i.ToString();

            if(File.Exists(path)&&DeleteAllSaves) File.Delete(path); 

            if (File.Exists(path))
            {
                Saves[i - 1].SetActive(true);
                
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenMenuStart()
    {
        
        ButtonsPanel.SetActive(true);
        NewGameMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SpecSelector.SetActive(true);
        AvatarSelector.SetActive(false);
        SaveSlots.SetActive(false);
        LoadSlots.SetActive(false);
    }

    public void OpenNewGame()
    {
        ButtonsPanel.SetActive(false);
        SaveSlots.SetActive(true);
    }

    public void SelectSaveSlot(int Slot)
    {
        SaveSlots.SetActive(false);
        PlayerPrefs.SetInt("slot", Slot);
        PlayerPrefs.SetInt("NewGame", 1);
        NewGameMenu.SetActive(true);
    }

    public void OpenLoadGame()
    {
        ButtonsPanel.SetActive(false);
        LoadSlots.SetActive(true);
    }

    public void SelectLoadSlot(int Slot)
    {
        SaveSlots.SetActive(false);
        PlayerPrefs.SetInt("slot", Slot);
        PlayerPrefs.SetInt("NewGame", 0);
        StartGame();
    }

    public void ExitNewGame()
    {
        SaveSlots.SetActive(false);
        LoadSlots.SetActive(false);
        ButtonsPanel.SetActive(true);
        NewGameMenu.SetActive(false);
        SpecSelector.SetActive(true);
        AvatarSelector.SetActive(false);
    }

    
    
    

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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

    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void PlayTime()
    {
        Time.timeScale = 1f;
    }

}
