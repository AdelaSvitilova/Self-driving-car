using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;


public class NeuralNetwork
{
    private List<Matrix<double>> weights;

    public NeuralNetwork(int sensorsCount = 3, int hiddenLayersCount = 1, int hiddenLayersSize = 6)
    {
        int[] neuronCounts = Generate.Repeat(hiddenLayersCount + 2, hiddenLayersSize);
        neuronCounts[0] = sensorsCount;
        neuronCounts[neuronCounts.Length - 1] = 2;
        
        weights = new List<Matrix<double>>();
        GenerateWeights(neuronCounts);
    }

    private NeuralNetwork(List<Matrix<double>> weights)
    {
        this.weights = weights;
    }

    private void GenerateWeights(int[] neuronCounts)
    {
        Matrix<double> weight;
        for (int i = 0; i < neuronCounts.Length - 1; i++)
        {
            weight = Matrix<double>.Build.Random(neuronCounts[i] + 1, neuronCounts[i+1], new ContinuousUniform(-1.0f, 1.0f));
            weights.Add(weight);
        }
    }

    public void Predict(float[] input, out float turn, out float speed)
    {
        Matrix<float> inputs = Matrix<float>.Build.Dense(1, input.Length, input);
        Matrix<double> neurons = inputs.ToDouble();
        Vector<double> one = Vector<double>.Build.Dense(1, 1);

        foreach (Matrix<double> weight in weights)
        {
            neurons = neurons.InsertColumn(neurons.ColumnCount, one);
            neurons = neurons.Multiply(weight);
            for (int i = 0; i < neurons.ColumnCount; i++)
            {
                neurons[0, i] = SpecialFunctions.Logistic(neurons[0, i]);
            }
        }

        float[] output = neurons.ToSingle().ToRowMajorArray();
        speed = output[0];
        turn = output[1] - 0.5f;
    }

    public void Mutation(float mutationProbability)
    {
        foreach (Matrix<double> weigth in weights)
        {
            for (int i = 0; i < weigth.RowCount; i++)
            {
                for (int j = 0; j < weigth.ColumnCount; j++)
                {
                    if(Random.value < mutationProbability)
                    {
                        weigth[i, j] = Random.Range(-1f, 1f);
                    }
                }

            }
        }
    }

    public NeuralNetwork Clone()
    {
        List<Matrix<double>> clonedWeights = new List<Matrix<double>>();
        foreach (Matrix<double> matrix in weights)
        {
            clonedWeights.Add(matrix.Clone());
        }
        NeuralNetwork clone = new NeuralNetwork(clonedWeights);
        return clone;
    }

    public NeuralNetwork CrossoverWith(NeuralNetwork nn)
    {
        List<Matrix<double>> clonedWeights = new List<Matrix<double>>();

        for (int m = 0; m < weights.Count; m++)
        {
            clonedWeights.Add(weights[m].Clone());

            for (int i = 0; i < clonedWeights[m].RowCount; i++)
            {
                for (int j = 0; j < clonedWeights[m].ColumnCount; j++)
                {
                    if (Random.value < 0.5)
                    {
                        clonedWeights[m][i, j] = nn.weights[m][i, j];
                    }
                }
            }
        }

        NeuralNetwork clone = new NeuralNetwork(clonedWeights);
        return clone;
    }
}
