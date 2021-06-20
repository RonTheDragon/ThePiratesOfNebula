using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NowPlaying : MonoBehaviour {

    public TextMeshProUGUI nowPlayingText;
    public UnityEngine.UI.Slider musicLength;


    void Update () {
      
        if (MusicManager.instance.CurrentTrackNumber() >= 0) {
            string timeText = SecondsToMS(MusicManager.instance.TimeInSeconds());
            string lengthText = SecondsToMS(MusicManager.instance.LengthInSeconds());

            nowPlayingText.text = "" + (MusicManager.instance.CurrentTrackNumber() + 1) + ".  " +
                MusicManager.instance.NowPlaying().name
                + " (" + timeText + "/" + lengthText + ")" ;

            musicLength.value = MusicManager.instance.TimeInSeconds();
            musicLength.maxValue = MusicManager.instance.LengthInSeconds();
            
           
        }
        else {
            nowPlayingText.text = "-----------------";
        }
	}

    string SecondsToMS(float seconds) {
        return string.Format("{0:D2}:{1:D2}", ((int)seconds)/60, ((int)seconds)%60);
    }
}
