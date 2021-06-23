﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
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
    Resolution[] resolutions;
    CanvasGroup CG;

    // Start is called before the first frame update
    private void Awake()
    {
        audiomixergroup = audioMixerGroup;
        CG = GetComponent<CanvasGroup>();
    }

    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
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

        audioMixer.SetFloat("Music", Music);
        PlayerPrefs.SetFloat("Music", Music);

        QualitySettings.SetQualityLevel(Quality);
        PlayerPrefs.SetInt("Quality", Quality);

        Resolution R = resolutions[Res];
        Screen.SetResolution(R.width, R.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Res", Res);
    }
    public void OpenSettings()
    {
        Sliders[0].value = PlayerPrefs.GetFloat("Sound");
        Sliders[1].value = PlayerPrefs.GetFloat("Music");
        Dropdowns[0].value = PlayerPrefs.GetInt("Quality");
        Dropdowns[1].value = PlayerPrefs.GetInt("Res");
        CG.interactable = true;
        CG.blocksRaycasts = true;
        transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ExitSettings()
    {
        sound = PlayerPrefs.GetFloat("Sound");
        Music = PlayerPrefs.GetFloat("Music");
        Quality = PlayerPrefs.GetInt("Quality");
        Res = PlayerPrefs.GetInt("Res");
        CG.interactable = false;
        CG.blocksRaycasts = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1f;
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
