using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMenuScript : MonoBehaviour
{
    void Start()
    {
        Setting.sensorCount = 3;
        Setting.track = null;
        Setting.populationSize = 20;
        Setting.mutationProbability = 0.1f;
        Setting.hiddenLayerSize = 6;
        Setting.hiddenLayersCount = 1;
        Setting.sensorLength = 15f;
    }
}
