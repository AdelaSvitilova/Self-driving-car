using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;


public class NeuralNetwork
{
    private Matrix<double> w1, w2;

    public NeuralNetwork()
    {
        int[] neuronCounts = new int[] { 3, 8, 2 };
        GenerateWeights(neuronCounts);
    }

    public NeuralNetwork(Matrix<double> w1, Matrix<double> w2)
    {
        this.w1 = w1;
        this.w2 = w2;
    }

    private void GenerateWeights(int[] NeuronCounts)
    {
        w1 = Matrix<double>.Build.Random(3 + 1, 8, new ContinuousUniform(-1.0f, 1.0f));
        w2 = Matrix<double>.Build.Random(8 + 1, 2, new ContinuousUniform(-1.0f, 1.0f));

        Debug.Log(w1);
        Debug.Log(w2);
    }

    public void Predict(float[] input, out float turn, out float speed)
    {
        Matrix<float> inputs = Matrix<float>.Build.Dense(1, input.Length, input);
        Matrix<double> neurons = inputs.ToDouble();
        Vector<double> one = Vector<double>.Build.Dense(1, 1);

        // Tenhle kod by se nìjak mohl opakovat
        neurons = neurons.InsertColumn(neurons.ColumnCount, one);
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
        }

        //return array

        float[] output = neurons.ToSingle().ToRowMajorArray();
        speed = output[0];
        turn = output[1] - 0.5f;
    }

    public void Matation(float mutationProbability)
    {
        // sjednotit, aby se kod neopakoval
        for (int i = 0; i < w1.RowCount; i++)
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
        }
    }

    public NeuralNetwork Clone()
    {
        Matrix<double> clonedW1 = w1.Clone();
        Matrix<double> clonedW2 = w2.Clone();
        NeuralNetwork clone = new NeuralNetwork(clonedW1, clonedW2);
        return clone;
    }
}
