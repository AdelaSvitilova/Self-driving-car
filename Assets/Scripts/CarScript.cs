using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    private float turn, speed;

    [SerializeField] float speedMultiplier, turnMultiplier;

    private Rigidbody rb;

    private float distance = 0f;
    private float duration = 0f;

    private Vector3 firstPosition;
    private Vector3 firstRotation;

    private Vector3 lastPosition;

    [SerializeField] float distanceMultiplier, durationMultiplier;
    [SerializeField] float maxRayDistance;
    [SerializeField] float maxTimeRide;

    public float Fitness { get; private set; }
    public bool RideEnded { get; private set; } = false;
    public NeuralNetwork NeuralNet { get; set; }

    [SerializeField] GameObject rayVisualizer;

    private List<LineRenderer> rayVisualizers;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        NeuralNet = new NeuralNetwork();
        firstPosition = transform.position;
        firstRotation = transform.eulerAngles;

        rayVisualizers = new List<LineRenderer>();
        for (int i = 0; i < 3; i++)
        {
            GameObject visualizer = Instantiate(rayVisualizer);
            visualizer.transform.parent = gameObject.transform;
            visualizer.transform.position = gameObject.transform.position;
            LineRenderer lr = visualizer.GetComponent<LineRenderer>();
            rayVisualizers.Add(lr);
        }
    }

    private void FixedUpdate()
    {
        if (!RideEnded)
        {
            UpdateFitnessInformation();
            float forwardSensor = SensorScanning(transform.forward, 0);
            float leftSensor = SensorScanning(transform.forward - transform.right, 1);
            float rightSensor = SensorScanning(transform.forward + transform.right, 2);

            NeuralNet.Predict(new float[] { leftSensor, forwardSensor, rightSensor }, out turn, out speed);
            Move();
        }        
    }

    private void Move()
    {
        //rb.AddRelativeForce(Vector3.forward * speed * speedMultiplier * Time.fixedDeltaTime * 100000f);
        //Vector3 currentPosition = transform.position;
        //transform.position = currentPosition + transform.forward * speed * speedMultiplier * Time.fixedDeltaTime;

        rb.velocity = transform.forward * speed * speedMultiplier * Time.fixedDeltaTime * 100f;

        transform.eulerAngles += new Vector3(0f, turn * turnMultiplier, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            EndRide();        
        }
    }

    private void CalculateFitness()
    {
        float avgSpeed = distance / duration;
        Fitness = distance * distanceMultiplier + avgSpeed * durationMultiplier;
    }

    private void UpdateFitnessInformation()
    {
        Vector3 currentPosition = transform.position;
        float distanceOfPositions = Vector3.Distance(currentPosition, lastPosition);
        distance += distanceOfPositions;
        lastPosition = currentPosition;
        duration += Time.fixedDeltaTime;

        if(duration > 5 && duration < 10 && Vector3.Distance(currentPosition, firstPosition) < 10)
        {
            distance = 0f;
            EndRide();
        }

        if(duration > maxTimeRide)
        {
            EndRide();
        }
    }

    private void EndRide()
    {
        RideEnded = true;
        CalculateFitness();
        rb.isKinematic = true;
        foreach (LineRenderer lr in rayVisualizers)
        {
            lr.enabled = false;
        }
    }


    private float SensorScanning(Vector3 direction, int visualizerIndex)
    {
        //nastaveni, aby pocatecni bod snimani nebyl uprostred auta, ale byl v predni casti auta
        Vector3 from = transform.position + (transform.forward * 2f);

        Ray ray = new Ray(from, direction);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, maxRayDistance))
        {
            //Debug.DrawRay(from, direction * hit.distance, Color.red);
            VisualizeRay(visualizerIndex, Color.red, from, hit.point);
        }
        else
        {
            //Debug.DrawRay(from, direction * maxRayDistance, Color.green);
            VisualizeRay(visualizerIndex, Color.green, from, from + direction * maxRayDistance);
        }
        return hit.distance / maxRayDistance;
    }

    private void VisualizeRay(int visualizerIndex, Color c, Vector3 from, Vector3 to)
    {
        rayVisualizers[visualizerIndex].endColor = c;
        rayVisualizers[visualizerIndex].startColor = c;
        rayVisualizers[visualizerIndex].SetPosition(0, from);
        rayVisualizers[visualizerIndex].SetPosition(1, to);
    }

    public void ResetCar()
    {
        transform.position = firstPosition;
        transform.eulerAngles = firstRotation;
        rb.isKinematic = false;

        foreach (LineRenderer lr in rayVisualizers)
        {
            lr.enabled = true;
        }

        Fitness = 0f;
        duration = 0f;
        distance = 0f;
        RideEnded = false;
    }

}
