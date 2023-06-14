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
        staticText.text = "Pravdìpodobnost mutace: " + Setting.mutationProbability.ToString() +
            " Velikost populace: " + Setting.populationSize.ToString() +
            " Poèet rodièù pro novou generaci: " + Setting.parentsCount.ToString() +
            " Poèet skrytých vrstev neuronové sítì: " + Setting.hiddenLayersCount.ToString() +
            " Velikost skryté vrstvy neuronové sítì: " + Setting.hiddenLayerSize.ToString();
    }
    
    void Update()
    {

        bestFitnessInGeneration = evolutionScript.BestCar.Fitness;

        if(bestFitnessInGeneration > bestFitness)
        {
            bestFitness = bestFitnessInGeneration;
        }

        changingText.text = "Aktuální nejlepší fitnes: "+ bestFitnessInGeneration.ToString("F2") +
            "\nCelkové nejlepší fitnes: " + bestFitness.ToString("F2");
    }

    public void ChangeGenerationText()
    {
        generationText.text = "Generace: " + evolutionScript.Generation.ToString();
    }
}
