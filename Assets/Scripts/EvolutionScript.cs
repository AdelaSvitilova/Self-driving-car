using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using TMPro;

public class EvolutionScript : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] int maxGenerations;
    private int parentsCount;

    public List<CarScript> cars { get; private set; }
    private bool carsStillMove;
    public int Generation { get; private set; }
    public CarScript BestCar { get; private set; }
    private float bestFitness = 0;

    private NeuralNetwork[] bestCars;
    [SerializeField] UnityEvent carRestarted;

    void Start()
    {
        parentsCount = Setting.parentsCount;
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
            FoundBestCar();
            if (!carsStillMove)
            {
                Invoke(nameof(GenerateNewPopulation), 1.5f);
            }
        }
    }

    private void FoundBestCar()
    {        
        foreach (CarScript car in cars)
        {
            if(car.Fitness > bestFitness)
            {
                bestFitness = car.Fitness;
                BestCar = car;
            }
        }
    }

    private void Selection()
    {
        cars = cars.OrderByDescending(c => c.Fitness).ToList();
        for (int i = 0; i < parentsCount; i++)
        {
            bestCars[i] = cars[i].NeuralNet.Clone();
        }
    }

    private void Crossover()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            int parent1 = Random.Range(0, parentsCount);
            int parent2 = Random.Range(0, parentsCount);
            while (parent1 == parent2)
            {
                parent2 = Random.Range(0, parentsCount);
            }
            cars[i].NeuralNet = bestCars[parent1].CrossoverWith(bestCars[parent2]);
            cars[i].NeuralNet.Mutation(Setting.mutationProbability);
        }
    }

    private void GenerateFirstPopulation()
    {
        cars = new List<CarScript>();
        for (int i = 0; i < Setting.populationSize; i++)
        {
            GameObject car = Instantiate(carPrefab, transform.position, transform.rotation);
            CarScript carScript = car.GetComponent<CarScript>();
            cars.Add(carScript);
        }
        Generation = 0;
        carsStillMove = true;
        BestCar = cars[0];
    }

    private void GenerateNewPopulation()
    {
        if(Generation < maxGenerations)
        {            
            Selection();
            Crossover();
            foreach (CarScript car in cars)
            {
                car.ResetCar();
            }
            Generation++;
            carsStillMove = true;
            bestFitness = 0;
            carRestarted.Invoke();
        }        
    }
}
