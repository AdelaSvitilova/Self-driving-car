using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    private Button[] buttons;
    ColorBlock on;
    ColorBlock off;

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => LightButton(button));
        }
        on = buttons[0].colors;
        on.normalColor = buttons[0].colors.highlightedColor;
        off = buttons[0].colors;
        off.normalColor = buttons[0].colors.normalColor;
    }

    private void LightButton(Button btn)
    {
        foreach (Button button in buttons)
        {
            button.colors = off;
        }
        btn.colors = on;
    }
}
