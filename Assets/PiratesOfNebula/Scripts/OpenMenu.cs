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

    public AudioMixer audioMixer;
    public static AudioMixerGroup[] audiomixergroup;
    public AudioMixerGroup[] audioMixerGroup;

    [Range(-80f, 0f)]
    public float DefaultVolume;

    float sound;
    float Music;
    int Quality;
    int Res;
    int CurrentRes;
    public Slider[] Sliders;
    public TMP_Dropdown[] Dropdowns;

    public bool DeleteAllSaves; // Deletes all Saves if Turns on. So Keep Off

    Resolution[] resolutions;

    public GameObject Ship;
    public GameObject Map;
    public GameObject OpenCanvas;
    public GameObject MainUI;
    public GameObject ShipControls;

    // Start is called before the first frame update
    private void Awake()
    {
        audiomixergroup = audioMixerGroup;
    }

    void Start()
    {
        Ship.SetActive(false);
        Map.SetActive(false);
        OpenCanvas.SetActive(true);
        MainUI.SetActive(false);
        ShipControls.SetActive(false);

        resolutions = Screen.resolutions;
        Dropdowns[1].ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentRes = i;
            }
        }
        Dropdowns[1].AddOptions(options);
        Dropdowns[1].RefreshShownValue();
        ResetSettings();
        audioMixer.SetFloat("Sound", PlayerPrefs.GetFloat("Sound"));
        audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
        Resolution R = resolutions[PlayerPrefs.GetInt("Res")];
        Screen.SetResolution(R.width, R.height, Screen.fullScreen);


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
        Ship.SetActive(false);
        Map.SetActive(false);
        OpenCanvas.SetActive(true);
        MainUI.SetActive(false);
        ShipControls.SetActive(false);
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
        Application.Quit();
    }

    public void OpenAvatarSelector()
    {
        SpecSelector.SetActive(false);
        AvatarSelector.SetActive(true);
    }

    public void StartGame()
    {
        //SceneManager.LoadScene("SampleScene");
        Ship.SetActive(true);
        Map.SetActive(true);
        OpenCanvas.SetActive(false);
        MainUI.SetActive(true);
        ShipControls.SetActive(true);
    }

    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void PlayTime()
    {
        Time.timeScale = 1f;
    }

    public void SetVolume(float volume)
    {
        sound = volume;
    }
    public void SetMusic(float volume)
    {
        Music = volume;
    }
    public void SetQuality(int i)
    {
        Quality = i;
       // QualitySettings.SetQualityLevel(i);
    }
    public void SetRes(int i)
    {
        Res = i;
        // QualitySettings.SetQualityLevel(i);
    }
    public void SaveSettings()
    {
        audioMixer.SetFloat("Sound", sound);
        PlayerPrefs.SetFloat("Sound", sound);

        audioMixer.SetFloat("Music",Music);  
        PlayerPrefs.SetFloat("Music", Music);

        QualitySettings.SetQualityLevel(Quality);
        PlayerPrefs.SetInt("Quality", Quality);

        Resolution R = resolutions[Res];
        Screen.SetResolution(R.width, R.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Res", Res);
    }
    public void OpenSettings()
    {
        Sliders[0].value= PlayerPrefs.GetFloat("Sound");
        Sliders[1].value = PlayerPrefs.GetFloat("Music");
        Dropdowns[0].value = PlayerPrefs.GetInt("Quality");
        Dropdowns[1].value = PlayerPrefs.GetInt("Res");
        ButtonsPanel.SetActive(false);
        SettingsMenu.SetActive(true);       
    }
    public void ExitSettings()
    {
        ButtonsPanel.SetActive(true);
        SettingsMenu.SetActive(false);
        sound = PlayerPrefs.GetFloat("Sound");
        Music = PlayerPrefs.GetFloat("Music");
        Quality = PlayerPrefs.GetInt("Quality");
        Res = PlayerPrefs.GetInt("Res");
    }
    public void ResetSettings()
    {
        audioMixer.SetFloat("Sound", DefaultVolume);
        audioMixer.SetFloat("Music", DefaultVolume);
        QualitySettings.SetQualityLevel(6);
        Resolution R = resolutions[CurrentRes];
        Screen.SetResolution(R.width, R.height, Screen.fullScreen);

        PlayerPrefs.SetFloat("Sound", DefaultVolume);
        PlayerPrefs.SetFloat("Music", DefaultVolume);
        PlayerPrefs.SetInt("Quality", 6);
        PlayerPrefs.SetInt("Res", CurrentRes);

        Sliders[0].value = DefaultVolume;
        Sliders[1].value = DefaultVolume;
        Dropdowns[0].value = 6;
        Dropdowns[1].value = CurrentRes;
    }

}
