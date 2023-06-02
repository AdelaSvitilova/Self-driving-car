using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneTextScript : MonoBehaviour
{
    [SerializeField] TMP_Text staticText;
    [SerializeField] TMP_Text changingText;

    private List<CarScript> cars;

    private float bestFitness;

    void Start()
    {
        staticText.text = "Pravdìpodobnost mutace: " + Setting.mutationProbability.ToString() +
            " Velikost populace: " + Setting.populationSize.ToString() +
            " Poèet rodièù pro novou generaci: " + Setting.parentsCount.ToString() +
            " Poèet skrytých vrstev neuronové sítì: " + Setting.hiddenLayersCount.ToString() +
            " Velikost skryté vrstvy neuronové sítì: " + Setting.hiddenLayerSize.ToString();
        cars = transform.GetComponent<EvolutionScript>().cars;
    }

    
    void Update()
    {
        float bestFit = -1f;

        foreach (CarScript car in cars)
        {
            if (car.Distance > bestFit)
            {
                bestFit = car.Distance;
            }
        }

        if(bestFit > bestFitness)
        {
            bestFitness = bestFit;
        }

        changingText.text = "Aktuální nejlepší fitnes: "+ bestFit.ToString("F2") +
            "\nCelkové nejlepší fitnes: " + bestFitness.ToString("F2");
    }
}
