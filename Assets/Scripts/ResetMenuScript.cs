using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMenuScript : MonoBehaviour
{
    void Start()
    {
        Setting.sensorCount = 3;
        Setting.track = null;
    }
}
