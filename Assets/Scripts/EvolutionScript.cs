using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EvolutionScript : MonoBehaviour
{
    public GameObject carPrefab;

    [SerializeField] int populationSize, maxGenerations;
    private int parentsCount = 2;
    private float mutationProbability = 0.1f;

    private CarScript[] cars;
    private bool carsStillMove;
    private int generation;

    private NeuralNetwork[] bestCars;

    void Start()
    {
        generateFirstPopulation();
        bestCars = new NeuralNetwork[parentsCount];
    }

    void Update()
    {
        if (carsStillMove)
        {
            carsStillMove = false;
            foreach (CarScript car in cars)
            {
                if (!car.RideEnded)
                {
                    carsStillMove = true;
                    break;
                }
            }
            if (!carsStillMove)
            {
                //Debug.Log("Všechna auta dojela");
                Invoke(nameof(generateNewPopulation), 1.5f);
            }
        }
    }

    private void selection()
    {
        cars = cars.OrderByDescending(c => c.Fitness).ToArray();
        for (int i = 0; i < parentsCount; i++)
        {
            bestCars[i] = cars[i].NeuralNet;
        }
    }

    private void crossover()
    {
        foreach (CarScript car in cars)
        {
            int index = Random.Range(0, parentsCount);
            car.NeuralNet = bestCars[index].Clone();
            car.NeuralNet.Matation(mutationProbability);
        }
    }

    private void generateFirstPopulation()
    {
        cars = new CarScript[populationSize];
        for (int i = 0; i < populationSize; i++)
        {
            GameObject car = Instantiate(carPrefab, transform.position, transform.rotation);
            CarScript carScript = car.GetComponent<CarScript>();
            cars[i] = carScript;            
        }
        generation = 0;
        carsStillMove = true;
    }

    private void generateNewPopulation()
    {
        if(generation < maxGenerations)
        {
            selection();
            crossover();
            foreach (CarScript car in cars)
            {
                car.ResetCar();
            }
            generation++;
            carsStillMove = true;
        }        
    }
}
