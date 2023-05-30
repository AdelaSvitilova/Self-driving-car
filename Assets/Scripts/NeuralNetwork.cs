using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;


public class NeuralNetwork
{
    //private Matrix<double> w1, w2;

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
        //w1 = Matrix<double>.Build.Random(3 + 1, 8, new ContinuousUniform(-1.0f, 1.0f));
        //w2 = Matrix<double>.Build.Random(8 + 1, 2, new ContinuousUniform(-1.0f, 1.0f));

        foreach (var w in weights)
        {
            //Debug.Log(w);
        }
    }

    public void Predict(float[] input, out float turn, out float speed)
    {
        Matrix<float> inputs = Matrix<float>.Build.Dense(1, input.Length, input);
        Matrix<double> neurons = inputs.ToDouble();
        Vector<double> one = Vector<double>.Build.Dense(1, 1);

        // Tenhle kod by se nìjak mohl opakovat
        /*neurons = neurons.InsertColumn(neurons.ColumnCount, one);
        neurons = neurons.Multiply(w1);
        for (int i = 0; i < neurons.ColumnCount; i++)
        {
            neurons[0, i] = SpecialFunctions.Logistic(neurons[0, i]);
        }
        // opakovat!!!!
        neurons = neurons.InsertColumn(neurons.ColumnCount, one);
        neurons = neurons.Multiply(w2);
        for (int i = 0; i < neurons.ColumnCount; i++)
        {
            neurons[0, i] = SpecialFunctions.Logistic(neurons[0, i]);
        }*/

        foreach (Matrix<double> weight in weights)
        {
            neurons = neurons.InsertColumn(neurons.ColumnCount, one);
            neurons = neurons.Multiply(weight);
            for (int i = 0; i < neurons.ColumnCount; i++)
            {
                neurons[0, i] = SpecialFunctions.Logistic(neurons[0, i]);
            }
        }

        //return array

        float[] output = neurons.ToSingle().ToRowMajorArray();
        speed = output[0];
        turn = output[1] - 0.5f;
    }

    public void Matation(float mutationProbability)
    {
        // sjednotit, aby se kod neopakoval
        /*for (int i = 0; i < w1.RowCount; i++)
        {
            for (int j = 0; j < w1.ColumnCount; j++)
            {
                //Debug.Log(Random.value);
                if(Random.value < mutationProbability)
                {
                    w1[i, j] = Random.Range(-1f, 1f);
                }
            }
        }

        for (int i = 0; i < w2.RowCount; i++)
        {
            for (int j = 0; j < w2.ColumnCount; j++)
            {
                //Debug.Log(Random.value);
                if (Random.value < mutationProbability)
                {
                    w2[i, j] = Random.Range(-1f, 1f);
                }
            }
        }*/

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

    /*public NeuralNetwork Clone()
    {
        Matrix<double> clonedW1 = w1.Clone();
        Matrix<double> clonedW2 = w2.Clone();
        NeuralNetwork clone = new NeuralNetwork(clonedW1, clonedW2);
        return clone;
    }*/

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
}
