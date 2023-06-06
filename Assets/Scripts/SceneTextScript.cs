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
    private GameObject bestCar;

    void Start()
    {
        bestCar = null;

        staticText.text = "Pravd�podobnost mutace: " + Setting.mutationProbability.ToString() +
            " Velikost populace: " + Setting.populationSize.ToString() +
            " Po�et rodi�� pro novou generaci: " + Setting.parentsCount.ToString() +
            " Po�et skryt�ch vrstev neuronov� s�t�: " + Setting.hiddenLayersCount.ToString() +
            " Velikost skryt� vrstvy neuronov� s�t�: " + Setting.hiddenLayerSize.ToString();
        cars = transform.GetComponent<EvolutionScript>().cars;
    }

    
    void Update()
    {
        float bestFit = -1f;

        foreach (CarScript car in cars)
        {
            if (car.Fitness > bestFit)
            {
                bestFit = car.Fitness;
                bestCar = car.gameObject;
            }
        }

        if(bestFit > bestFitness)
        {
            bestFitness = bestFit;
        }

        changingText.text = "Aktu�ln� nejlep�� fitnes: "+ bestFit.ToString("F2") +
            "\nCelkov� nejlep�� fitnes: " + bestFitness.ToString("F2");
    }

    public GameObject BestCar {
        get { return bestCar; }
    }
}
