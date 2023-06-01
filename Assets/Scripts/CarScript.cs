using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    private float turn, speed;

    [SerializeField] float speedMultiplier, turnMultiplier;

    private Rigidbody rb;

    public float Distance { get; private set; } = 0f;
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

    //[SerializeField] int sensorCount;
    public int sensorCount;
    [SerializeField] GameObject rayVisualizer;
    private List<LineRenderer> rayVisualizers;
    private List<Vector3> rayDirections;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        NeuralNet = new NeuralNetwork(sensorCount);
        firstPosition = transform.position;
        firstRotation = transform.eulerAngles;
        GenerateRay();
    }

    private void GenerateRay()
    {
        rayVisualizers = new List<LineRenderer>();
        for (int i = 0; i < sensorCount; i++)
        {
            GameObject visualizer = Instantiate(rayVisualizer);
            visualizer.transform.parent = gameObject.transform;
            visualizer.transform.position = gameObject.transform.position;
            LineRenderer lr = visualizer.GetComponent<LineRenderer>();
            rayVisualizers.Add(lr);
        }

        rayDirections = new List<Vector3>();
        if (sensorCount == 1)
        {
            rayDirections.Add(transform.forward);
        }
        if (sensorCount == 3)
        {
            rayDirections.Add((transform.forward - transform.right).normalized);
            rayDirections.Add(transform.forward);
            rayDirections.Add((transform.forward + transform.right).normalized);
        }
        if (sensorCount == 5)
        {
            rayDirections.Add((transform.forward - transform.right).normalized);
            rayDirections.Add((2 * transform.forward - transform.right).normalized);
            rayDirections.Add(transform.forward);
            rayDirections.Add((2 * transform.forward + transform.right).normalized);
            rayDirections.Add((transform.forward + transform.right).normalized);
        }
    }

    private void FixedUpdate()
    {
        if (!RideEnded)
        {
            UpdateFitnessInformation();

            float[] sensorValues = new float[sensorCount];
            for (int i = 0; i < sensorCount; i++)
            {
                sensorValues[i] = SensorScanning(i);
            }

            NeuralNet.Predict(sensorValues, out turn, out speed);
            Move();
        }        
    }

    private void Move()
    {
        Vector3 currentPosition = transform.position;
        //transform.position = currentPosition + transform.forward * speed * speedMultiplier * Time.fixedDeltaTime;
        transform.position = Vector3.Lerp(currentPosition, currentPosition + transform.forward * speed * speedMultiplier, Time.fixedDeltaTime);
        //rb.velocity = transform.forward * speed * speedMultiplier * Time.fixedDeltaTime * 100f;
        //transform.position = transform.TransformDirection(Vector3.Lerp(Vector3.zero, transform.forward * speed * speedMultiplier, Time.fixedDeltaTime));

        transform.eulerAngles += new Vector3(0f, turn * turnMultiplier, 0f);
        //Quaternion desiredRotation = Quaternion.Euler(0f, turn * turnMultiplier, 0f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime);
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
        float avgSpeed = Distance / duration;
        Fitness = Distance * distanceMultiplier + avgSpeed * durationMultiplier;
    }

    private void UpdateFitnessInformation()
    {
        Vector3 currentPosition = transform.position;
        float distanceOfPositions = Vector3.Distance(currentPosition, lastPosition);
        Distance += distanceOfPositions;
        lastPosition = currentPosition;
        duration += Time.fixedDeltaTime;

        if(duration > 7 && duration < 10 && Vector3.Distance(currentPosition, firstPosition) < 10)
        {
            Distance = 0f;
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


    private float SensorScanning(int rayIndex)
    {
        //nastaveni, aby pocatecni bod snimani nebyl uprostred auta, ale byl v predni casti auta
        Vector3 from = transform.position + (transform.forward * 2f) + (transform.up);
        Vector3 direction = transform.TransformDirection(rayDirections[rayIndex]);

        Ray ray = new Ray(from, direction);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, maxRayDistance))
        {
            //Debug.DrawRay(from, direction * hit.distance, Color.red);
            if (hit.distance > (3f / 4f * maxRayDistance))
            {
                VisualizeRay(rayIndex, Color.yellow, from, hit.point);
            }
            else
            {
                VisualizeRay(rayIndex, Color.red, from, hit.point);
            }
        }
        else
        {
            //Debug.DrawRay(from, direction * maxRayDistance, Color.green);
            if(Physics.Raycast(ray, maxRayDistance + 3f))
            {
                VisualizeRay(rayIndex, Color.yellow, from, from + direction * maxRayDistance);
            }
            else
            {
                VisualizeRay(rayIndex, Color.green, from, from + direction * maxRayDistance);
            }
            
        }
        return hit.distance / maxRayDistance;
    }

    private void VisualizeRay(int visualizerIndex, Color c, Vector3 from, Vector3 to)
    {        
        rayVisualizers[visualizerIndex].SetPosition(0, from);
        rayVisualizers[visualizerIndex].SetPosition(1, to);
        rayVisualizers[visualizerIndex].endColor = c;
        rayVisualizers[visualizerIndex].startColor = c;
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
        Distance = 0f;
        RideEnded = false;
    }
}
