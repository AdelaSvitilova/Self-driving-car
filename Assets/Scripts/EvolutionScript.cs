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
        carPrefab.GetComponent<CarScript>().sensorCount = Setting.sensorCount;
        GenerateFirstPopulation();
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
                Invoke(nameof(GenerateNewPopulation), 1.5f);
            }
        }
    }

    private void Selection()
    {
        cars = cars.OrderByDescending(c => c.Fitness).ToArray();
        for (int i = 0; i < parentsCount; i++)
        {
            bestCars[i] = cars[i].NeuralNet.Clone();
        }
    }

    private void CrossoverMutation()
    {
        foreach (CarScript car in cars)
        {
            int index = Random.Range(0, parentsCount);
            car.NeuralNet = bestCars[index].Clone();
            car.NeuralNet.Matation(mutationProbability);
        }
    }

    private void Crossover()
    {
        for (int i = parentsCount; i < populationSize; i++)
        {
            int parent1 = Random.Range(0, parentsCount);
            int parent2 = Random.Range(0, parentsCount);
            while (parent1 == parent2)
            {
                parent2 = Random.Range(0, parentsCount);
            }
            cars[i].NeuralNet = bestCars[parent1].CrossoverWith(bestCars[parent2]);
            cars[i].NeuralNet.Matation(mutationProbability);
        }
    }

    private void GenerateFirstPopulation()
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

    private void GenerateNewPopulation()
    {
        if(generation < maxGenerations)
        {
            Selection();
            //CrossoverMutation();
            Crossover();
            foreach (CarScript car in cars)
            {
                car.ResetCar();
            }
            generation++;
            carsStillMove = true;
        }        
    }
}
