using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] TMP_Dropdown sensorsCount;
    public void SetTrack(int track)
    {
        Setting.track = track;
    }

    public void ChangeSensorsCount()
    {
        Setting.sensorCount = int.Parse(sensorsCount.options[sensorsCount.value].text);
        Debug.Log(sensorsCount.options[sensorsCount.value].text);
    }


    public void StartSimulation()
    {
        SceneManager.LoadScene("Track" + Setting.track);
    }    
}
