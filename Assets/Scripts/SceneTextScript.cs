using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneTextScript : MonoBehaviour
{
    [SerializeField] TMP_Text staticText;
    [SerializeField] TMP_Text changingText;
    [SerializeField] TMP_Text generationText;

    private float bestFitness = 0;
    private float bestFitnessInGeneration;
    private EvolutionScript evolutionScript;

    void Start()
    {
        evolutionScript = GameObject.FindGameObjectWithTag("EvoManager").GetComponent<EvolutionScript>();
        staticText.text = "Pravd�podobnost mutace: " + Setting.mutationProbability.ToString() +
            " Velikost populace: " + Setting.populationSize.ToString() +
            " Po�et rodi�� pro novou generaci: " + Setting.parentsCount.ToString() +
            " Po�et skryt�ch vrstev neuronov� s�t�: " + Setting.hiddenLayersCount.ToString() +
            " Velikost skryt� vrstvy neuronov� s�t�: " + Setting.hiddenLayerSize.ToString();
    }
    
    void Update()
    {

        bestFitnessInGeneration = evolutionScript.BestCar.Fitness;

        if(bestFitnessInGeneration > bestFitness)
        {
            bestFitness = bestFitnessInGeneration;
        }

        changingText.text = "Aktu�ln� nejlep�� fitnes: "+ bestFitnessInGeneration.ToString("F2") +
            "\nCelkov� nejlep�� fitnes: " + bestFitness.ToString("F2");
    }

    public void ChangeGenerationText()
    {
        generationText.text = "Generace: " + evolutionScript.Generation.ToString();
    }
}
