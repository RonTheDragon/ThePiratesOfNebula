using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject TutorialMain;
    public GameObject FirstPhase;
    public GameObject SecondPhase;
    public GameObject ThirdPhase;
    public GameObject FouthPhase;
    public GameObject FifthPhase;

    public GameObject InfoPanel;

    public Button FirstPhaseNextButton;
    public Button FirstPhaseSkipButton;
    public Button FourthPhaseNextButton;
    public Button FifthPhasePlayButton;

    public Button InfoButton;

    bool ButtonListenerChanged = false;

    private void Start()
    {
        FirstPhaseNextButton.onClick.AddListener(() => SecondTutorial());
        FirstPhaseSkipButton.onClick.AddListener(() => SkipTutorial());
        FourthPhaseNextButton.onClick.AddListener(() => FifthTutorial());
        FifthPhasePlayButton.onClick.AddListener(() => SkipTutorial());

        InfoButton.onClick.AddListener(() => OpenInfo());

        InfoPanel.SetActive(false);
    }

    private void Update()
    {
        if (InfoPanel.activeInHierarchy && !ButtonListenerChanged)
        {
            InfoButton.onClick.AddListener(() => CloseInfo());
            ButtonListenerChanged = true;
        }

        if(!InfoPanel.activeInHierarchy && !ButtonListenerChanged)
        {
            InfoButton.onClick.AddListener(() => OpenInfo());
            ButtonListenerChanged = true;
        }
    }

    public void StartTutorial()
    {
        Time.timeScale = 1f;
        TutorialMain.SetActive(true);
        FirstPhase.SetActive(true);
        SecondPhase.SetActive(false);
        ThirdPhase.SetActive(false);
        FouthPhase.SetActive(false);
        FifthPhase.SetActive(false);
    }
    public void SecondTutorial()
    {
        Time.timeScale = 1f;
        TutorialMain.SetActive(true);
        FirstPhase.SetActive(false);
        SecondPhase.SetActive(true);
        ThirdPhase.SetActive(false);
        FouthPhase.SetActive(false);
        FifthPhase.SetActive(false);
    }
    public void ThirdTutorial()
    {
        Time.timeScale = 1f;
        TutorialMain.SetActive(true);
        FirstPhase.SetActive(false);
        SecondPhase.SetActive(false);
        ThirdPhase.SetActive(true);
        FouthPhase.SetActive(false);
        FifthPhase.SetActive(false);
    }
    public void FourthTutorial()
    {
        Time.timeScale = 1f;
        TutorialMain.SetActive(true);
        FirstPhase.SetActive(false);
        SecondPhase.SetActive(false);
        ThirdPhase.SetActive(false);
        FouthPhase.SetActive(true);
        FifthPhase.SetActive(false);
    }
    public void FifthTutorial()
    {
        Time.timeScale = 1f;
        TutorialMain.SetActive(true);
        FirstPhase.SetActive(false);
        SecondPhase.SetActive(false);
        ThirdPhase.SetActive(false);
        FouthPhase.SetActive(false);
        FifthPhase.SetActive(true);
    }
    public void SkipTutorial()
    {
        Time.timeScale = 1f;
        TutorialMain.SetActive(false);
        FirstPhase.SetActive(false);
        SecondPhase.SetActive(false);
        ThirdPhase.SetActive(false);
        FouthPhase.SetActive(false);
        FifthPhase.SetActive(false);
    }

    void OpenInfo()
    {
        InfoPanel.SetActive(true);
        ButtonListenerChanged = false;
    }
    void CloseInfo()
    {
        InfoPanel.SetActive(false); 
        ButtonListenerChanged = false;
    }
}
