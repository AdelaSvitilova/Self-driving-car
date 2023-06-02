using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text writeText;
    public void SetTrack(int track)
    {
        Setting.track = track;
    }

    public void SetHighlight(Button btn)
    {
        //btn.colors.normalColor = btn.colors.highlightedColor;

        ColorBlock colors = btn.colors;
        colors.normalColor = colors.highlightedColor;
        btn.colors = colors;
    }

    public void ChangeSensorsCount()
    {
        Setting.sensorCount = int.Parse(dropdown.options[dropdown.value].text);
    }


    public void StartSimulation()
    {
        SceneManager.LoadScene("Track" + Setting.track);
    }    

    public void ChangePopulationSize()
    {
        Setting.populationSize = ((int)slider.value);
        writeText.text = ((int)slider.value).ToString();
    }

    public void ChangeMutationProbability()
    {
        float value = Mathf.Round(slider.value * 100f) / 100f;
        Setting.mutationProbability = value;
        writeText.text = value.ToString();
    }

    public void ChangeSensorsLength()
    {
        float value = Mathf.Round(slider.value * 100f) / 100f;
        Setting.sensorLength = value;
        writeText.text = value.ToString();
    }

    public void ChangeHiddenLayerSize()
    {
        Setting.hiddenLayerSize = int.Parse(dropdown.options[dropdown.value].text);
    }

    public void ChangeHiddenLayerCount()
    {
        Setting.hiddenLayersCount = int.Parse(dropdown.options[dropdown.value].text);
    }

    public void ChangeParentsCount()
    {
        Setting.parentsCount = int.Parse(dropdown.options[dropdown.value].text);
    }
}
